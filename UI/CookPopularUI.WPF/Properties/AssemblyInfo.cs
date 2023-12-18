using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                               //(used if a resource is not found in the page,
                                               // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: ComVisible(true)]
[assembly: CLSCompliant(true)]
//[assembly: ObfuscateAssembly(true)]
//[assembly: ReferenceAssembly("CSharp Common Tools")]
//[assembly: PrimaryInteropAssembly(1, 2)]
//[assembly: XmlnsCompatibleWith("https://github.cookpopulartoolkit/2021/xaml", "https://github.cookpopularui/2021/xaml")]


[assembly: XmlnsPrefix("https://github.cookpopularui/2021/xaml", "ui")]
[assembly: XmlnsDefinition("https://github.cookpopularui/2021/xaml", "CookPopularUI.WPF")]
[assembly: XmlnsDefinition("https://github.cookpopularui/2021/xaml", "CookPopularUI.WPF.Controls")]
[assembly: XmlnsDefinition("https://github.cookpopularui/2021/xaml", "CookPopularUI.WPF.Themes")]
[assembly: XmlnsDefinition("https://github.cookpopularui/2021/xaml", "CookPopularUI.WPF.Tools")]
[assembly: XmlnsDefinition("https://github.cookpopularui/2021/xaml", "CookPopularUI.WPF.Windows")]


#if USE_ORIGIN_WPF_XMLNAMESPACE
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "CookPopularUI.WPF")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "CookPopularUI.WPF.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "CookPopularUI.WPF.Themes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "CookPopularUI.WPF.Tools")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "CookPopularUI.WPF.Windows")]
#endif
