using System;
using System.Collections.Generic;


namespace Str.MvvmCommon.Contracts {

  public interface IMvvmContainer {

    void Initialize<T>(Func<T> registrar);

    void InitializeControllers();

    void RegisterInstance<T>(T instance);

    object Get(Type type);

    T Get<T>();

    T GetNew<T>() where T : class;

    IEnumerable<T> GetAll<T>();

    IEnumerable<Type> GetTypes();

  }

}
