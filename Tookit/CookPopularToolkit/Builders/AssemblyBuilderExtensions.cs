/*
 *Description: AssemblyBuilderExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 10:58:38
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CookPopularToolkit
{
    /// <summary>
    /// <see cref="CSharpScript"/>、<see cref="Roslyn"/>、
    /// <see cref="System.Reflection.Emit"/>、<see cref="CSharpCodeProvider"/>
    /// </summary>

    public class AssemblyBuilderExtensions
    {
        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="propertyType">属性基础类型(byte/int/string/float/double/bool等)和List基础类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyDefaultValue">属性初始值</param>
        /// <returns></returns>
        private static string PropertyString(string propertyType, string propertyName, string propertyDefaultValue)
        {
            StringBuilder sbproperty = new StringBuilder();
            sbproperty.Append(" private " + propertyType + "  _" + propertyName + "   =  " + propertyDefaultValue + ";\n");
            sbproperty.Append(" public " + propertyType + " " + propertyName + "\n");
            sbproperty.Append(" {\n");
            sbproperty.Append(" get{   return   _" + propertyName + ";}   \n");
            sbproperty.Append(" set{   _" + propertyName + "   =   value;   }\n");
            sbproperty.Append(" }\n");
            return sbproperty.ToString();
        }

        /// <summary>
        /// 创建动态类
        /// </summary>
        /// <param name="className">动态类名字</param>
        /// <param name="propertyList">属性列表</param>
        /// <returns></returns>
        public static Assembly? CreateAssembly(string className, List<string[]> propertyList)
        {
            //创建编译器实例。   
            CSharpCodeProvider provider = new CSharpCodeProvider();
            //设置编译参数。   
            CompilerParameters paras = new CompilerParameters();
            //编译器生成的临时文件，参数2为true，放置为当前文件夹下，false则放入windows的temp文件夹下
            paras.TempFiles = new TempFileCollection(".", false);
            //如果为true，则生成exe文件，false会生成临时dll文件
            paras.GenerateExecutable = false;
            paras.GenerateInMemory = true;

            //创建动态代码。   
            StringBuilder classsource = new StringBuilder();
            //float、bool等类型需要引用System
            classsource.Append("using System;\n");
            //List需要引用System.Collections.Generic
            classsource.Append("using System.Collections.Generic;\n");
            classsource.Append("public class " + className + " \n");
            classsource.Append("{\n");

            try
            {
                //创建属性。   
                for (int i = 0; i < propertyList.Count; i++)
                {
                    classsource.Append(PropertyString(propertyList[i][0], propertyList[i][1], propertyList[i][2]));
                }
                classsource.Append("}");
                System.Diagnostics.Debug.WriteLine(classsource.ToString());
                //编译代码。   
                CompilerResults result = provider.CompileAssemblyFromSource(paras, classsource.ToString());
                //获取编译后的程序集。   
                Assembly assembly = result.CompiledAssembly;

                return assembly;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static Assembly? CreateAssembly()
        {
            //创建编译器实例。   
            var provider = new CSharpCodeProvider();
            //设置编译参数。   
            var cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;

            // Generate an executable instead of 
            // a class library.
            //cp.GenerateExecutable = true;

            // Set the assembly file name to generate.
            cp.OutputAssembly = "c:\\1.dll";

            // Generate debug information.
            cp.IncludeDebugInformation = true;


            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Set the level at which the compiler 
            // should start displaying warnings.
            cp.WarningLevel = 3;

            // Set whether to treat all warnings as errors.
            cp.TreatWarningsAsErrors = false;

            // Set compiler argument to optimize output.
            cp.CompilerOptions = "/optimize";

            cp.ReferencedAssemblies.Add("System.dll");
            //cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.ReferencedAssemblies.Add("System.Data.dll");
            //cp.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
            cp.ReferencedAssemblies.Add("System.Deployment.dll");
            cp.ReferencedAssemblies.Add("System.Design.dll");
            cp.ReferencedAssemblies.Add("System.Drawing.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");


            //创建动态代码。   

            StringBuilder classSource = new StringBuilder();
            classSource.Append("using System;using System.Windows.Forms;\npublic   class   DynamicClass: UserControl \n");
            classSource.Append("{\n");
            classSource.Append("public DynamicClass()\n{\nInitializeComponent();\nConsole.WriteLine(\"hello\");}\n");
            classSource.Append("private System.ComponentModel.IContainer components = null;\nprotected override void Dispose(bool disposing)\n{\n");
            classSource.Append("if (disposing && (components != null)){components.Dispose();}base.Dispose(disposing);\n}\n");
            classSource.Append("private void InitializeComponent(){\nthis.SuspendLayout();this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);");
            classSource.Append("this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;this.Name = \"DynamicClass\";this.Size = new System.Drawing.Size(112, 74);this.ResumeLayout(false);\n}");
            //创建属性。   
            /*************************在这里改成需要的属性******************************/
            //classSource.Append(PropertyString("aaa"));
            //classSource.Append(propertyString("bbb"));
            //classSource.Append(propertyString("ccc"));

            classSource.Append("}");

            System.Diagnostics.Debug.WriteLine(classSource.ToString());


            //编译代码。   
            CompilerResults result = provider.CompileAssemblyFromSource(cp, classSource.ToString());
            if (result.Errors.Count > 0)
            {
                for (int i = 0; i < result.Errors.Count; i++)
                    Console.WriteLine(result.Errors[i]);
                Console.WriteLine("error");
                return null;
            }

            //获取编译后的程序集。   
            Assembly assembly = result.CompiledAssembly;

            return assembly;
        }
    }
}
