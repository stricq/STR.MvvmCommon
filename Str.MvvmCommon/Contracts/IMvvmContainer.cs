using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Str.MvvmCommon.Contracts {

  public interface IMvvmContainer {

    void Initialize(Action<IServiceCollection, IConfiguration> configure);

    void InitializeControllers();

    void OnStartup();

    void OnExit();

    object Get(Type type);

    T Get<T>();

    IEnumerable<T> GetAll<T>();

  }

}
