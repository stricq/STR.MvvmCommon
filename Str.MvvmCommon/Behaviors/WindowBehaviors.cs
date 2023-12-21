using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

using Str.MvvmCommon.Contracts;
using Str.MvvmCommon.Core;


namespace Str.MvvmCommon.Behaviors {

  [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is a library.")]
  [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "This is a library.")]
  [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
  public static class WindowBehaviors {

    #region Closing Command

    public static readonly DependencyProperty ClosingCommandProperty = DependencyProperty.RegisterAttached("ClosingCommand", typeof(ICommandAsync<CancelEventArgs>), typeof(WindowBehaviors), new PropertyMetadata(null, OnClosingCommandChanged));

    public static ICommandAsync<CancelEventArgs>? GetClosingCommand(DependencyObject o) {
      return o.GetValue(ClosingCommandProperty) as ICommandAsync<CancelEventArgs>;
    }

    public static void SetClosingCommand(DependencyObject o, ICommandAsync<CancelEventArgs> value) {
      o.SetValue(ClosingCommandProperty, value);
    }

    private static void OnClosingCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not Window element) return;

      if (e.OldValue == null && e.NewValue != null) element.Closing += OnClosingChanged;
      else if (e is { OldValue: not null, NewValue: null }) element.Closing -= OnClosingChanged;
    }

    private static void OnClosingChanged(object? sender, CancelEventArgs e) {
      if (ObservableObject.IsDesignMode) return;

      if (sender is not Window obj) return;

      if (obj.GetValue(ClosingCommandProperty) is ICommandAsync<CancelEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion Closing Command

    #region Show Property

    public static readonly DependencyProperty ShowProperty = DependencyProperty.RegisterAttached("Show", typeof(Action), typeof(WindowBehaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnShowPropertyChanged));

    public static Action? GetShow(DependencyObject o) {
      return o.GetValue(ShowProperty) as Action;
    }

    public static void SetShow(DependencyObject o, Action value) {
      o.SetValue(ShowProperty, value);
    }

    private static void OnShowPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not Window element || e.NewValue as Action == element.Show) return;

      SetShow(o, element.Show);
    }

    #endregion Show Property

    #region Hide Property

    public static readonly DependencyProperty HideProperty = DependencyProperty.RegisterAttached("Hide", typeof(Action), typeof(WindowBehaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHidePropertyChanged));

    public static Action? GetHide(DependencyObject o) {
      return o.GetValue(HideProperty) as Action;
    }

    public static void SetHide(DependencyObject o, Action value) {
      o.SetValue(HideProperty, value);
    }

    private static void OnHidePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not Window element || e.NewValue as Action == element.Hide) return;

      SetHide(o, element.Hide);
    }

    #endregion Hide Property

    #region Activate Property

    public static readonly DependencyProperty ActivateProperty = DependencyProperty.RegisterAttached("Activate", typeof(Func<bool>), typeof(WindowBehaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnActivatePropertyChanged));

    public static Func<bool>? GetActivate(DependencyObject o) {
      return o.GetValue(ActivateProperty) as Func<bool>;
    }

    public static void SetActivate(DependencyObject o, Func<bool> value) {
      o.SetValue(ActivateProperty, value);
    }

    private static void OnActivatePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (o is not Window element || e.NewValue as Func<bool> == element.Activate) return;

      SetActivate(o, element.Activate);
    }

    #endregion Show Property

  }

}
