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
  [SuppressMessage("ReSharper", "InconsistentlySynchronizedField", Justification = "Always synchronized.")]
  [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
  public class Messenger : IMessenger {

    #region Private Fields

    private bool isCleanupRegistered;

    private Dictionary<Type, List<WeakActionAndToken>> recipientsOfSubclassesAction;
    private Dictionary<Type, List<WeakActionAndToken>> recipientsStrictAction;

    private readonly object registerLock = new object();

    private struct WeakActionAndToken {
      public WeakAction Action;

      public object Token;
    }

    #endregion Private Fields

    #region IMessenger Implementation

    public void Register<TMessage>(object Recipient, Action<TMessage> Action) {
      Register(Recipient, null, false, Action);
    }

    public void Register<TMessage>(object Recipient, Func<TMessage, Task> Action) {
      Register(Recipient, null, false, Action);
    }

    public void Register<TMessage>(object Recipient, bool ReceiveDerivedMessagesToo, Action<TMessage> Action) {
      Register(Recipient, null, ReceiveDerivedMessagesToo, Action);
    }

    public void Register<TMessage>(object Recipient, bool ReceiveDerivedMessagesToo, Func<TMessage, Task> Action) {
      Register(Recipient, null, ReceiveDerivedMessagesToo, Action);
    }

    public void Register<TMessage>(object Recipient, object Token, Action<TMessage> Action) {
      Register(Recipient, Token, false, Action);
    }

    public void Register<TMessage>(object Recipient, object Token, Func<TMessage, Task> Action) {
      Register(Recipient, Token, false, Action);
    }

    public void Register<TMessage>(object Recipient, object Token, bool ReceiveDerivedMessagesToo, Action<TMessage> Action) {
      lock(registerLock) {
        Type messageType = typeof(TMessage);

        if (messageType.IsGenericType) messageType = messageType.GetGenericTypeDefinition();

        Dictionary<Type, List<WeakActionAndToken>> recipients;

        if (ReceiveDerivedMessagesToo) {
          if (recipientsOfSubclassesAction == null) recipientsOfSubclassesAction = new Dictionary<Type, List<WeakActionAndToken>>();

          recipients = recipientsOfSubclassesAction;
        }
        else {
          if (recipientsStrictAction == null) recipientsStrictAction = new Dictionary<Type, List<WeakActionAndToken>>();

          recipients = recipientsStrictAction;
        }

        lock(recipients) {
          List<WeakActionAndToken> list;

          if (!recipients.ContainsKey(messageType)) {
            list = new List<WeakActionAndToken>();

            recipients.Add(messageType, list);
          }
          else list = recipients[messageType];

          WeakAction<TMessage> weakAction = new WeakAction<TMessage>(Recipient, Action);

          WeakActionAndToken item = new WeakActionAndToken {
            Action = weakAction,
            Token  = Token
          };

          list.Add(item);
        }
      }

      RequestCleanup();
    }

    public void Register<TMessage>(object Recipient, object Token, bool ReceiveDerivedMessagesToo, Func<TMessage, Task> Action) {
      lock(registerLock) {
        Type messageType = typeof(TMessage);

        if (messageType.IsGenericType) messageType = messageType.GetGenericTypeDefinition();

        Dictionary<Type, List<WeakActionAndToken>> recipients;

        if (ReceiveDerivedMessagesToo) {
          if (recipientsOfSubclassesAction == null) recipientsOfSubclassesAction = new Dictionary<Type, List<WeakActionAndToken>>();

          recipients = recipientsOfSubclassesAction;
        }
        else {
          if (recipientsStrictAction == null) recipientsStrictAction = new Dictionary<Type, List<WeakActionAndToken>>();

          recipients = recipientsStrictAction;
        }

        lock(recipients) {
          List<WeakActionAndToken> list;

          if (!recipients.ContainsKey(messageType)) {
            list = new List<WeakActionAndToken>();

            recipients.Add(messageType, list);
          }
          else list = recipients[messageType];

          WeakFunc<TMessage> weakAction = new WeakFunc<TMessage>(Recipient, Action);

          WeakActionAndToken item = new WeakActionAndToken {
            Action = weakAction,
            Token  = Token
          };

          list.Add(item);
        }
      }

      RequestCleanup();
    }

    public void Send<TMessage>(TMessage Message) {
      SendToTargetOrType(Message, null, null);
    }

    public void Send<TMessage>(TMessage Message, object Token) {
      SendToTargetOrType(Message, null, Token);
    }

    public Task SendAsync<TMessage>(TMessage Message) {
      return SendToTargetOrTypeAsync(Message, null, null);
    }

    public Task SendAsync<TMessage>(TMessage Message, object Token) {
      return SendToTargetOrTypeAsync(Message, null, Token);
    }

    public Task SendOnUiThreadAsync<TMessage>(TMessage Message) {
      return TaskHelper.RunOnUiThreadAsync(() => SendToTargetOrType(Message, null, null));
    }

    public Task SendOnUiThreadAsync<TMessage>(TMessage Message, object Token) {
      return TaskHelper.RunOnUiThreadAsync(() => SendToTargetOrType(Message, null, Token));
    }

    public void Unregister(object Recipient) {
      UnregisterFromLists(Recipient, recipientsOfSubclassesAction);
      UnregisterFromLists(Recipient, recipientsStrictAction);
    }

    public void Unregister<TMessage>(object Recipient) {
      Unregister<TMessage>(Recipient, null, null);
    }

    public void Unregister<TMessage>(object Recipient, object Token) {
      Unregister<TMessage>(Recipient, Token, null);
    }

    public void Unregister<TMessage>(object Recipient, Action<TMessage> Action) {
      Unregister(Recipient, null, Action);
    }

    public void Unregister<TMessage>(object Recipient, Func<TMessage, Task> Action) {
      Unregister(Recipient, null, Action);
    }

    public void Unregister<TMessage>(object Recipient, object Token, Action<TMessage> Action) {
      UnregisterFromLists(Recipient, Token, Action, recipientsStrictAction);
      UnregisterFromLists(Recipient, Token, Action, recipientsOfSubclassesAction);

      RequestCleanup();
    }

    public void Unregister<TMessage>(object Recipient, object Token, Func<TMessage, Task> Action) {
      UnregisterFromLists(Recipient, Token, Action, recipientsStrictAction);
      UnregisterFromLists(Recipient, Token, Action, recipientsOfSubclassesAction);

      RequestCleanup();
    }

    #endregion IMessenger Implementation

    #region Private Methods

    private static void CleanupList(Dictionary<Type, List<WeakActionAndToken>> lists) {
      if (lists == null) return;

      lock(lists) {
        List<Type> listsToRemove = new List<Type>();

        foreach((Type type, List<WeakActionAndToken> weakActionAndTokens) in lists) {
          List<WeakActionAndToken> recipientsToRemove = weakActionAndTokens.Where(item => item.Action == null || !item.Action.IsAlive).ToList();

          foreach(WeakActionAndToken recipient in recipientsToRemove) weakActionAndTokens.Remove(recipient);

          if (weakActionAndTokens.Count == 0) listsToRemove.Add(type);
        }

        foreach(Type key in listsToRemove) lists.Remove(key);
      }
    }

    private static void SendToList<TMessage>(TMessage message, IReadOnlyCollection<WeakActionAndToken> list, Type messageTargetType, object token) {
      if (list == null) return;
      //
      // Clone to protect from people registering in a "receive message" method
      //
      List<WeakActionAndToken> listClone = list.ToList();

      foreach(WeakActionAndToken item in listClone) {
        if (item.Action is IExecuteWithObject executeAction && item.Action.IsAlive && item.Action.Target != null
                                  && (messageTargetType == null || item.Action.Target.GetType() == messageTargetType || messageTargetType.IsInstanceOfType(item.Action.Target))
                                  && ((item.Token == null && token == null) || item.Token != null && item.Token.Equals(token))) {
          executeAction.ExecuteWithObject(message);
        }
      }
    }

    private static Task SendToListAsync<TMessage>(TMessage message, IReadOnlyCollection<WeakActionAndToken> list, Type messageTargetType, object token) {
      if (list == null) return Task.CompletedTask;
      //
      // Clone to protect from people registering in a "receive message" method
      //
      List<WeakActionAndToken> listClone = list.ToList();

      Task asyncTask = listClone.ForEachAsync(item => {
        if (item.Action is IExecuteWithObjectAsync executeActionAsync && item.Action.IsAlive && item.Action.Target != null
            && (messageTargetType == null || item.Action.Target.GetType() == messageTargetType || messageTargetType.IsInstanceOfType(item.Action.Target))
            && ((item.Token == null && token == null) || item.Token != null && item.Token.Equals(token))) {
          return executeActionAsync.ExecuteWithObjectAsync(message);
        }

        return Task.FromResult(0);
      });

      Task syncTask = listClone.ForEachAsync(item => {
        if (item.Action is IExecuteWithObject executeAction && item.Action.IsAlive && item.Action.Target != null
            && (messageTargetType == null || item.Action.Target.GetType() == messageTargetType || messageTargetType.IsInstanceOfType(item.Action.Target))
            && ((item.Token == null && token == null) || item.Token != null && item.Token.Equals(token))) {
          return Task.Run(() => executeAction.ExecuteWithObject(message));
        }

        return Task.FromResult(0);
      });

      return Task.WhenAll(new List<Task> { asyncTask, syncTask });
    }

    private void SendToTargetOrType<TMessage>(TMessage message, Type messageTargetType, object token) {
      Type messageType = typeof(TMessage);

      if (messageType.IsGenericType) messageType = messageType.GetGenericTypeDefinition();

      if (recipientsOfSubclassesAction != null) {
        //
        // Clone to protect from people registering in a "receive message" method
        //
        List<Type> listClone = recipientsOfSubclassesAction.Keys.ToList();

        foreach(Type type in listClone) {
          List<WeakActionAndToken> list = null;

          if (messageType == type || messageType.IsSubclassOf(type) || type.IsAssignableFrom(messageType) || type.IsSubclassOf(messageType) || messageType.IsAssignableFrom(type)) {
            lock(recipientsOfSubclassesAction) {
              list = recipientsOfSubclassesAction[type].ToList();
            }
          }

          SendToList(message, list, messageTargetType, token);
        }
      }

      if (recipientsStrictAction != null) {
        lock(recipientsStrictAction) {
          if (recipientsStrictAction.ContainsKey(messageType)) {
            List<WeakActionAndToken> list = recipientsStrictAction[messageType].ToList();

            SendToList(message, list, messageTargetType, token);
          }
        }
      }

      RequestCleanup();
    }

    private async Task SendToTargetOrTypeAsync<TMessage>(TMessage message, Type messageTargetType, object token) {
      Type messageType = typeof(TMessage);

      if (messageType.IsGenericType) messageType = messageType.GetGenericTypeDefinition();

      if (recipientsOfSubclassesAction != null) {
        //
        // Clone to protect from people registering in a "receive message" method
        //
        List<Type> listClone = recipientsOfSubclassesAction.Keys.ToList();

        foreach(Type type in listClone) {
          List<WeakActionAndToken> list = null;

          if (messageType == type || messageType.IsSubclassOf(type) || type.IsAssignableFrom(messageType) || type.IsSubclassOf(messageType) || messageType.IsAssignableFrom(type)) {
            lock(recipientsOfSubclassesAction) {
              list = recipientsOfSubclassesAction[type].ToList();
            }
          }

          await SendToListAsync(message, list, messageTargetType, token).Fire();
        }
      }

      if (recipientsStrictAction != null) {
        List<WeakActionAndToken> list = null;

        lock(recipientsStrictAction) {
          if (recipientsStrictAction.ContainsKey(messageType)) {
            list = recipientsStrictAction[messageType].ToList();
          }
        }

        await SendToListAsync(message, list, messageTargetType, token).Fire();
      }

      RequestCleanup();
    }

    private static void UnregisterFromLists(object recipient, Dictionary<Type, List<WeakActionAndToken>> lists) {
      if (recipient == null || lists == null || lists.Count == 0) return;

      lock(lists) {
        foreach(Type messageType in lists.Keys) {
          foreach(WeakActionAndToken item in lists[messageType]) {
            IExecuteWithObject weakAction = (IExecuteWithObject)item.Action;

            if (weakAction != null && recipient == weakAction.Target) weakAction.MarkForDeletion();
          }
        }
      }
    }

    private static void UnregisterFromLists<TMessage>(object recipient, object token, Action<TMessage> action, Dictionary<Type, List<WeakActionAndToken>> lists) {
      Type messageType = typeof(TMessage);

      if (recipient == null || lists == null || lists.Count == 0 || !lists.ContainsKey(messageType)) return;

      lock(lists) {
        foreach(WeakActionAndToken item in lists[messageType]) {
          if (item.Action is WeakAction<TMessage> weakActionCasted && recipient == weakActionCasted.Target
                                       && (action == null || action.Method.Name == weakActionCasted.MethodName)
                                       && (token  == null || token.Equals(item.Token))) {
            item.Action.MarkForDeletion();
          }
        }
      }
    }

    private static void UnregisterFromLists<TMessage>(object recipient, object token, Func<TMessage, Task> action, Dictionary<Type, List<WeakActionAndToken>> lists) {
      Type messageType = typeof(TMessage);

      if (recipient == null || lists == null || lists.Count == 0 || !lists.ContainsKey(messageType)) return;

      lock(lists) {
        foreach(WeakActionAndToken item in lists[messageType]) {
          if (item.Action is WeakAction<TMessage> weakActionCasted && recipient == weakActionCasted.Target
                                       && (action == null || action.Method.Name == weakActionCasted.MethodName)
                                       && (token  == null || token.Equals(item.Token))) {
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

    #endregion Private Methods

  }

}
