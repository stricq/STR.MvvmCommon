﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Threading.Tasks;

using Str.Common.Extensions;

using Str.MvvmCommon.Contracts;
using Str.MvvmCommon.Helpers;


namespace Str.MvvmCommon.Core {

  [Export(typeof(IMvvmContainer))]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class MvvmContainer : IMvvmContainer {

    #region Private Fields

    private static ComposablePartCatalog catalog;
    private static CompositionContainer  container;

    #endregion Private Fields

    #region IMvvmContainer Implementation

    public void Initialize<T>(Func<T> Registrar) {
      catalog = Registrar() as ComposablePartCatalog;

      if (catalog == null) throw new Exception("Return from Registrar() must be ComposablePartCatalog.");

      container = new CompositionContainer(catalog);

      MvvmLocator.Container = this;
    }

    public void InitializeControllers() {
      IEnumerable<IController> controllers = GetAll<IController>();

      IOrderedEnumerable<IGrouping<int, IController>> groups = controllers.GroupBy(c => c.InitializePriority).OrderBy(g => g.Key);

      foreach(IGrouping<int, IController> group in groups) {
        Task.Run(() => group.ForEachAsync(controller => controller.InitializeAsync())).Wait();
      }
    }

    public void RegisterInstance<T>(T instance) {
      container.ComposeExportedValue(instance);
    }

    public object Get(Type Type) {
      Lazy<object, object> lazy = container.GetExports(Type, null, null).FirstOrDefault();

      return lazy?.Value;
    }

    public T Get<T>() {
      return container.GetExportedValue<T>();
    }

    public T GetNew<T>() where T : class {
      IEnumerable<Export> parts = container.GetExports(new ContractBasedImportDefinition(AttributedModelServices.GetContractName(typeof(T)), AttributedModelServices.GetTypeIdentity(typeof(T)), null, ImportCardinality.ZeroOrMore, false, false, CreationPolicy.NonShared));

      Export part = parts.SingleOrDefault();

      return part?.Value as T;
    }

    public IEnumerable<T> GetAll<T>() {
      return container.GetExportedValues<T>();
    }

    public IEnumerable<Type> GetTypes() {
      return catalog.Parts.Select(part => ReflectionModelServices.GetPartType(part).Value).ToList();
    }

    #endregion IMvvmContainer Implementation

  }

}
