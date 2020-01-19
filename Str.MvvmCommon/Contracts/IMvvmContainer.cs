using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Str.MvvmCommon.Contracts {

  [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is a library.")]
  [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "This is a library.")]
  public interface IMvvmContainer {

    void Initialize(Action<IServiceCollection, IConfiguration> configure);

    void InitializeControllers();

    void OnStartup();

    void OnExit();

    object Get(Type type);

    T Get<T>();

    IEnumerable<object> GetAll(Type type);

    IEnumerable<T> GetAll<T>();

  }

}
