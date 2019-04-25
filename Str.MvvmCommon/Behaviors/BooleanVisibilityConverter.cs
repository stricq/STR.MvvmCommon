using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Str.MvvmCommon.Behaviors {

  [ValueConversion(typeof(bool), typeof(Visibility))]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public class BooleanVisibilityConverter : IValueConverter {

    #region IValueConverter Implementation

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return !(value is bool visible) ? Visibility.Hidden : visible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      Visibility? visible = value as Visibility?;

      if (visible.HasValue && visible.Value == Visibility.Hidden) return null;

      return visible == Visibility.Visible;
    }

    #endregion IValueConverter Implementation

  }

}
