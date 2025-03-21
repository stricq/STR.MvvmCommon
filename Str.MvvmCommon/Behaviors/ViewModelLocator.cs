﻿using System.Diagnostics.CodeAnalysis;
using System.Windows;

using Str.MvvmCommon.Helpers;


namespace Str.MvvmCommon.Behaviors;


[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is a library.")]
[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
public static class ViewModelLocator {

    #region ViewModel Property

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.RegisterAttached("ViewModel", typeof(Type), typeof(ViewModelLocator), new FrameworkPropertyMetadata(null, OnViewModelPropertyChanged));

    public static Type? GetViewModel(DependencyObject o) {
        return o.GetValue(ViewModelProperty) as Type;
    }

    public static void SetViewModel(DependencyObject o, Type value) {
        o.SetValue(ViewModelProperty, value);
    }

    private static void OnViewModelPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
        if (o is not FrameworkElement element) return;

        if (e.OldValue == null && e.NewValue != null) {
            if (e.NewValue is Type type) element.DataContext = MvvmLocator.Container?.Get(type);
        }
        else if (e is { OldValue: not null, NewValue: null }) {
            element.DataContext = null;
        }
    }

    #endregion ViewModel Property

}
