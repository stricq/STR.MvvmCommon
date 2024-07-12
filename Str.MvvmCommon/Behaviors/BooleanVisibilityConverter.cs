using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Str.MvvmCommon.Behaviors;


[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is a library.")]
[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
[ValueConversion(typeof(bool), typeof(Visibility))]
public class BooleanVisibilityConverter : IValueConverter {

    #region IValueConverter Implementation

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is not bool visible ? Visibility.Hidden : visible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        Visibility? visible = value as Visibility?;

        if (visible is Visibility.Hidden) return false;

        return visible == Visibility.Visible;
    }

    #endregion IValueConverter Implementation

}
