using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;


namespace Str.MvvmCommon.Helpers {
  //
  // This code is based on code found in MvvmLight.
  //
  // https://github.com/lbugnion/mvvmlight/tree/master/GalaSoft.MvvmLight/GalaSoft.MvvmLight%20(PCL)/Messaging
  //
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
  internal class WeakAction {

    #region Private Fields

    private Action staticAction;

    #endregion Private Fields

    #region Properties

    protected MethodInfo Method { get; set; }

    protected WeakReference ActionReference { get; set; }

    protected WeakReference Reference { get; set; }

    public virtual string MethodName => staticAction != null ? staticAction.Method.Name : Method.Name;

    public bool IsStatic => staticAction != null;

    public virtual bool IsAlive {
      get {
        if (staticAction == null && Reference == null) return false;

        if (staticAction != null) return Reference == null || Reference.IsAlive;

        return Reference.IsAlive;
      }
    }

    public object Target => Reference?.Target;

    protected object ActionTarget => ActionReference?.Target;

    #endregion Properties

    #region Public Methods

    public void Execute() {
      if (staticAction != null) {
        staticAction();

        return;
      }

      object actionTarget = ActionTarget;

      if (!IsAlive) return;

      if (Method == null || ActionReference == null || actionTarget == null) return;

      if (Delegate.CreateDelegate(typeof(Action), Method) is Action action) action();
    }

    public void MarkForDeletion() {
      Reference       = null;
      ActionReference = null;
      Method          = null;

      staticAction = null;
    }

    #endregion Public Methods

  }


  internal class WeakAction<T> : WeakAction, IExecuteWithObject {

    #region Private Fields

    private Action<T> staticAction;

    #endregion Private Fields

    #region Constructors

    public WeakAction(object target, Action<T> Action) {
      if (Action.Method.IsStatic) {
        staticAction = Action;

        if (target != null) {
          //
          // Keep a reference to the target to control the WeakAction's lifetime.
          //
          Reference = new WeakReference(target);
        }

        return;
      }

      Method          = Action.Method;
      ActionReference = new WeakReference(Action.Target);
      Reference       = new WeakReference(target);
    }

    #endregion Constructors

    #region Properties

    public override string MethodName => staticAction != null ? staticAction.Method.Name : Method.Name;

    public override bool IsAlive {
      get {
        if (staticAction == null && Reference == null) return false;

        if (staticAction != null) return Reference == null || Reference.IsAlive;

        return Reference.IsAlive;
      }
    }

    #endregion Properties

    #region Public Methods

    public void ExecuteWithObject(object parameter) {
      T parameterCasted = (T)parameter;

      execute(parameterCasted);
    }

    public new void MarkForDeletion() {
      staticAction = null;

      base.MarkForDeletion();
    }

    #endregion Public Methods

    #region Private Methods

    private void execute(T parameter = default) {
      if (staticAction != null) {
        staticAction(parameter);

        return;
      }

      if (!IsAlive) return;

      if (Method == null || ActionReference == null) return;

      if (Delegate.CreateDelegate(typeof(Action<T>), ActionTarget, Method) is Action<T> action) action(parameter);
    }

    #endregion Private Methods

  }


  internal class WeakFunc<T> : WeakAction, IExecuteWithObjectAsync {

    #region Private Fields

    private Func<T, Task> staticFunc;

    #endregion Private Fields

    #region Constructors

    public WeakFunc(object target, Func<T, Task> Action) {
      if (Action.Method.IsStatic) {
        staticFunc = Action;

        if (target != null) {
          //
          // Keep a reference to the target to control the WeakAction's lifetime.
          //
          Reference = new WeakReference(target);
        }

        return;
      }

      Method          = Action.Method;
      ActionReference = new WeakReference(Action.Target);
      Reference       = new WeakReference(target);
    }

    #endregion Constructors

    #region Properties

    public override string MethodName => staticFunc != null ? staticFunc.Method.Name : Method.Name;

    public override bool IsAlive {
      get {
        if (staticFunc == null && Reference == null) return false;

        if (staticFunc != null) return Reference == null || Reference.IsAlive;

        return Reference.IsAlive;
      }
    }

    #endregion Properties

    #region Public Methods

    public async Task ExecuteWithObjectAsync(object parameter) {
      T parameterCasted = (T)parameter;

      await execute(parameterCasted);
    }

    public new void MarkForDeletion() {
      staticFunc = null;

      base.MarkForDeletion();
    }

    #endregion Public Methods

    #region Private Methods

    private async Task execute(T Parameter = default) {
      if (staticFunc != null) {
        await staticFunc(Parameter);

        return;
      }

      if (IsAlive) {
        if (Method != null && ActionReference != null) {
          if (Delegate.CreateDelegate(typeof(Func<T, Task>), ActionTarget, Method) is Func<T, Task> func) await func(Parameter);
        }
      }
    }

    #endregion Private Methods

  }

}
