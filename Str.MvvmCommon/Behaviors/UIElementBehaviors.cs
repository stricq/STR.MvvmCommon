using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

using Str.MvvmCommon.Contracts;
using Str.MvvmCommon.Core;


namespace Str.MvvmCommon.Behaviors;


[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is a library.")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "This is a library.")]
public static class UIElementBehaviors {

    #region MouseEnter Command

    public static readonly DependencyProperty MouseEnterCommandProperty = DependencyProperty.RegisterAttached("MouseEnterCommand", typeof(ICommandAsync<MouseEventArgs>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, OnMouseEnterCommandChanged));

    public static ICommandAsync<MouseEventArgs>? GetMouseEnterCommand(DependencyObject o) {
        return o.GetValue(MouseEnterCommandProperty) as ICommandAsync<MouseEventArgs>;
    }

    public static void SetMouseEnterCommand(DependencyObject o, ICommandAsync<MouseEventArgs> value) {
        o.SetValue(MouseEnterCommandProperty, value);
    }

    private static void OnMouseEnterCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element) return;

        if (e.OldValue == null && e.NewValue != null) element.MouseEnter += OnMouseEnterChanged;
        else if (e is { OldValue: not null, NewValue: null }) element.MouseEnter -= OnMouseEnterChanged;
    }

    private static void OnMouseEnterChanged(object sender, MouseEventArgs e) {
        if (!ReferenceEquals(sender, e.OriginalSource)) return;

        if (ObservableObject.IsDesignMode) return;

        if (sender is not UIElement obj) return;

        if (obj.GetValue(MouseEnterCommandProperty) is ICommandAsync<MouseEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion MouseEnter Command

    #region MouseLeave Command

    public static readonly DependencyProperty MouseLeaveCommandProperty = DependencyProperty.RegisterAttached("MouseLeaveCommand", typeof(ICommandAsync<MouseEventArgs>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, OnMouseLeaveCommandChanged));

    public static ICommandAsync<MouseEventArgs>? GetMouseLeaveCommand(DependencyObject o) {
        return o.GetValue(MouseLeaveCommandProperty) as ICommandAsync<MouseEventArgs>;
    }

    public static void SetMouseLeaveCommand(DependencyObject o, ICommandAsync<MouseEventArgs> value) {
        o.SetValue(MouseLeaveCommandProperty, value);
    }

    private static void OnMouseLeaveCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element) return;

        if (e.OldValue == null && e.NewValue != null) element.MouseLeave += OnMouseLeaveChanged;
        else if (e is { OldValue: not null, NewValue: null }) element.MouseLeave -= OnMouseLeaveChanged;
    }

    private static void OnMouseLeaveChanged(object sender, MouseEventArgs e) {
        if (!ReferenceEquals(sender, e.OriginalSource)) return;

        if (ObservableObject.IsDesignMode) return;

        if (sender is not UIElement obj) return;

        if (obj.GetValue(MouseLeaveCommandProperty) is ICommandAsync<MouseEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion MouseLeave Command

    #region PreviewKeyDown Command

    public static readonly DependencyProperty PreviewKeyDownCommandProperty = DependencyProperty.RegisterAttached("PreviewKeyDownCommand", typeof(ICommandAsync<KeyEventArgs>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, OnPreviewKeyDownCommandChanged));

    public static ICommandAsync<KeyEventArgs>? GetPreviewKeyDownCommand(DependencyObject o) {
        return o.GetValue(PreviewKeyDownCommandProperty) as ICommandAsync<KeyEventArgs>;
    }

    public static void SetPreviewKeyDownCommand(DependencyObject o, ICommandAsync<KeyEventArgs> value) {
        o.SetValue(PreviewKeyDownCommandProperty, value);
    }

    private static void OnPreviewKeyDownCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element) return;

        if (e.OldValue == null && e.NewValue != null) element.PreviewKeyDown += OnPreviewKeyDownChanged;
        else if (e is { OldValue: not null, NewValue: null }) element.PreviewKeyDown -= OnPreviewKeyDownChanged;
    }

    private static void OnPreviewKeyDownChanged(object sender, KeyEventArgs e) {
        if (ObservableObject.IsDesignMode) return;

        if (sender is not UIElement obj) return;

        if (obj.GetValue(PreviewKeyDownCommandProperty) is ICommandAsync<KeyEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion PreviewKeyDown Command

    #region PreviewMouseMove Command

    public static readonly DependencyProperty PreviewMouseMoveCommandProperty = DependencyProperty.RegisterAttached("PreviewMouseMoveCommand", typeof(ICommandAsync<MouseEventArgs>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, OnPreviewMouseMoveCommandChanged));

    public static ICommandAsync<MouseEventArgs>? GetPreviewMouseMoveCommand(DependencyObject o) {
        return o.GetValue(PreviewMouseMoveCommandProperty) as ICommandAsync<MouseEventArgs>;
    }

    public static void SetPreviewMouseMoveCommand(DependencyObject o, ICommandAsync<MouseEventArgs> value) {
        o.SetValue(PreviewMouseMoveCommandProperty, value);
    }

    private static void OnPreviewMouseMoveCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element) return;

        if (e.OldValue == null && e.NewValue != null) element.PreviewMouseMove += OnPreviewMouseMoveChanged;
        else if (e is { OldValue: not null, NewValue: null }) element.PreviewMouseMove -= OnPreviewMouseMoveChanged;
    }

    private static void OnPreviewMouseMoveChanged(object sender, MouseEventArgs e) {
        if (!ReferenceEquals(sender, e.OriginalSource)) return;

        if (ObservableObject.IsDesignMode) return;

        if (sender is not UIElement obj) return;

        if (obj.GetValue(PreviewMouseMoveCommandProperty) is ICommandAsync<MouseEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion PreviewMouseMove Command

    #region PreviewMouseUp Command

    public static readonly DependencyProperty PreviewMouseUpCommandProperty = DependencyProperty.RegisterAttached("PreviewMouseUpCommand", typeof(ICommandAsync<MouseButtonEventArgs>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, OnPreviewMouseUpCommandChanged));

    public static ICommandAsync<MouseButtonEventArgs>? GetPreviewMouseUpCommand(DependencyObject o) {
        return o.GetValue(PreviewMouseUpCommandProperty) as ICommandAsync<MouseButtonEventArgs>;
    }

    public static void SetPreviewMouseUpCommand(DependencyObject o, ICommandAsync<MouseButtonEventArgs> value) {
        o.SetValue(PreviewMouseUpCommandProperty, value);
    }

    private static void OnPreviewMouseUpCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element) return;

        if (e.OldValue == null && e.NewValue != null) element.PreviewMouseUp += OnPreviewMouseUpChanged;
        else if (e is { OldValue: not null, NewValue: null }) element.PreviewMouseUp -= OnPreviewMouseUpChanged;
    }

    private static void OnPreviewMouseUpChanged(object sender, MouseButtonEventArgs e) {
        if (!ReferenceEquals(sender, e.OriginalSource)) return;

        if (ObservableObject.IsDesignMode) return;

        if (sender is not UIElement obj) return;

        if (obj.GetValue(PreviewMouseUpCommandProperty) is ICommandAsync<MouseButtonEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion PreviewMouseUp Command

    #region PreviewMouseDown Command

    public static readonly DependencyProperty PreviewMouseDownCommandProperty = DependencyProperty.RegisterAttached("PreviewMouseDownCommand", typeof(ICommandAsync<MouseButtonEventArgs>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, OnPreviewMouseDownCommandChanged));

    public static ICommandAsync<MouseButtonEventArgs>? GetPreviewMouseDownCommand(DependencyObject o) {
        return o.GetValue(PreviewMouseDownCommandProperty) as ICommandAsync<MouseButtonEventArgs>;
    }

    public static void SetPreviewMouseDownCommand(DependencyObject o, ICommandAsync<MouseButtonEventArgs> value) {
        o.SetValue(PreviewMouseDownCommandProperty, value);
    }

    private static void OnPreviewMouseDownCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element) return;

        if (e.OldValue == null && e.NewValue != null) element.PreviewMouseDown += OnPreviewMouseDownChanged;
        else if (e is { OldValue: not null, NewValue: null }) element.PreviewMouseDown -= OnPreviewMouseDownChanged;
    }

    private static void OnPreviewMouseDownChanged(object sender, MouseButtonEventArgs e) {
        if (!ReferenceEquals(sender, e.OriginalSource)) return;

        if (ObservableObject.IsDesignMode) return;

        if (sender is not UIElement obj) return;

        if (obj.GetValue(PreviewMouseDownCommandProperty) is ICommandAsync<MouseButtonEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion PreviewMouseDown Command

    #region PreviewMouseRightButtonDown Command

    public static readonly DependencyProperty PreviewMouseRightButtonDownCommandProperty = DependencyProperty.RegisterAttached("PreviewMouseRightButtonDownCommand", typeof(ICommandAsync<MouseButtonEventArgs>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, OnPreviewMouseRightButtonDownCommandChanged));

    public static ICommandAsync<MouseButtonEventArgs>? GetPreviewMouseRightButtonDownCommand(DependencyObject o) {
        return o.GetValue(PreviewMouseRightButtonDownCommandProperty) as ICommandAsync<MouseButtonEventArgs>;
    }

    public static void SetPreviewMouseRightButtonDownCommand(DependencyObject o, ICommandAsync<MouseButtonEventArgs> value) {
        o.SetValue(PreviewMouseRightButtonDownCommandProperty, value);
    }

    private static void OnPreviewMouseRightButtonDownCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element) return;

        if (e.OldValue == null && e.NewValue != null) element.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
        else if (e is { OldValue: not null, NewValue: null }) element.PreviewMouseRightButtonDown -= OnPreviewMouseRightButtonDown;
    }

    private static void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
        if (!ReferenceEquals(sender, e.OriginalSource)) return;

        if (ObservableObject.IsDesignMode) return;

        if (sender is not UIElement obj) return;

        if (obj.GetValue(PreviewMouseRightButtonDownCommandProperty) is ICommandAsync<MouseButtonEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion PreviewMouseRightButtonDown Command

    #region PreviewMouseWheel Command

    public static readonly DependencyProperty PreviewMouseWheelCommandProperty = DependencyProperty.RegisterAttached("PreviewMouseWheelCommand", typeof(ICommandAsync<MouseWheelEventArgs>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, OnPreviewMouseWheelCommandChanged));

    public static ICommandAsync<MouseWheelEventArgs>? GetPreviewMouseWheelCommand(DependencyObject o) {
        return o.GetValue(PreviewMouseWheelCommandProperty) as ICommandAsync<MouseWheelEventArgs>;
    }

    public static void SetPreviewMouseWheelCommand(DependencyObject o, ICommandAsync<MouseWheelEventArgs> value) {
        o.SetValue(PreviewMouseWheelCommandProperty, value);
    }

    private static void OnPreviewMouseWheelCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element) return;

        if (e.OldValue == null && e.NewValue != null) element.PreviewMouseWheel += OnPreviewMouseWheelChanged;
        else if (e is { OldValue: not null, NewValue: null }) element.PreviewMouseWheel -= OnPreviewMouseWheelChanged;
    }

    private static void OnPreviewMouseWheelChanged(object sender, MouseWheelEventArgs e) {
        if (!ReferenceEquals(sender, e.OriginalSource)) return;

        if (ObservableObject.IsDesignMode) return;

        if (sender is not UIElement obj) return;

        if (obj.GetValue(PreviewMouseWheelCommandProperty) is ICommandAsync<MouseWheelEventArgs> command && command.CanExecute(e)) command.Execute(e);
    }

    #endregion PreviewMouseWheel Command

    #region Focus Property

    public static readonly DependencyProperty FocusProperty = DependencyProperty.RegisterAttached("Focus", typeof(Func<bool>), typeof(UIElementBehaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFocusPropertyChanged));

    public static Func<bool>? GetFocus(DependencyObject o) {
        return o.GetValue(FocusProperty) as Func<bool>;
    }

    public static void SetFocus(DependencyObject o, Func<bool> value) {
        o.SetValue(FocusProperty, value);
    }

    private static void OnFocusPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not UIElement element || e.NewValue as Func<bool> == element.Focus) return;

        SetFocus(o, element.Focus);
    }

    #endregion Show Property

}
