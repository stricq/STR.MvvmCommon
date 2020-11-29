using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

    private readonly ArgumentNullException hostNullException = new ArgumentNullException(nameof(host), "The container has not been initialized.  Please call the Initialize() method first.");

    private IHost? host;

    #endregion Private Fields

    #region IMvvmContainer Implementation

    public void Initialize(Action<IServiceCollection, IConfiguration> configure) {
      host = Host.CreateDefaultBuilder().ConfigureServices((context, services) => {
        services.AddSingleton<IMessenger, Messenger>();

        List<Type>? classes = Assembly.GetEntryAssembly()?.GetTypes().Where(type => type.IsClass).ToList();

        if (classes != null) {
          classes.Where(type => type.Name.EndsWith("View") || type.Name.EndsWith("ViewModel"))
                 .ForEach(type => services.AddSingleton(type));

          classes.Where(type => typeof(IController).IsAssignableFrom(type) && type.Name.EndsWith("Controller"))
                 .ForEach(type => services.AddSingleton(typeof(IController), type));
        }

        configure.Invoke(services, context.Configuration);
      }).Build();

      MvvmLocator.Container = this;
    }

    public async Task InitializeControllersAsync(bool descending = false) {
      IEnumerable<IController> controllers = GetAll<IController>();

      IEnumerable<IGrouping<int, IController>> groups = controllers.GroupBy(c => c.InitializePriority);

      groups = descending ? groups.OrderByDescending(g => g.Key) : groups.OrderBy(g => g.Key);

      foreach(IGrouping<int, IController> group in groups) {
        await group.ForEachAsync(controller => controller.InitializeAsync()).Fire();
      }
    }

    public async Task OnStartupAsync() {
      if (host == null) throw hostNullException;

      await host.StartAsync().Fire();

      TaskHelper.InitializeOnUiThread();
    }

    public async Task OnExitAsync() {
      if (host == null) throw hostNullException;

      using(host) {
        await host.StopAsync(TimeSpan.FromSeconds(5)).Fire();
      }
    }

    public object? Get(Type type) {
      if (host == null) throw hostNullException;

      return host.Services.GetService(type);
    }

    public T Get<T>() where T : notnull {
      if (host == null) throw hostNullException;

      return host.Services.GetRequiredService<T>();
    }

    public IEnumerable<object?> GetAll(Type type) {
      if (host == null) throw hostNullException;

      return host.Services.GetServices(type);
    }

    public IEnumerable<T> GetAll<T>() {
      if (host == null) throw hostNullException;

      return host.Services.GetServices<T>();
    }

    #endregion IMvvmContainer Implementation

  }

}
