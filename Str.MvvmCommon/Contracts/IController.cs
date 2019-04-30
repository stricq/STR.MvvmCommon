﻿using System.Threading.Tasks;


namespace Str.MvvmCommon.Contracts {

  public interface IController {

    int InitializePriority { get; }

    Task InitializeAsync();

  }

}
