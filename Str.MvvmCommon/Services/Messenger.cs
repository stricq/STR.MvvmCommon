using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

using Str.Common.Extensions;

using Str.MvvmCommon.Contracts;
using Str.MvvmCommon.Helpers;


namespace Str.MvvmCommon.Services {
  //
  // This code is based on code found in MvvmLight.
  //
  // https://github.com/lbugnion/mvvmlight/tree/master/GalaSoft.MvvmLight/GalaSoft.MvvmLight%20(PCL)/Messaging
  //
  [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
  public class Messenger : IMessenger {

    #region Private Fields

    private bool isCleanupRegistered;

    private readonly Dictionary<Type, List<WeakFuncAndToken>> recipientsOfSubclassesAction = new Dictionary<Type, List<WeakFuncAndToken>>();
    private readonly Dictionary<Type, List<WeakFuncAndToken>> recipientsStrictAction       = new Dictionary<Type, List<WeakFuncAndToken>>();

    private readonly object registerLock = new object();

    private struct WeakFuncAndToken {
      public IWeakFunc Func;

      public object? Token;
    }

    #endregion Private Fields

    #region IMessenger Implementation

    #region Register

    public void Register<TMessage>(IMessageReceiver recipient, Func<TMessage, Task> action) {
      Register(recipient, null, false, action);
    }

    public void Register<TMessage>(IMessageReceiver recipient, bool receiveDerivedMessagesToo, Func<TMessage, Task> action) {
      Register(recipient, null, receiveDerivedMessagesToo, action);
    }

    public void Register<TMessage>(IMessageReceiver recipient, object? token, Func<TMessage, Task> action) {
      Register(recipient, token, false, action);
    }

    public void Register<TMessage>(IMessageReceiver recipient, object? token, bool receiveDerivedMessagesToo, Func<TMessage, Task> action) {
      lock(registerLock) {
        Type messageType = typeof(TMessage);

        if (messageType.IsGenericType) messageType = messageType.GetGenericTypeDefinition();

        Dictionary<Type, List<WeakFuncAndToken>> recipients = receiveDerivedMessagesToo ? recipientsOfSubclassesAction : recipientsStrictAction;

        lock(recipients) {
          List<WeakFuncAndToken> list;

          if (!recipients.ContainsKey(messageType)) {
            list = new List<WeakFuncAndToken>();

            recipients.Add(messageType, list);
          }
          else list = recipients[messageType];

          WeakFunc<TMessage> weakFunc = new WeakFunc<TMessage>(recipient, action);

          WeakFuncAndToken item = new WeakFuncAndToken {
            Func = weakFunc,
            Token  = token
          };

          list.Add(item);
        }
      }

      RequestCleanup();
    }

    #endregion Register

    #region SendAsync

    public Task SendAsync<TMessage>(TMessage message, object? token = default) {
      return SendToTargetAsync(message, token);
    }

    #endregion SendAsync

    #region SendOnUiThreadAsync

    public Task SendOnUiThreadAsync<TMessage>(TMessage message, object? token = default) {
      return TaskHelper.RunOnUiThreadAsync(() => SendToTargetAsync(message, token));
    }

    #endregion SendOnUiThreadAsync

    #region Unregister

    public void Unregister(IMessageReceiver recipient) {
      UnregisterFromLists(recipient, recipientsStrictAction);
      UnregisterFromLists(recipient, recipientsOfSubclassesAction);

      RequestCleanup();
    }

    public void Unregister<TMessage>(IMessageReceiver recipient) {
      UnregisterFromLists<TMessage>(recipient, null, null, recipientsStrictAction);
      UnregisterFromLists<TMessage>(recipient, null, null, recipientsOfSubclassesAction);

      RequestCleanup();
    }

    public void Unregister<TMessage>(IMessageReceiver recipient, Func<TMessage, Task> action) {
      Unregister(recipient, null, action);
    }

    public void Unregister<TMessage>(IMessageReceiver recipient, object? token, Func<TMessage, Task> action) {
      UnregisterFromLists(recipient, token, action, recipientsStrictAction);
      UnregisterFromLists(recipient, token, action, recipientsOfSubclassesAction);

      RequestCleanup();
    }

    #endregion Unregister

    #endregion IMessenger Implementation

    #region Private Methods

    private async Task SendToTargetAsync<TMessage>(TMessage message, object? token) {
      Type messageType = typeof(TMessage);

      if (messageType.IsGenericType) messageType = messageType.GetGenericTypeDefinition();
      //
      // Clone to protect from people registering in a "receive message" method
      //
      Dictionary<Type, List<WeakFuncAndToken>> clone = recipientsOfSubclassesAction.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

      foreach(Type type in clone.Keys) {
        if (messageType != type && !messageType.IsSubclassOf(type) && !type.IsAssignableFrom(messageType) && !type.IsSubclassOf(messageType) && !messageType.IsAssignableFrom(type)) continue;

        List<WeakFuncAndToken> list = clone[type].ToList();

        await SendToListAsync(message, list, token).Fire();
      }

      clone = recipientsStrictAction.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

      if (clone.ContainsKey(messageType)) {
        List<WeakFuncAndToken> list = clone[messageType].ToList();

        await SendToListAsync(message, list, token).Fire();
      }

      RequestCleanup();
    }

    private static async Task SendToListAsync<TMessage>(TMessage message, IEnumerable<WeakFuncAndToken> list, object? token) {
      //
      // Clone to protect from people registering in a "receive message" method
      //
      List<WeakFuncAndToken> listClone = list.ToList();

      foreach(WeakFuncAndToken item in listClone) {
        if (item.Func.IsAlive && ((item.Token == null && token == null) || item.Token != null && item.Token.Equals(token))) {
          await (item.Func as WeakFunc<TMessage>)!.ExecuteWithObjectAsync(message).Fire();
        }
      }
    }

    private static void UnregisterFromLists(IMessageReceiver recipient, Dictionary<Type, List<WeakFuncAndToken>> list) {
      if (list.Count == 0) return;

      lock(list) {
        foreach(Type messageType in list.Keys) {
          foreach(WeakFuncAndToken item in list[messageType]) {
            IWeakFunc weakFunc = item.Func;

            if (recipient == weakFunc.Target) weakFunc.MarkForDeletion();
          }
        }
      }
    }

    private static void UnregisterFromLists<TMessage>(IMessageReceiver recipient, object? token, Func<TMessage, Task>? action, Dictionary<Type, List<WeakFuncAndToken>> list) {
      Type messageType = typeof(TMessage);

      if (list.Count == 0 || !list.ContainsKey(messageType)) return;

      lock(list) {
        foreach(WeakFuncAndToken item in list[messageType]) {
          if (recipient == item.Func.Target && (action == null || action.Method.Name == item.Func.MethodName) && (token  == null || token.Equals(item.Token))) {
            item.Func.MarkForDeletion();
          }
        }
      }
    }

    private void RequestCleanup() {
      if (isCleanupRegistered) return;

      Action cleanupAction = Cleanup;

      Dispatcher.CurrentDispatcher.BeginInvoke(cleanupAction, DispatcherPriority.ApplicationIdle, null);

      isCleanupRegistered = true;
    }

    private void Cleanup() {
      CleanupList(recipientsOfSubclassesAction);
      CleanupList(recipientsStrictAction);

      isCleanupRegistered = false;
    }

    private static void CleanupList(Dictionary<Type, List<WeakFuncAndToken>> dictionary) {
      lock(dictionary) {
        List<Type> listsToRemove = new List<Type>();

        foreach((Type type, List<WeakFuncAndToken> weakFuncAndTokens) in dictionary) {
          List<WeakFuncAndToken> recipientsToRemove = weakFuncAndTokens.Where(item => !item.Func.IsAlive).ToList();

          foreach(WeakFuncAndToken recipient in recipientsToRemove) weakFuncAndTokens.Remove(recipient);

          if (weakFuncAndTokens.Count == 0) listsToRemove.Add(type);
        }

        foreach(Type key in listsToRemove) dictionary.Remove(key);
      }
    }

    #endregion Private Methods

  }

}
