/*
 *Description: IconPatternKind
 *Author: Chance.zheng
 *Creat Time: 2023/9/12 14:53:45
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 图标类型
    /// </summary>
    /// 使用以下方法可快速找到Resource中的Geometry的Key值
    /// <![CDATA[
    /// private static readonly ResourceDictionary GeometryFigureDictionary = new ResourceDictionary { Source = new Uri(@"pack://application:,,,/CookPopularUI.WPF;component/Resources/Dictionaries/GeometryFigure.xaml", UriKind.Absolute) };
    /// var list = new List<string>();
    /// foreach (var key in GeometryFigureDictionary.Keys)
    /// {
    ///     if (key != null)
    ///         list.Add(key.ToString()!.Replace("Geometry", ""));
    /// }
    /// list.Sort();
    ///
    /// StreamWriter streamWriter = new StreamWriter(@"C:\Users\Desktop\1.txt");
    /// streamWriter.Write(string.Join(",\n", list));
    /// streamWriter.Flush();
    /// streamWriter.Close();
    /// ]]>
    public enum IconPatternKind
    {
        Custom,
        Actual,
        Adaptive,
        AirFan,
        Alertor,
        Arch,
        AZ,
        Bottom,
        BottomTriangle,
        Catalog,
        Category,
        Check,
        Circle,
        Clock,
        Close,
        CloseCircle,
        ColorPicker,
        Copy,
        Cut,
        Date,
        DateClock,
        Error,
        Exit,
        ExitFullScreen,
        Eye,
        Failed,
        Fatal,
        FigureOne,
        FigureTwo,
        File,
        Flag,
        FlagOutline,
        Folder,
        FullScreen,
        Hide,
        HomePage,
        Info,
        Language,
        LastEpisode,
        Leaf,
        Left,
        LeftArrow,
        LeftTriangle,
        Line,
        Lock,
        Loop,
        Loving,
        Maximize,
        Minimize,
        Minus,
        MinusNoCircle,
        NextEpisode,
        Open,
        PageLeft,
        PageRight,
        Paste,
        Pause,
        Plane,
        Play,
        Plus,
        PlusNoCircle,
        Praise,
        Printer,
        Question,
        Rectangle,
        Refresh,
        ResizeGrip,
        Restart,
        Restore,
        Right,
        RightTriangle,
        Search,
        SelectAll,
        Setting,
        SinFunc,
        Smile,
        Square,
        Star,
        Stop,
        Success,
        Theme,
        Tool,
        Top,
        TopTriangle,
        VolumeHigh,
        VolumeMedium,
        VolumeOff,
        Warning,
        ZoomIn,
        ZoomOut
    }
}
