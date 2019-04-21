using System;
using System.Diagnostics.CodeAnalysis;


namespace Str.MvvmCommon.Core {

  [AttributeUsage(AttributeTargets.Class)]
  [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
  [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public class ViewModelAttribute : Attribute {

    #region Constructor

    public ViewModelAttribute(string name) {
      Name = name;
    }

    #endregion Constructor

    #region Public Properties

    public string Name { get; }

    public bool IsDesignMode { get; set; }

    #endregion Public Properties

  }

}
