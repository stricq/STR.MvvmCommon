using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Markup;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Str.Common.Extensions;

using Str.MvvmCommon.Contracts;
using Str.MvvmCommon.Helpers;
using Str.MvvmCommon.Services;


[assembly: XmlnsDefinition("http://schemas.stricq.com/mvvmcommon", "Str.MvvmCommon.Behaviors")]


namespace Str.MvvmCommon.Core {

  [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
  public class MvvmContainer : IMvvmContainer {

    #region Private Fields

    private IHost host;

    #endregion Private Fields

    #region IMvvmContainer Implementation

    public void Initialize(Action<IServiceCollection, IConfiguration> configure) {
      host = Host.CreateDefaultBuilder().ConfigureServices((context, services) => {
        services.AddSingleton<IMessenger, Messenger>();

        configure(services, context.Configuration);
      }).Build();

      MvvmLocator.Container = this;
    }

    public void InitializeControllers(bool descending = false) {
      IEnumerable<IController> controllers = GetAll<IController>();

      IEnumerable<IGrouping<int, IController>> groups = controllers.GroupBy(c => c.InitializePriority);

      groups = descending ? groups.OrderByDescending(g => g.Key) : groups.OrderBy(g => g.Key);

      foreach(IGrouping<int, IController> group in groups) {
        group.ForEachAsync(controller => controller.InitializeAsync()).FireAndWait();
      }
    }

    public void OnStartup() {
      host.StartAsync().FireAndWait(true);

      TaskHelper.InitializeOnUiThread();
    }

    public void OnExit() {
      using(host) {
        host.StopAsync(TimeSpan.FromSeconds(5)).FireAndWait(true);
      }
    }

    public object Get(Type type) {
      return host.Services.GetService(type);
    }

    public T Get<T>() {
      return host.Services.GetRequiredService<T>();
    }

    public IEnumerable<object> GetAll(Type type) {
      return host.Services.GetServices(type);
    }

    public IEnumerable<T> GetAll<T>() {
      return host.Services.GetServices<T>();
    }

    #endregion IMvvmContainer Implementation

  }

}
