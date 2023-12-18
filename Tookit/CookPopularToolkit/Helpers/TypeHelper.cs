/*
 *Description: TypeHelper
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 11:42:54
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace CookPopularToolkit
{
    public static class TypeHelper
    {
        /// <summary>
        /// 属性赋值
        /// </summary>
        /// <param name="t">对象</param>
        /// <param name="propertyname">属性名称</param>
        /// <param name="value">属性值</param>
        /// <remarks>使用反射</remarks>
        public static void SetPropertyValueInReflect<T>(this T t, string propertyname, object value)
        {
            CheckHelper.ArgumentNullException(t, nameof(t));

            var propertyInfo = t!.GetType().GetProperty(propertyname);
            if (propertyInfo != null)
                propertyInfo.SetValue(t, value, null);
        }


        /// <summary>
        /// 属性赋值
        /// </summary>
        /// <param name="propertyname">属性名称</param>
        /// <param name="value">属性值</param>
        /// <remarks>使用反射</remarks>
        public static T? SetPropertyValueInReflect<T>(string propertyname, object value)
        {
            object? instance = Activator.CreateInstance(typeof(T));

            if (instance == null)
                return default;

            instance.SetPropertyValueInReflect(propertyname, value);

            return (T)instance;
        }


        /// <summary>
        /// 给对象所有属性赋默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <remarks>被<see cref="DefaultValueAttribute"/>标记的属性将赋值</remarks>
        public static void SetPropertiesDefaultValue<T>(this T t)
        {
            CheckHelper.ArgumentNullException(t, nameof(t));

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(t!))
            {
                var attribute = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
                if (attribute == null) continue;
                prop.SetValue(t, attribute.Value);
            }
        }


        /// <summary>
        /// 给类型所有对象赋默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks>被<see cref="DefaultValueAttribute"/>标记的属性将赋值</remarks>
        public static T? SetPropertiesDefaultValue<T>()
        {
            var instance = Activator.CreateInstance(typeof(T));
            instance.SetPropertiesDefaultValue();

            if (instance == null)
                return default;

            return (T)instance;
        }


        /// <summary>
        /// 属性赋值
        /// </summary>
        /// <param name="t">对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <remarks>
        /// 使用<see cref="ILGenerator"/>
        /// </remarks>
        public static void SetPropertyValueInEmit<T>(this T t, string propertyName, object propertyValue)
        {
            CheckHelper.ArgumentNullException(t, nameof(t));

            var type = t!.GetType();//"EmitCallable"
            var dynamicMethod = new DynamicMethod("EmitCallable", null, new[] { type, typeof(object) }, type.Module);
            var iLGenerator = dynamicMethod.GetILGenerator();

            var callMethod = type.GetMethod("set_" + propertyName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
            if (callMethod == null) return;
            var parameterInfo = callMethod.GetParameters()[0];
            var local = iLGenerator.DeclareLocal(parameterInfo.ParameterType, true);

            iLGenerator.Emit(OpCodes.Ldarg_1);
            if (parameterInfo.ParameterType.IsValueType)
            {
                // 如果是值类型，拆箱
                iLGenerator.Emit(OpCodes.Unbox_Any, parameterInfo.ParameterType);
            }
            else
            {
                // 如果是引用类型，转换
                iLGenerator.Emit(OpCodes.Castclass, parameterInfo.ParameterType);
            }

            iLGenerator.Emit(OpCodes.Stloc, local);
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldloc, local);

            iLGenerator.EmitCall(OpCodes.Callvirt, callMethod, null);
            iLGenerator.Emit(OpCodes.Ret);

            dynamicMethod.Invoke(t, new object[] { t, propertyValue });
            //var action = dynamicMethod.CreateDelegate(typeof(Action<T, object>)) as Action<T, object>;
            //action?.Invoke(t, propertyValue);
        }


        /// <summary>
        /// 属性赋值
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <remarks>
        /// 使用<see cref="ILGenerator"/>
        /// </remarks>
        public static T? SetPropertyValueInEmit<T>(string propertyName, object propertyValue)
        {
            var type = typeof(T);
            object? instance = Activator.CreateInstance(type);

            if (instance == null)
                return default;

            instance.SetPropertyValueInEmit(propertyName, propertyValue);

            return (T)instance;
        }


        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="t">对象</param>
        /// <param name="propertyname">属性名称</param>
        /// <returns>属性值，是object类型，使用时记得转换</returns>
        public static object? GetPropertyValue<T>(this T t, string propertyname)
        {
            try
            {
                CheckHelper.ArgumentNullException(t, nameof(t));

                var propertyInfo = t!.GetType().GetProperty(propertyname);
                return propertyInfo?.GetValue(t, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取属性初始值
        /// </summary>
        /// <param name="propertyname">属性名称</param>
        /// <returns>属性值，是object类型，使用时记得转换</returns>
        public static object? GetPropertyInitValue<T>(string propertyname)
        {
            var obj = Activator.CreateInstance(typeof(T));
            return obj.GetPropertyValue(propertyname);
        }


        /// <summary>
        /// 获取某个类中所有属性值
        /// </summary>
        /// <param name="t">对象</param>
        /// <returns>类中所有属性值</returns>
        public static IList<object> GetPropertyValues(this object t)
        {
            try
            {
                CheckHelper.ArgumentNullException(t, nameof(t));

                IList<object> propertyValues = new List<object>();
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(t!))
                {
                    propertyValues.Add(prop.GetValue(t));
                }

                return propertyValues;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取某个类中所有属性初始值
        /// </summary>
        /// <returns>类中所有属性初始值</returns>
        public static IList<object> GetPropertyInitValues(this object t)
        {
            var instance = Activator.CreateInstance(t.GetType());
            return GetPropertyValues(instance);
        }


        /// <summary>
        /// 获取某个类中所有属性初始值
        /// </summary>
        /// <returns>类中所有属性初始值</returns>
        public static IList<object> GetPropertyInitValues<T>()
        {
            var instance = Activator.CreateInstance(typeof(T));
            return GetPropertyValues(instance);
        }


        /// <summary>
        /// 获取某个类中所有属性上指定特性的属性值
        /// </summary>
        /// <typeparam name="TAttribute">特性</typeparam>
        /// <param name="t">对象</param>
        /// <returns>类中所有属性上指定特性的属性值</returns>
        public static IList<object> GetAttriutePropertyValues<TAttribute>(this object t) where TAttribute : Attribute
        {
            try
            {
                CheckHelper.ArgumentNullException(t, nameof(t));

                IList<object> propertyValues = new List<object>();
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(t!))
                {
                    var attribute = prop.Attributes[typeof(TAttribute)];
                    if (attribute == null) continue;
                    propertyValues.Add(prop.GetValue(t));
                }

                return propertyValues;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取某个类中所有属性上指定特性的属性初始值
        /// </summary>
        /// <typeparam name="TType">类型</typeparam>
        /// <typeparam name="TAttribute">特性</typeparam>
        /// <returns>类中所有属性上指定特性的属性初始值</returns>
        public static IList<object> GetAttriutePropertyInitValues<TType, TAttribute>() where TAttribute : Attribute
        {
            var obj = Activator.CreateInstance(typeof(TType));
            return obj.GetAttriutePropertyValues<TAttribute>();
        }


        /// <summary>
        /// 获取某个类中所有属性上<see cref="DescriptionAttribute"/>标记的<see cref="DescriptionAttribute.Description"/>值
        /// </summary>
        /// <typeparam name="T">特性</typeparam>
        /// <returns>类中所有属性上<see cref="DescriptionAttribute"/>标记的<see cref="DescriptionAttribute.Description"/>值</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static IList<object> GetDescriptionAttributeValuesOfProperty<T>()
        {
            try
            {
                IList<object> descriptionValues = new List<object>();
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(typeof(T)))
                {
                    var attribute = prop.Attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
                    if (attribute == null) continue;
                    descriptionValues.Add(attribute.Description);
                }

                return descriptionValues;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取某个类中所有属性上<see cref="DefaultValueAttribute"/>标记的<see cref="DefaultValueAttribute.Value"/>值
        /// </summary>
        /// <typeparam name="T">特性</typeparam>
        /// <returns>类中所有属性上<see cref="DefaultValueAttribute"/>标记的<see cref="DefaultValueAttribute.Value"/>值</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static IList<object> GetDefaultValueAttributeValuesOfProperty<T>()
        {
            try
            {
                IList<object> defaultValues = new List<object>();
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(typeof(T)))
                {
                    var attribute = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
                    if (attribute == null) continue;
                    defaultValues.Add(attribute.Value);
                }

                return defaultValues;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取某个类中所有属性上某个特性标记的某个特性的属性值
        /// </summary>
        /// <typeparam name="TType">类</typeparam>
        /// <typeparam name="TAttribute">特性</typeparam>
        /// <param name="attributePropertyName">特性的属性名称</param>
        /// <returns>类中所有属性上某个特性标记的某个特性的属性值</returns>
        public static IList<object> GetAttributeValuesOfProperty<TType, TAttribute>(string attributePropertyName) where TAttribute : Attribute
        {
            try
            {
                CheckHelper.ArgumentNullOrEmptyException(attributePropertyName, nameof(attributePropertyName));

                IList<object> defaultValues = new List<object>();
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(typeof(TType)))
                {
                    var attributeType = typeof(TAttribute);
                    var attribute = prop.Attributes[attributeType] as TAttribute;
                    if (attribute == null) continue;

                    var attributeProperty = attributeType.GetProperties().Where(p => p.Name == attributePropertyName).FirstOrDefault();
                    CheckHelper.ArgumentNullExceptionWithMessage(attributeProperty, $"{attributeType.Name} has no property nameof {attributePropertyName}");
                    var attributePropertyValue = attributeProperty.GetValue(attribute);
                    defaultValues.Add(attributePropertyValue);
                }

                return defaultValues;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
