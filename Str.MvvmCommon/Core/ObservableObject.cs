using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using Str.Common.Extensions;


namespace Str.MvvmCommon.Core;


[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "This is a library.")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is a library.")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "This is a library.")]
public class ObservableObject : INotifyPropertyChanged {

    #region INotifyPropertyChanged Implementation

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion INotifyPropertyChanged Implementation

    #region Properties

    public static bool IsDesignMode {
        get {
            using Process process = Process.GetCurrentProcess();

            string name = process.ProcessName.ToLower().Trim();

            return name == "devenv" || name == "xdesproc";
        }
    }

    #endregion Properties

    #region Protected Methods

    protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpresssion) {
        if (propertyExpresssion.Body is not MemberExpression memberInfo) throw new ArgumentException("The parameter is not a property.", nameof(propertyExpresssion));

        PropertyInfo propertyInfo = memberInfo.Member as PropertyInfo
                                 ?? throw new ArgumentException("The specified property does not exist.", nameof(propertyExpresssion));

        OnPropertyChanged(new PropertyChangedEventArgs(propertyInfo.Name));
    }

    protected void RaisePropertyChanged() {
        OnPropertyChanged(new PropertyChangedEventArgs(String.Empty));
    }

    protected bool SetField(ref double field, double value, params Expression<Func<double>>[] selectorArray) {
        if (value.HasMinimalDifference(field)) return false;

        field = value;

        if (selectorArray.Length == 0) RaisePropertyChanged();
        else selectorArray.ForEach(RaisePropertyChanged);

        return true;
    }

    protected bool SetField<T1>(ref double field, double value, Expression<Func<double>> selector, Expression<Func<T1>> selector1) {
        if (value.HasMinimalDifference(field)) return false;

        field = value;

        RaisePropertyChanged(selector);
        RaisePropertyChanged(selector1);

        return true;
    }

    protected bool SetField<T1, T2>(ref double field, double value, Expression<Func<double>> selector, Expression<Func<T1>> selector1, Expression<Func<T2>> selector2) {
        if (value.HasMinimalDifference(field)) return false;

        field = value;

        RaisePropertyChanged(selector);
        RaisePropertyChanged(selector1);
        RaisePropertyChanged(selector2);

        return true;
    }

    protected bool SetField<T1, T2, T3>(ref double field, double value, Expression<Func<double>> selector, Expression<Func<T1>> selector1, Expression<Func<T2>> selector2, Expression<Func<T3>> selector3) {
        if (value.HasMinimalDifference(field)) return false;

        field = value;

        RaisePropertyChanged(selector);
        RaisePropertyChanged(selector1);
        RaisePropertyChanged(selector2);
        RaisePropertyChanged(selector3);

        return true;
    }

    protected bool SetField<T>(ref T field, T value, params Expression<Func<T>>[] selectorArray) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;

        if (selectorArray.Length == 0) RaisePropertyChanged();
        else selectorArray.ForEach(RaisePropertyChanged);

        return true;
    }

    protected bool SetField<T, T1>(ref T field, T value, Expression<Func<T>> selector, Expression<Func<T1>> selector1) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;

        RaisePropertyChanged(selector);
        RaisePropertyChanged(selector1);

        return true;
    }

    protected bool SetField<T, T1, T2>(ref T field, T value, Expression<Func<T>> selector, Expression<Func<T1>> selector1, Expression<Func<T2>> selector2) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;

        RaisePropertyChanged(selector);
        RaisePropertyChanged(selector1);
        RaisePropertyChanged(selector2);

        return true;
    }

    protected bool SetField<T, T1, T2, T3>(ref T field, T value, Expression<Func<T>> selector, Expression<Func<T1>> selector1, Expression<Func<T2>> selector2, Expression<Func<T3>> selector3) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;

        RaisePropertyChanged(selector);
        RaisePropertyChanged(selector1);
        RaisePropertyChanged(selector2);
        RaisePropertyChanged(selector3);

        return true;
    }

    protected bool SetField<T, T1, T2, T3, T4>(ref T field, T value, Expression<Func<T>> selector, Expression<Func<T1>> selector1, Expression<Func<T2>> selector2, Expression<Func<T3>> selector3, Expression<Func<T4>> selector4) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;

        RaisePropertyChanged(selector);
        RaisePropertyChanged(selector1);
        RaisePropertyChanged(selector2);
        RaisePropertyChanged(selector3);
        RaisePropertyChanged(selector4);

        return true;
    }

    protected bool SetField<T, T1, T2, T3, T4, T5>(ref T field, T value, Expression<Func<T>> selector, Expression<Func<T1>> selector1, Expression<Func<T2>> selector2, Expression<Func<T3>> selector3, Expression<Func<T4>> selector4, Expression<Func<T5>> selector5) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;

        RaisePropertyChanged(selector);
        RaisePropertyChanged(selector1);
        RaisePropertyChanged(selector2);
        RaisePropertyChanged(selector3);
        RaisePropertyChanged(selector4);
        RaisePropertyChanged(selector5);

        return true;
    }

    #endregion Protected Methods

    #region Private Methods

    private void OnPropertyChanged(PropertyChangedEventArgs pce) {
        PropertyChanged?.Invoke(this, pce);
    }

    #endregion Private Methods

}
