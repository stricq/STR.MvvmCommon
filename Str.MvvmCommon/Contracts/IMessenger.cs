using System;
using System.Threading.Tasks;


namespace Str.MvvmCommon.Contracts {
  //
  // This code is based on code found in MvvmLight.
  //
  // https://github.com/lbugnion/mvvmlight/tree/master/GalaSoft.MvvmLight/GalaSoft.MvvmLight%20(PCL)/Messaging
  //
  public interface IMessenger {

    void Register<TMessage>(object recipient, Action<TMessage> action);

    void Register<TMessage>(object recipient, Func<TMessage, Task> action);

    void Register<TMessage>(object recipient, object token, Action<TMessage> action);

    void Register<TMessage>(object recipient, object token, Func<TMessage, Task> action);

    void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action);

    void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Func<TMessage, Task> action);

    void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action);

    void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Func<TMessage, Task> action);

    void Send<TMessage>(TMessage Message);

    void Send<TMessage>(TMessage Message, object token);

    Task SendAsync<TMessage>(TMessage Message);

    Task SendAsync<TMessage>(TMessage Message, object token);

    Task SendOnUiThreadAsync<TMessage>(TMessage Message);

    Task SendOnUiThreadAsync<TMessage>(TMessage Message, object token);

    void Unregister(object recipient);

    void Unregister<TMessage>(object recipient);

    void Unregister<TMessage>(object recipient, object token);

    void Unregister<TMessage>(object recipient, Action<TMessage> action);

    void Unregister<TMessage>(object recipient, Func<TMessage, Task> action);

    void Unregister<TMessage>(object recipient, object token, Action<TMessage> action);

    void Unregister<TMessage>(object recipient, object token, Func<TMessage, Task> action);

  }

}
