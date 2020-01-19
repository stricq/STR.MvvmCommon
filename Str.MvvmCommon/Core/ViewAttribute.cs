using System;
using System.Diagnostics.CodeAnalysis;


namespace Str.MvvmCommon.Core {

  [AttributeUsage(AttributeTargets.Class)]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public class ViewAttribute : Attribute {

    public string Name { get; set; }

    public string Data { get; set; }

    public string Tag { get; set; }

  }

}
