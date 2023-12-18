/*
 *Description: ObjectExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/15 9:19:30
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace CookPopularToolkit
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object? obj) => obj == null;

        /// <summary>
        /// 装箱
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object ToObject(this object obj) => obj;

        /// <summary>
        /// 拆箱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>T</returns>
        /// <remarks>类型不一致则返回值,否则返回该类型(TType)的默认值</remarks>
        public static T? ToTargetType<T>(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj.GetType().Equals(typeof(T)) ? (T)obj : default(T);
        }

        /// <summary>
        /// 拆箱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>T</returns>
        /// <remarks>强制转换</remarks>
        public static T ToCastTargetType<T>(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return new Converter<object, T>(p =>
            (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(obj)).Invoke(obj);
        }

        public static T? CastTo<T>(this object value)
        {
            return typeof(T).IsValueType && value != null ? (T)Convert.ChangeType(value, typeof(T))
                                                          : value is T typeValue ? typeValue : default;
        }

        /// <summary>
        /// 反射调用指定类型的Internal方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="caller"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <remarks>
        /// .Net通过"NotifyPropertyChange"主动触发通知
        /// 当子属性发生变化时，发送一个Changed通知，新旧值相同，并将IsASubPropertyChange设置为true
        /// NotifyPropertyChange(new DependencyPropertyChangedEventArgs(dp, dp.GetMetadata(DependencyObjectType), GetValue(dp)));
        /// </remarks>
        /// <![CDATA[InvokeInternal<DependencyObject>("NotifySubPropertyChange", new object[] { DpColorProperty });]]>
        /// <returns></returns>
        public static object? InvokeInternal<T>(this T caller, string method, object[] parameters)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic);
            return methodInfo?.Invoke(caller, parameters);
        }
    }
}
