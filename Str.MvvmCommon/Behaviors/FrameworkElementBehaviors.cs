﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

using Str.MvvmCommon.Contracts;
using Str.MvvmCommon.Core;


namespace Str.MvvmCommon.Behaviors {

  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public static class FrameworkElementBehaviors {

    #region Initialized Command

    public static readonly DependencyProperty InitializedCommandProperty = DependencyProperty.RegisterAttached("InitializedCommand", typeof(ICommand), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnInitializedCommandChanged));

    public static ICommand GetInitializedCommand(DependencyObject o) {
      return o.GetValue(InitializedCommandProperty) as ICommand;
    }

    public static void SetInitializedCommand(DependencyObject o, ICommand value) {
      o.SetValue(InitializedCommandProperty, value);
    }

    private static void OnInitializedCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (!(o is FrameworkElement element)) return;

      if (e.OldValue == null && e.NewValue != null) {
        element.Initialized += OnInitializedChanged;
      }
      else if (e.OldValue != null && e.NewValue == null) {
        element.Initialized -= OnInitializedChanged;
      }
    }

    private static void OnInitializedChanged(object sender, EventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (!(sender is FrameworkElement obj)) return;

      if (obj.GetValue(InitializedCommandProperty) is ICommand command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion Initialized Command

    #region Loaded Command

    public static readonly DependencyProperty LoadedCommandProperty = DependencyProperty.RegisterAttached("LoadedCommand", typeof(ICommand), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnLoadedCommandChanged));

    public static ICommand GetLoadedCommand(DependencyObject o) {
      return o.GetValue(LoadedCommandProperty) as ICommand;
    }

    public static void SetLoadedCommand(DependencyObject o, ICommand value) {
      o.SetValue(LoadedCommandProperty, value);
    }

    private static void OnLoadedCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (!(o is FrameworkElement element)) return;

      if (e.OldValue == null && e.NewValue != null) {
        element.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoadedChanged), false);
      }
      else if (e.OldValue != null && e.NewValue == null) {
        element.RemoveHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoadedChanged));
      }
    }

    private static void OnLoadedChanged(object sender, RoutedEventArgs e) {
      if (!ReferenceEquals(sender, e.OriginalSource)) return;

      if (ObservableObject.IsDesignMode) return;

      if (!(sender is FrameworkElement obj)) return;

      if (obj.GetValue(LoadedCommandProperty) is ICommand command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion Loaded Command

    #region SizeChanged Command

    public static readonly DependencyProperty SizeChangedCommandProperty = DependencyProperty.RegisterAttached("SizeChangedCommand", typeof(ICommand), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnSizeChangedCommandChanged));

    public static ICommand GetSizeChangedCommand(DependencyObject o) {
      return o.GetValue(SizeChangedCommandProperty) as ICommand;
    }

    public static void SetSizeChangedCommand(DependencyObject o, ICommand value) {
      o.SetValue(SizeChangedCommandProperty, value);
    }

    private static void OnSizeChangedCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (!(o is FrameworkElement element)) return;

      if (e.OldValue == null && e.NewValue != null) {
        element.AddHandler(FrameworkElement.SizeChangedEvent, new SizeChangedEventHandler(OnSizeChanged), false);
      }
      else if (e.OldValue != null && e.NewValue == null) {
        element.RemoveHandler(FrameworkElement.SizeChangedEvent, new SizeChangedEventHandler(OnSizeChanged));
      }
    }

    private static void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      if (!ReferenceEquals(sender, e.OriginalSource)) return;

      if (ObservableObject.IsDesignMode) return;

      if (!(sender is FrameworkElement obj)) return;

      if (obj.GetValue(SizeChangedCommandProperty) is ICommand command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion SizeChanged Command

    #region ContextMenuOpening Command

    public static readonly DependencyProperty ContextMenuOpeningCommandProperty = DependencyProperty.RegisterAttached("ContextMenuOpeningCommand", typeof(ICommand), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnContextMenuOpeningCommandChanged));

    public static ICommand GetContextMenuOpeningCommand(DependencyObject o) {
      return o.GetValue(ContextMenuOpeningCommandProperty) as ICommand;
    }

    public static void SetContextMenuOpeningCommand(DependencyObject o, ICommand value) {
      o.SetValue(ContextMenuOpeningCommandProperty, value);
    }

    private static void OnContextMenuOpeningCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (!(o is FrameworkElement element)) return;

      if (e.OldValue == null && e.NewValue != null) {
        element.AddHandler(FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(OnContextMenuOpening), false);
      }
      else if (e.OldValue != null && e.NewValue == null) {
        element.RemoveHandler(FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(OnContextMenuOpening));
      }
    }

    private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (!(sender is FrameworkElement obj)) return;

      if (obj.GetValue(ContextMenuOpeningCommandProperty) is ICommand command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion ContextMenuOpening Command

    #region ContextMenuClosing Command

    public static readonly DependencyProperty ContextMenuClosingCommandProperty = DependencyProperty.RegisterAttached("ContextMenuClosingCommand", typeof(ICommand), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnContextMenuClosingCommandChanged));

    public static ICommand GetContextMenuClosingCommand(DependencyObject o) {
      return o.GetValue(ContextMenuClosingCommandProperty) as ICommand;
    }

    public static void SetContextMenuClosingCommand(DependencyObject o, ICommand value) {
      o.SetValue(ContextMenuClosingCommandProperty, value);
    }

    private static void OnContextMenuClosingCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (!(o is FrameworkElement element)) return;

      if (e.OldValue == null && e.NewValue != null) {
        element.AddHandler(FrameworkElement.ContextMenuClosingEvent, new ContextMenuEventHandler(OnContextMenuClosing), false);
      }
      else if (e.OldValue != null && e.NewValue == null) {
        element.RemoveHandler(FrameworkElement.ContextMenuClosingEvent, new ContextMenuEventHandler(OnContextMenuClosing));
      }
    }

    private static void OnContextMenuClosing(object sender, ContextMenuEventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (!(sender is FrameworkElement obj)) return;

      if (obj.GetValue(ContextMenuClosingCommandProperty) is ICommand command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion ContextMenuClosing Command

    #region DataTemplateInjector Command

    public static readonly DependencyProperty DataTemplateInjectorProperty = DependencyProperty.RegisterAttached("DataTemplateInjector", typeof(IEnumerable<IViewLocator>), typeof(FrameworkElementBehaviors), new FrameworkPropertyMetadata(null, OnDataTemplateInjectorChanged));

    public static IEnumerable<IViewLocator> GetDataTemplateInjector(DependencyObject o) {
      return o.GetValue(DataTemplateInjectorProperty) as IEnumerable<IViewLocator>;
    }

    public static void SetDataTemplateInjector(DependencyObject o, IEnumerable<IViewLocator> value) {
      o.SetValue(DataTemplateInjectorProperty, value);
    }

    private static void OnDataTemplateInjectorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (!(o is FrameworkElement) || !(e.NewValue is IEnumerable<IViewLocator> views)) return;

      foreach(IViewLocator view in views) {
        if (view?.DataContext == null) continue;

        DataTemplate dt = CreateTemplate(view.DataContext.GetType(), view.GetType());

        if (dt?.DataTemplateKey == null) continue;

        if (!Application.Current.Resources.Contains(dt.DataTemplateKey)) {
          Application.Current.Resources.Add(dt.DataTemplateKey, dt);
        }
      }
    }

    private static DataTemplate CreateTemplate(Type viewModelType, Type viewType) {
      const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";

      string xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name);

      ParserContext context = new ParserContext { XamlTypeMapper = new XamlTypeMapper(new string[0]) };

      context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
      context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

      context.XmlnsDictionary.Add("",   "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
      context.XmlnsDictionary.Add("x",  "http://schemas.microsoft.com/winfx/2006/xaml");
      context.XmlnsDictionary.Add("vm", "vm");
      context.XmlnsDictionary.Add("v",  "v");

      return XamlReader.Parse(xaml, context) as DataTemplate;
    }

    #endregion DataTemplateInjector Command

  }

}
