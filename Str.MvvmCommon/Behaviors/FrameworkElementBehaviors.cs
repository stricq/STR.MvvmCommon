using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

using Str.MvvmCommon.Contracts;
using Str.MvvmCommon.Core;


namespace Str.MvvmCommon.Behaviors {

  [SuppressMessage("ReSharper", "UnusedType.Global",   Justification = "This is a library.")]
  [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is a library.")]
  public static class FrameworkElementBehaviors {

    #region Initialized Command

    public static readonly DependencyProperty InitializedCommandProperty = DependencyProperty.RegisterAttached("InitializedCommand", typeof(ICommandAsync<EventArgs>), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnInitializedCommandChanged));

    public static ICommandAsync<EventArgs>? GetInitializedCommand(DependencyObject o) {
      return o.GetValue(InitializedCommandProperty) as ICommandAsync<EventArgs>;
    }

    public static void SetInitializedCommand(DependencyObject o, ICommandAsync<EventArgs> value) {
      o.SetValue(InitializedCommandProperty, value);
    }

    private static void OnInitializedCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not FrameworkElement element) return;

      if (e.OldValue == null && e.NewValue != null) element.Initialized += OnInitializedChanged;
      else if (e is { OldValue: not null, NewValue: null }) element.Initialized -= OnInitializedChanged;
    }

    private static void OnInitializedChanged(object? sender, EventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (sender is not FrameworkElement obj) return;

      if (obj.GetValue(InitializedCommandProperty) is ICommandAsync<EventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion Initialized Command

    #region Loaded Command

    public static readonly DependencyProperty LoadedCommandProperty = DependencyProperty.RegisterAttached("LoadedCommand", typeof(ICommandAsync<RoutedEventArgs>), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnLoadedCommandChanged));

    public static ICommandAsync<RoutedEventArgs>? GetLoadedCommand(DependencyObject o) {
      return o.GetValue(LoadedCommandProperty) as ICommandAsync<RoutedEventArgs>;
    }

    public static void SetLoadedCommand(DependencyObject o, ICommandAsync<RoutedEventArgs> value) {
      o.SetValue(LoadedCommandProperty, value);
    }

    private static void OnLoadedCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not FrameworkElement element) return;

      if (e.OldValue == null && e.NewValue != null) element.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoadedChanged), false);
      else if (e is { OldValue: not null, NewValue: null }) element.RemoveHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoadedChanged));
    }

    private static void OnLoadedChanged(object sender, RoutedEventArgs e) {
      if (!ReferenceEquals(sender, e.OriginalSource)) return;

      if (ObservableObject.IsDesignMode) return;

      if (sender is not FrameworkElement obj) return;

      if (obj.GetValue(LoadedCommandProperty) is ICommandAsync<RoutedEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion Loaded Command

    #region SizeChanged Command

    public static readonly DependencyProperty SizeChangedCommandProperty = DependencyProperty.RegisterAttached("SizeChangedCommand", typeof(ICommandAsync<SizeChangedEventArgs>), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnSizeChangedCommandChanged));

    public static ICommandAsync<SizeChangedEventArgs>? GetSizeChangedCommand(DependencyObject o) {
      return o.GetValue(SizeChangedCommandProperty) as ICommandAsync<SizeChangedEventArgs>;
    }

    public static void SetSizeChangedCommand(DependencyObject o, ICommandAsync<SizeChangedEventArgs> value) {
      o.SetValue(SizeChangedCommandProperty, value);
    }

    private static void OnSizeChangedCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not FrameworkElement element) return;

      if (e.OldValue == null && e.NewValue != null) element.AddHandler(FrameworkElement.SizeChangedEvent, new SizeChangedEventHandler(OnSizeChanged), false);
      else if (e is { OldValue: not null, NewValue: null }) element.RemoveHandler(FrameworkElement.SizeChangedEvent, new SizeChangedEventHandler(OnSizeChanged));
    }

    private static void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      if (!ReferenceEquals(sender, e.OriginalSource)) return;

      if (ObservableObject.IsDesignMode) return;

      if (sender is not FrameworkElement obj) return;

      if (obj.GetValue(SizeChangedCommandProperty) is ICommandAsync<SizeChangedEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion SizeChanged Command

    #region ContextMenuOpening Command

    public static readonly DependencyProperty ContextMenuOpeningCommandProperty = DependencyProperty.RegisterAttached("ContextMenuOpeningCommand", typeof(ICommandAsync<ContextMenuEventArgs>), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnContextMenuOpeningCommandChanged));

    public static ICommandAsync<ContextMenuEventArgs>? GetContextMenuOpeningCommand(DependencyObject o) {
      return o.GetValue(ContextMenuOpeningCommandProperty) as ICommandAsync<ContextMenuEventArgs>;
    }

    public static void SetContextMenuOpeningCommand(DependencyObject o, ICommandAsync<ContextMenuEventArgs> value) {
      o.SetValue(ContextMenuOpeningCommandProperty, value);
    }

    private static void OnContextMenuOpeningCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not FrameworkElement element) return;

      if (e.OldValue == null && e.NewValue != null) element.AddHandler(FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(OnContextMenuOpening), false);
      else if (e is { OldValue: not null, NewValue: null }) element.RemoveHandler(FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(OnContextMenuOpening));
    }

    private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (sender is not FrameworkElement obj) return;

      if (obj.GetValue(ContextMenuOpeningCommandProperty) is ICommandAsync<ContextMenuEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion ContextMenuOpening Command

    #region ContextMenuClosing Command

    public static readonly DependencyProperty ContextMenuClosingCommandProperty = DependencyProperty.RegisterAttached("ContextMenuClosingCommand", typeof(ICommandAsync<ContextMenuEventArgs>), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnContextMenuClosingCommandChanged));

    public static ICommandAsync<ContextMenuEventArgs>? GetContextMenuClosingCommand(DependencyObject o) {
      return o.GetValue(ContextMenuClosingCommandProperty) as ICommandAsync<ContextMenuEventArgs>;
    }

    public static void SetContextMenuClosingCommand(DependencyObject o, ICommandAsync<ContextMenuEventArgs> value) {
      o.SetValue(ContextMenuClosingCommandProperty, value);
    }

    private static void OnContextMenuClosingCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not FrameworkElement element) return;

      if (e.OldValue == null && e.NewValue != null) element.AddHandler(FrameworkElement.ContextMenuClosingEvent, new ContextMenuEventHandler(OnContextMenuClosing), false);
      else if (e is { OldValue: not null, NewValue: null }) element.RemoveHandler(FrameworkElement.ContextMenuClosingEvent, new ContextMenuEventHandler(OnContextMenuClosing));
    }

    private static void OnContextMenuClosing(object sender, ContextMenuEventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (sender is not FrameworkElement obj) return;

      if (obj.GetValue(ContextMenuClosingCommandProperty) is ICommandAsync<ContextMenuEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion ContextMenuClosing Command

    #region DataTemplateInjector Command

    public static readonly DependencyProperty DataTemplateInjectorProperty = DependencyProperty.RegisterAttached("DataTemplateInjector", typeof(IEnumerable<IViewLocator>), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnDataTemplateInjectorChanged));

    public static IEnumerable<IViewLocator>? GetDataTemplateInjector(DependencyObject o) {
      return o.GetValue(DataTemplateInjectorProperty) as IEnumerable<IViewLocator>;
    }

    public static void SetDataTemplateInjector(DependencyObject o, IEnumerable<IViewLocator> value) {
      o.SetValue(DataTemplateInjectorProperty, value);
    }

    private static void OnDataTemplateInjectorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (o is not FrameworkElement || e.NewValue is not IEnumerable<IViewLocator> views) return;

      foreach(IViewLocator view in views) {
        DataTemplate? dt = CreateTemplate(view.DataContext.GetType(), view.GetType());

        if (dt?.DataTemplateKey == null) continue;

        if (!Application.Current.Resources.Contains(dt.DataTemplateKey)) Application.Current.Resources.Add(dt.DataTemplateKey, dt);
      }
    }

    private static DataTemplate? CreateTemplate(Type viewModelType, Type viewType) {
      const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";

      string xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name);

      ParserContext context = new() { XamlTypeMapper = new XamlTypeMapper(Array.Empty<string>()) };

      context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace ?? String.Empty, viewModelType.Assembly.FullName ?? String.Empty);
      context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace ?? String.Empty, viewType.Assembly.FullName ?? String.Empty);

      context.XmlnsDictionary.Add("",   "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
      context.XmlnsDictionary.Add("x",  "http://schemas.microsoft.com/winfx/2006/xaml");
      context.XmlnsDictionary.Add("vm", "vm");
      context.XmlnsDictionary.Add("v",  "v");

      return XamlReader.Parse(xaml, context) as DataTemplate;
    }

    #endregion DataTemplateInjector Command

  }

}
