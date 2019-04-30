using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

using Str.MvvmCommon.Core;
using Str.MvvmCommon.Helpers;


namespace Str.MvvmCommon.Behaviors {

  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Local")]
  public static class ViewModelLocator {

    #region ViewModel Property

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.RegisterAttached("ViewModel", typeof(string), typeof(ViewModelLocator), new FrameworkPropertyMetadata(null, OnViewModelPropertyChanged));

    public static string GetViewModel(DependencyObject o) {
      return o.GetValue(ViewModelProperty) as string;
    }

    public static void SetViewModel(DependencyObject o, string value) {
      o.SetValue(ViewModelProperty, value);
    }

    private static void OnViewModelPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (!(o is FrameworkElement element)) return;

      if (e.OldValue == null && e.NewValue != null) {
        string name = e.NewValue as string;

        object context = null;

        if (ObservableObject.IsDesignMode) {
          context = GetViewModelFromAttribute(name, true);
        }

        if (context == null) context = GetViewModelFromAttribute(name, false);

        element.DataContext = context;
      }
      else if (e.OldValue != null && e.NewValue == null) {
        element.DataContext = null;
      }
    }

    #endregion ViewModel Property

    #region ComposedViewModel Property

    public static readonly DependencyProperty ComposedViewModelProperty = DependencyProperty.RegisterAttached("ComposedViewModel", typeof(string), typeof(ViewModelLocator), new FrameworkPropertyMetadata(null, OnComposedViewModelPropertyChanged));

    public static string GetComposedViewModel(DependencyObject o) {
      return o.GetValue(ComposedViewModelProperty) as string;
    }

    public static void SetComposedViewModel(DependencyObject o, string value) {
      o.SetValue(ComposedViewModelProperty, value);
    }

    private static void OnComposedViewModelPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
      if (!(o is FrameworkElement element)) return;

      if (e.OldValue == null && e.NewValue != null) {
        string name = e.NewValue as string;

        object context;

        if (ObservableObject.IsDesignMode) {
          context = GetViewModelFromAttribute(name, true) ?? GetViewModelFromAttribute(name, false);
        }
        else context = GetComposedViewModelFromAttribute(name, false);

        element.DataContext = context;
      }
      else if (e.OldValue != null && e.NewValue == null) {
        element.DataContext = null;
      }
    }

    #endregion ComposedViewModel Property

    #region Private Methods

    private static object GetViewModelFromAttribute(string name, bool designMode) {
      Type type = GetViewModelTypeFromAttribute(name, designMode);

      return type == null ? null : Activator.CreateInstance(type);
    }

    private static object GetComposedViewModelFromAttribute(string name, bool designMode) {
      Type type = GetViewModelTypeFromAttribute(name, designMode);

      return type == null ? null : MvvmLocator.Container.Get(type);
    }

    private static Type GetViewModelTypeFromAttribute(string name, bool designMode) {
      Type type = null;

      try {
        type = (from t in MvvmLocator.Container.GetTypes()
                 let attribute = t.GetCustomAttributes(typeof(ViewModelAttribute), true).FirstOrDefault() as ViewModelAttribute
               where attribute != null
                  && attribute.Name == name
                  && attribute.IsDesignMode == designMode
              select t).FirstOrDefault();
      }
      catch(Exception ex) {
        Debug.Print("{0}\n{1}", ex.Message, ex.StackTrace);
      }

      return type;
    }

    #endregion Private Methods

  }

}
