[![Fork me on Github](Demos/CookPopularUI.WPFDemo/Assets/CookCSharp.png)](https://github.com/CookCSharp/CookPopularUI)

# Welcome to CookPopularUI

## **介绍**

CookPopularUI是支持.Net462+、.NetCore3.1、.Net6.0+的一款Xaml控件库。目前只包含WPF控件库，其中参考了一些资料，提供了100多款常用控件，并持续更新。如果你的项目用到此库，不要忘记点个赞，有问题可加QQ群交流：658794308，欢迎大家参与开发和指出问题。
***

## **使用**

- 添加Nuget包引用
  ```
  <PackageReference Include="CookPopularUI.WPF" Version="1.0.1-preview2" />
  ```

- 添加如下代码即可全部引用（两种方式皆可）
  ```
  <Application.Resources>
      <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!--<ResourceDictionary Source="pack://application:,,,/CookPopularUI.WPF;component/Themes/DefaultPopularColor.xaml" />-->
                <!--<ResourceDictionary Source="pack://application:,,,/CookPopularUI.WPF;component/Themes/DefaultPopularControl.xaml" />-->
                <ui:PopularTheme Language="English" Theme="Light" />
            </ResourceDictionary.MergedDictionaries>
      </ResourceDictionary>
  </Application.Resources>
  ```

## **效果**

- **Home**
    ![效果](Resources/Effect/Home.png)

- **BlockBar**
    ![效果](Resources/Effect/BlockBar.png)

- **Border**
    ![效果](Resources/Effect/Border.png)
    ![效果](Resources/Effect/BorderEx1.png)
    ![效果](Resources/Effect/BorderEx2.png)

- **Button**
    ![效果](Resources/Effect/Button.png)

- **Carousel**
    ![效果](Resources/Effect/Carousel.gif)

- **CheckBox**
    ![效果](Resources/Effect/CheckBox.png)

- **ColorPicker**
    ![效果](Resources/Effect/ColorPicker.gif)

- **ComboBox**
    ![效果](Resources/Effect/ComboBox.gif)

- **DataGrid**
    ![效果](Resources/Effect/DataGrid.png)

- **DateTime**
    ![效果](Resources/Effect/DateTime.gif)

- **DialogBox**
    ![效果](Resources/Effect/DialogBox.gif)

- **Expander**
    ![效果](Resources/Effect/Expander.png)

- **Geometry**
    ![效果](Resources/Effect/Geometry.png)

- **GroupBox**
    ![效果](Resources/Effect/GroupBox.png)

- **Icon**
    ![效果](Resources/Effect/Icon.gif)

- **Input**
    ![效果](Resources/Effect/Input.png)

- **Label**
    ![效果](Resources/Effect/Label.png)

- **ListBox**
    ![效果](Resources/Effect/ListBox.png)

- **ListView**
    ![效果](Resources/Effect/ListView.png)

- **MarkupExtension**
    ![效果](Resources/Effect/MarkupExtension.png)

- **Media**
    ![效果](Resources/Effect/Media.png)

- **Menu**
    ![效果](Resources/Effect/Menu.gif)

- **Message**
    ![效果](Resources/Effect/Message.gif)

- **OtherButton**
    ![效果](Resources/Effect/OtherButton.png)

- **OtherControl**
    ![效果](Resources/Effect/OtherControl.png)

- **Panel**
    ![效果](Resources/Effect/Panel.png)

- **ProgressBar**
    ![效果](Resources/Effect/ProgressBar.gif)

- **ProgressButton**
    ![效果](Resources/Effect/ProgressButton.gif)

- **PropertyGrid**
    ![效果](Resources/Effect/PropertyGrid.png)

- **RadioButton**
    ![效果](Resources/Effect/RadioButton.png)

- **RangeSlider**
    ![效果](Resources/Effect/RangeSlider.gif)

- **ScrollViewer**
    ![效果](Resources/Effect/ScrollViewer.png)

- **Shape**
    ![效果](Resources/Effect/Shape.png)

- **SideBar**
    ![效果](Resources/Effect/SideBar.gif)

- **SideBarEx**
    ![效果](Resources/Effect/SideBarEx.gif)

- **StepBar**
    ![效果](Resources/Effect/StepBar.gif)

- **Swiper**
    ![效果](Resources/Effect/Swiper.gif)

- **TabControl**
    ![效果](Resources/Effect/TabControl.gif)

- **Text**
    ![效果](Resources/Effect/Text.png)

- **TitleBar**
    ![效果](Resources/Effect/TitleBar.png)

- **ToggleButton**
    ![效果](Resources/Effect/ToggleButton.png)

- **ToolBar**
    ![效果](Resources/Effect/ToolBar.png)

- **TreeView**
    ![效果](Resources/Effect/TreeView.png)

- **VirtualizingWrapPanel**
    ![效果](Resources/Effect/VirtualizingWrapPanel.gif)

- **Window**
    ![效果](Resources/Effect/Window.gif)

- **WrapPanelFill**
    ![效果](Resources/Effect/WrapPanelFill.png)

### 准备新加入控件

- 方形布局(子项靠近边框/子项居中)、圆形布局、ControlPanel
- Boxes：增加相框控件
- ProcessBar
- ButtonGroup、ToggleButtonGroup、CheckBoxGroup、RadioButtonGroup
- PageScroller
- Extensions <br />
  CookPopularUI.WPF.BarCode <br />
  CookPopularUI.WPF.QRCode <br />
  CookPopularUI.WPF.Animation <br />
  CookPopularUI.Compress <br />

### 遗留问题

- WPFDemo采用Prism、Caliburn.Micro、ReactiveUI、CommunityTools、(WAF、MEF)这几种mvvm框架示例
- MessageDialog系统菜单栏应该有关闭和移动按钮可以点击
- 窗体ShowDialog时标题栏应该闪烁
- PropertyGrid中自定义Item编辑器，DockPanel.Dock
- 资源国际化扩展（可自定义）
- Demo中Button类别分开展示
- UniformSpacePanel水平时水平设置无效、垂直时垂直设置无效
- SideBarEx动画不和谐
- ItemsControl类型控件大量数据性能测试

### 单元测试

1. 单元测试
2. BentchMark性能测试

## ⭐️ Stargazers

<img src="https://starchart.cc/CookCSharp/CookPopularUI.svg" alt="Stargazers over time" style="max-width: 100%">

<!-- ## 🏆 Forkers

<img src="https://forkchart.cc/CookCSharp/CookPopularUI.svg" alt="Fork over time" style="max-width: 100%"> -->