using System;
using System.Reflection;
using System.Threading.Tasks;

using Str.MvvmCommon.Contracts;


namespace Str.MvvmCommon.Helpers {

  internal class WeakFunc<TMessage> : IWeakFunc {

    #region Private Fields

    private Func<TMessage, Task>? staticFunc;

    private MethodInfo? method;

    private WeakReference? actionReference;

    private WeakReference reference;

    #endregion Private Fields

    #region Constructor

    public WeakFunc(IMessageReceiver target, Func<TMessage, Task> action) {
      //
      // Keep a reference to the target to control the WeakAction's lifetime.
      //
      reference = new WeakReference(target);

      if (action.Method.IsStatic) {
        staticFunc = action;

        return;
      }

      method          = action.Method;
      actionReference = new WeakReference(action.Target);
    }

    #endregion Constructor

    #region IWeakFunc Implementation

    public string? MethodName => staticFunc != null ? staticFunc.Method.Name : method?.Name;

    public bool IsAlive => reference.IsAlive;

    public object? Target => reference.Target;

    public void MarkForDeletion() {
      staticFunc = null;

      reference       = new WeakReference(null);
      actionReference = null;
      method          = null;
    }

    #endregion IWeakFunc Implementation

    #region Public Methods

    public Task ExecuteWithObjectAsync(TMessage parameter) {
      if (staticFunc != null) {
        return staticFunc(parameter);
      }

      if (!IsAlive || method == null || actionReference == null) return Task.CompletedTask;

      if (Delegate.CreateDelegate(typeof(Func<TMessage, Task>), ActionTarget, method) is Func<TMessage, Task> func) return func(parameter);

      return Task.CompletedTask;
    }

    #endregion Public Methods

    #region Private Properties

    private object? ActionTarget => actionReference?.Target;

    #endregion Private Properties

  }

}
