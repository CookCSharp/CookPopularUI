/*
 *Description: ObjectBuilder
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 11:05:04
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit
{
    /// <summary>
    /// 创建类
    /// </summary>
    public static class ObjectBuilder
    {
        public class PropertyDefine
        {
            public string PropertyName { get; set; }

            public Type PropertyType { get; set; }

            public object PropertyValue { get; set; }
        }

        private static TypeBuilder GetTypeBuilder(string assemblyName)
        {
            TypeAttributes typeAttributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass |
                                            TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout;

            var name = new AssemblyName(assemblyName);
#if NETFRAMEWORK
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
#else
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
#endif
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("CustomModule");
            TypeBuilder tb = moduleBuilder.DefineType(assemblyName, typeAttributes, null);

            //assemblyBuilder.Save("");
            return tb;
        }

        private static void GetPropertyIL(MethodBuilder getPropertyBuilder, FieldBuilder fieldBuilder)
        {
            var getAIL = getPropertyBuilder.GetILGenerator();
            getAIL.Emit(OpCodes.Ldarg_0);   //this
            getAIL.Emit(OpCodes.Ldfld, fieldBuilder); //numA
            getAIL.Emit(OpCodes.Ret); //return numA
        }

        private static void SetPropertyIL(MethodBuilder setPropertyBuilder, FieldBuilder fieldBuilder)
        {
            var setAIL = setPropertyBuilder.GetILGenerator();
            //setAIL.Emit(OpCodes.Nop);   //这句可省略
            setAIL.Emit(OpCodes.Ldarg_0);  //this
            setAIL.Emit(OpCodes.Ldarg_1);  //value
            setAIL.Emit(OpCodes.Stfld, fieldBuilder); //numA = value;
            setAIL.Emit(OpCodes.Ret);   //return;
        }

        private static void CreateProperty(TypeBuilder tb, Type propertyType, string propertyName)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr = tb.DefineMethod("set_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { propertyType });
            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }

        /// <summary>
        /// 使用<see cref="System.Reflection.Emit"/>动态创建对象
        /// </summary>
        /// <param name="propertyDefines"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static object? CreateObject(List<PropertyDefine> propertyDefines, string? assemblyName = null)
        {
            assemblyName = string.IsNullOrEmpty(assemblyName) ? "CustomAssembly" : assemblyName;
            TypeBuilder tb = GetTypeBuilder(assemblyName!);
            ConstructorBuilder constructorBuilder = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            foreach (var field in propertyDefines)
            {
                CreateProperty(tb, field.PropertyType, field.PropertyName);
            }

            Type? objectType = tb.CreateTypeInfo()?.AsType();
            if (objectType == null) return null;
            var instance = Activator.CreateInstance(objectType);

            foreach (var field in propertyDefines)
            {
                instance.SetPropertyValueInReflect(field.PropertyName, field.PropertyValue);
            }

            return instance;
        }

        /// <summary>
        /// 泛型方式创建对象
        /// </summary>
        /// <returns><typeparamref name="T"/></returns>
        public static T CreateInstanceInGenerate<T>() where T : new()
        {
            return new T();
        }

        /// <summary>
        /// 使用<see cref="System.Reflection.Emit"/>在当前程序集中创建实例
        /// </summary>
        /// <returns><typeparamref name="T"/></returns>
        public static T? CreateInstanceInEmit<T>() => (T?)CreateInstanceInEmit(typeof(T));

        public static object? CreateInstanceInEmit(Type type)
        {
            try
            {
                ConstructorInfo defaultCtor = type.GetConstructor(new Type[] { });
                DynamicMethod dynamicMethod = new DynamicMethod(string.Format("_{0:N}", Guid.NewGuid()), type, null);

                var gen = dynamicMethod.GetILGenerator();
                gen.Emit(OpCodes.Newobj, defaultCtor);
                gen.Emit(OpCodes.Ret);

                //var objCreator = dynamicMethod.CreateDelegate(typeof(Func<T>)) as Func<T>;
                //return objCreator.IsNull() ? default : objCreator!.Invoke();

                return dynamicMethod.Invoke(type, new object[] { });
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 使用<see cref="Activator"/>在当前程序集中创建对象
        /// </summary>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <returns></returns>
        public static object? CreateInstanceInActivator(string typeName)
        {
            try
            {
                var type = Type.GetType(typeName);

                return type.IsNull() ? default : Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 使用<see cref="Activator"/>在当前程序集中创建对象
        /// </summary>
        /// <returns><typeparamref name="T"/></returns>
        public static T? CreateInstanceInActivator<T>()
        {
            var type = typeof(T);
            var intance = CreateInstanceInActivator(type.FullName);

            return intance.IsNull() ? default : (T)intance!;
        }

        /// <summary>
        /// 使用<see cref="Activator"/>在当前程序集中创建对象
        /// </summary>
        /// <param name="nameSpaceStr">完整命名空间名称</param>
        /// <param name="className">类名称</param>
        /// <returns></returns>
        public static object? CreateInstanceInActivator(string nameSpaceStr, string className)
        {
            var type = Type.GetType($"{nameSpaceStr}.{className}");
            var intance = CreateInstanceInActivator(type.FullName);

            return intance.IsNull() ? default : intance;
        }

        /// <summary>
        /// 使用<see cref="Assembly.LoadFile(string)"/>创建对象
        /// </summary>
        /// <param name="assemblyFilePath">程序集路径，不能是相对路径(exe/dll)</param>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <remarks>没用引用该程序集时</remarks>
        /// <returns></returns>
        public static object? CreateInstanceInAssemblyFile(string assemblyFilePath, string typeName)
        {
            try
            {
                var assembly = Assembly.LoadFile(assemblyFilePath);
                var instance = assembly?.CreateInstance(typeName);

                return instance;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 使用<see cref="Assembly.Load(string)"/>创建对象
        /// </summary>
        /// <param name="assemblyName">程序集名称，不含后缀名</param>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <remarks>不同程序集,装载调用</remarks>
        /// <returns></returns>
        public static object? CreateInstanceInAssemblyName(string assemblyName, string typeName)
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                var instance = assembly?.CreateInstance(typeName, false);

                return instance;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 使用<see cref="Assembly"/>在当前程序集中创建对象
        /// </summary>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <returns></returns>
        public static object? CreateInstanceInCurrentAssembly(string typeName)
        {
            try
            {
                var assembly = Assembly.GetEntryAssembly(); //获取当前程序集
                var instance = assembly.CreateInstance(typeName);

                //var type = Type.GetType(typeName);
                //var instance = type.Assembly.CreateInstance(type.FullName);

                return instance;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 使用<see cref="Assembly"/>在当前程序集中创建对象
        /// </summary>
        /// <returns><typeparamref name="T"/></returns>
        public static T? CreateInstanceInCurrentAssembly<T>()
        {
            var instance = CreateInstanceInCurrentAssembly(typeof(T).FullName);

            return instance.IsNull() ? default : (T)instance!;
        }
    }
}
