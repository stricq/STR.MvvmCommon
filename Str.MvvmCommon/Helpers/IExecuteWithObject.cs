using System.Threading.Tasks;


namespace Str.MvvmCommon.Helpers {
  //
  // This code is based on code found in MvvmLight.
  //
  // https://github.com/lbugnion/mvvmlight/tree/master/GalaSoft.MvvmLight/GalaSoft.MvvmLight%20(PCL)/Messaging
  //
  internal interface IExecuteWithObject {

    object Target { get; }

    void ExecuteWithObject(object parameter);

    void MarkForDeletion();

  }


  internal interface IExecuteWithObjectAsync {

    object Target { get; }

    Task ExecuteWithObjectAsync(object parameter);

    void MarkForDeletion();

  }


}
