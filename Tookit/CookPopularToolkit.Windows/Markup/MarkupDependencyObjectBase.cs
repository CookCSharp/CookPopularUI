/*
 *Description: MarkupDependencyObjectBase
 *Author: Chance.zheng
 *Creat Time: 2023/11/22 13:36:39
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CookPopularToolkit.Windows
{
    public abstract class MarkupDependencyObjectBase : MarkupExtensionBase
    {
        //private Dispatcher _dispatcher;
        //private DependencyObjectType _dType;

        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //public Dispatcher Dispatcher => _dispatcher;

        //public DependencyObjectType DependencyObjectType
        //{
        //    get
        //    {
        //        if (_dType == null)
        //        {
        //            _dType = DependencyObjectType.FromSystemType(GetType());
        //        }

        //        return _dType;
        //    }
        //}

        //protected MarkupDependencyObjectBase()
        //{
        //    _dispatcher = Dispatcher.CurrentDispatcher;
        //}

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool CheckAccess()
        //{
        //    bool result = true;
        //    Dispatcher dispatcher = _dispatcher;
        //    if (dispatcher != null)
        //    {
        //        result = dispatcher.CheckAccess();
        //    }

        //    return result;
        //}

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void VerifyAccess()
        //{
        //    _dispatcher?.VerifyAccess();
        //}

        //private PropertyMetadata SetupPropertyChange(DependencyProperty dp)
        //{
        //    if (dp != null)
        //    {
        //        if (!dp.ReadOnly)
        //        {
        //            return dp.GetMetadata(DependencyObjectType);
        //        }

        //        throw new InvalidOperationException(SR.Get("ReadOnlyChangeNotAllowed", dp.Name));
        //    }

        //    throw new ArgumentNullException("dp");
        //}

        //public void SetValue(DependencyProperty dp, object value)
        //{
        //    VerifyAccess();
        //    PropertyMetadata metadata = SetupPropertyChange(dp);
        //    SetValueCommon(dp, value, metadata, coerceWithDeferredReference: false, coerceWithCurrentValue: false, OperationType.Unknown, isInternal: false);
        //}

        public void SetValue(DependencyProperty dp, object value)
        {
            var instance = (DependencyObject)Activator.CreateInstance(typeof(DependencyObject));
            instance.SetValue(dp, value);
            //typeof(DependencyObject).GetMethod("SetValue",System.Reflection.BindingFlags.NonPublic)
        }

        public object GetValue(DependencyProperty dp)
        {
            var instance = (DependencyObject)Activator.CreateInstance(typeof(DependencyObject));
            return instance.GetValue(dp);
            //typeof(DependencyObject).GetMethod("SetValue",System.Reflection.BindingFlags.NonPublic)
        }
    }
}
