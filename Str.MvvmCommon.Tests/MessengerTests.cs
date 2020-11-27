using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Str.Common.Extensions;
using Str.Common.Messages;

using Str.MvvmCommon.Contracts;
using Str.MvvmCommon.Services;


namespace Str.MvvmCommon.Tests {

  [TestClass]
  public class MessengerTests : IMessageReceiver {

    private static int counter;

    [TestMethod, TestCategory("Unit")]
    public async Task RegisterRecipient() {
      Messenger messenger = new Messenger();

      messenger.Register<MessageA>(this, null, false, Receiver);
      messenger.Register<MessageA>(this, null, false, Receiver);

      static Task Receiver(MessageA message) {
        Interlocked.Increment(ref counter);

        return Task.CompletedTask;
      }

      await messenger.SendAsync(new MessageA()).Fire();

      await messenger.SendAsync(new MessageB()).Fire();

      messenger.Unregister(this);

      await messenger.SendAsync(new MessageA()).Fire();

      Assert.AreEqual(2, counter);
    }

  }

  public class MessageA : MessageBase { }

  public class MessageB : MessageBase { }

}
