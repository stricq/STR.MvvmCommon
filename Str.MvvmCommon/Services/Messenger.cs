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
      public IWeakFunc Action;

      public object? Token;
    }

    #endregion Private Fields

    #region IMessenger Implementation

    #region Register

    public void Register<TMessage>(IMessageReceiver Recipient, Func<TMessage, Task> Action) {
      Register(Recipient, null, false, Action);
    }

    public void Register<TMessage>(IMessageReceiver Recipient, bool ReceiveDerivedMessagesToo, Func<TMessage, Task> Action) {
      Register(Recipient, null, ReceiveDerivedMessagesToo, Action);
    }

    public void Register<TMessage>(IMessageReceiver Recipient, object? Token, Func<TMessage, Task> Action) {
      Register(Recipient, Token, false, Action);
    }

    public void Register<TMessage>(IMessageReceiver Recipient, object? Token, bool ReceiveDerivedMessagesToo, Func<TMessage, Task> Action) {
      lock(registerLock) {
        Type messageType = typeof(TMessage);

        if (messageType.IsGenericType) messageType = messageType.GetGenericTypeDefinition();

        Dictionary<Type, List<WeakFuncAndToken>> recipients = ReceiveDerivedMessagesToo ? recipientsOfSubclassesAction : recipientsStrictAction;

        lock(recipients) {
          List<WeakFuncAndToken> list;

          if (!recipients.ContainsKey(messageType)) {
            list = new List<WeakFuncAndToken>();

            recipients.Add(messageType, list);
          }
          else list = recipients[messageType];

          WeakFunc<TMessage> weakAction = new WeakFunc<TMessage>(Recipient, Action);

          WeakFuncAndToken item = new WeakFuncAndToken {
            Action = weakAction,
            Token  = Token
          };

          list.Add(item);
        }
      }

      RequestCleanup();
    }

    #endregion Register

    #region Send

    public Task SendAsync<TMessage>(TMessage Message) {
      return SendToTargetAsync(Message, null);
    }

    public Task SendAsync<TMessage>(TMessage Message, object? Token) {
      return SendToTargetAsync(Message, Token);
    }

    #endregion Send

    #region SendOnUiThread

    public Task SendOnUiThreadAsync<TMessage>(TMessage Message) {
      return TaskHelper.RunOnUiThreadAsync(() => SendToTargetAsync(Message, null));
    }

    public Task SendOnUiThreadAsync<TMessage>(TMessage Message, object Token) {
      return TaskHelper.RunOnUiThreadAsync(() => SendToTargetAsync(Message, Token));
    }

    #endregion SendOnUiThread

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

    private static Task SendToListAsync<TMessage>(TMessage message, IEnumerable<WeakFuncAndToken> list, object? token) {
      //
      // Clone to protect from people registering in a "receive message" method
      //
      List<WeakFuncAndToken> listClone = list.ToList();

      Task asyncTask = listClone.ForEachAsync(item => {
        if (item.Action.IsAlive && ((item.Token == null && token == null) || item.Token != null && item.Token.Equals(token))) {
          return (item.Action as WeakFunc<TMessage>)!.ExecuteWithObjectAsync(message);
        }

        return Task.CompletedTask;
      });

      return asyncTask;
    }

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

    private static void UnregisterFromLists(IMessageReceiver recipient, Dictionary<Type, List<WeakFuncAndToken>> list) {
      if (list.Count == 0) return;

      lock(list) {
        foreach(Type messageType in list.Keys) {
          foreach(WeakFuncAndToken item in list[messageType]) {
            IWeakFunc weakAction = item.Action;

            if (recipient == weakAction.Target) weakAction.MarkForDeletion();
          }
        }
      }
    }

    private static void UnregisterFromLists<TMessage>(IMessageReceiver recipient, object? token, Func<TMessage, Task>? action, Dictionary<Type, List<WeakFuncAndToken>> list) {
      Type messageType = typeof(TMessage);

      if (list.Count == 0 || !list.ContainsKey(messageType)) return;

      lock(list) {
        foreach(WeakFuncAndToken item in list[messageType]) {
          if (recipient == item.Action.Target && (action == null || action.Method.Name == item.Action.MethodName) && (token  == null || token.Equals(item.Token))) {
            item.Action.MarkForDeletion();
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

        foreach((Type type, List<WeakFuncAndToken> weakActionAndTokens) in dictionary) {
          List<WeakFuncAndToken> recipientsToRemove = weakActionAndTokens.Where(item => !item.Action.IsAlive).ToList();

          foreach(WeakFuncAndToken recipient in recipientsToRemove) weakActionAndTokens.Remove(recipient);

          if (weakActionAndTokens.Count == 0) listsToRemove.Add(type);
        }

        foreach(Type key in listsToRemove) dictionary.Remove(key);
      }
    }

    #endregion Private Methods

  }

}
