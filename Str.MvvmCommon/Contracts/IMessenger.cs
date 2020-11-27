using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;


namespace Str.MvvmCommon.Contracts {
  //
  // This code is based on code found in MvvmLight.
  //
  // https://github.com/lbugnion/mvvmlight/tree/master/GalaSoft.MvvmLight/GalaSoft.MvvmLight%20(PCL)/Messaging
  //
  [SuppressMessage("ReSharper", "UnusedMember.Global",        Justification = "This is a library.")]
  [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "This is a library.")]
  public interface IMessenger {

    #region Register

    void Register<TMessage>(IMessageReceiver recipient, Func<TMessage, Task> action);

    void Register<TMessage>(IMessageReceiver recipient, object? token, Func<TMessage, Task> action);

    void Register<TMessage>(IMessageReceiver recipient, bool receiveDerivedMessagesToo, Func<TMessage, Task> action);

    void Register<TMessage>(IMessageReceiver recipient, object? token, bool receiveDerivedMessagesToo, Func<TMessage, Task> action);

    #endregion Register

    #region SendAsync

    Task SendAsync<TMessage>(TMessage message);

    Task SendAsync<TMessage>(TMessage message, object? token);

    #endregion SendAsync

    #region SendOnUiThreadAsync

    Task SendOnUiThreadAsync<TMessage>(TMessage message);

    Task SendOnUiThreadAsync<TMessage>(TMessage message, object token);

    #endregion SendOnUiThreadAsync

    #region Unregister

    void Unregister(IMessageReceiver recipient);

    void Unregister<TMessage>(IMessageReceiver recipient);

    void Unregister<TMessage>(IMessageReceiver recipient, Func<TMessage, Task> action);

    void Unregister<TMessage>(IMessageReceiver recipient, object token, Func<TMessage, Task> action);

    #endregion Unregister

  }

}
