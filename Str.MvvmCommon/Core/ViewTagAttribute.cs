using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;


namespace Str.MvvmCommon.Core {

  [MetadataAttribute]
  [AttributeUsage(AttributeTargets.Class)]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public class ViewTagAttribute : ExportAttribute {

    public string Name { get; set; }

    public string Data { get; set; }

    public string Tag  { get; set; }

  }

}
