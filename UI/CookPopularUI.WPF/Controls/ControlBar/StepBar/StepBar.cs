/*
 *Description: StepBar
 *Author: Chance.zheng
 *Creat Time: 2023/9/8 10:12:26
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(StepBarItem))]
    [DefaultEvent("StepChanged")]
    [TemplatePart(Name = ElementProgressBar, Type = typeof(ProgressBar))]
    public class StepBar : ItemsControl
    {
        private const string ElementProgressBar = "PART_ProgressBar";

        private ProgressBar _progressBar;
        private int _originStepIndex = -1;


        /// <summary>
        /// 停靠位置
        /// </summary>
        public Dock Dock
        {
            get => (Dock)GetValue(DockProperty);
            set => SetValue(DockProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Dock"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register(nameof(Dock), typeof(Dock), typeof(StepBar), new PropertyMetadata(Dock.Top));


        /// <summary>
        /// 表示步骤的类型
        /// </summary>
        public StepControlKind StepKind
        {
            get => (StepControlKind)GetValue(StepKindProperty);
            set => SetValue(StepKindProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StepKind"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StepKindProperty =
            DependencyProperty.Register(nameof(StepKind), typeof(StepControlKind), typeof(StepBar), new PropertyMetadata(StepControlKind.Number));


        /// <summary>
        /// <see cref="StepKind"/>值为<see cref="StepControlKind.Number"/>时的步骤索引
        /// </summary>
        public int StepIndex
        {
            get => (int)GetValue(StepIndexProperty);
            set => SetValue(StepIndexProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StepIndex"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StepIndexProperty =
            DependencyProperty.Register(nameof(StepIndex), typeof(int), typeof(StepBar),
                new FrameworkPropertyMetadata(ValueBoxes.Integer0Box, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStepIndexChanged, CoerceStepIndex));

        private static object CoerceStepIndex(DependencyObject d, object basevalue)
        {
            var ctl = (StepBar)d;
            var stepIndex = (int)basevalue;
            if (ctl.Items.Count == 0 && stepIndex > 0)
            {
                ctl._originStepIndex = stepIndex;
                return ValueBoxes.Integer0Box;
            }

            return stepIndex < 0 ? ValueBoxes.Integer0Box : stepIndex >= ctl.Items.Count
                                 ? ctl.Items.Count == 0 ? 0 : ctl.Items.Count - 1 : basevalue;
        }

        private static void OnStepIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = d as StepBar;
            ctl?.OnStepIndexChanged((int)e.OldValue, (int)e.NewValue);
        }

        private void OnStepIndexChanged(int oldStepIndex, int newStepIndex)
        {
            if (oldStepIndex == newStepIndex) return;

            for (var i = 0; i < newStepIndex; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is StepBarItem stepBarItem)
                {
                    stepBarItem.Status = StepStatus.Finish;
                }
            }

            for (var i = newStepIndex + 1; i < Items.Count; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is StepBarItem stepBarItem)
                {
                    stepBarItem.Status = StepStatus.Waiting;
                }
            }

            if (ItemContainerGenerator.ContainerFromIndex(newStepIndex) is StepBarItem stepItemSelected)
                stepItemSelected.Status = StepStatus.Running;
            _progressBar?.BeginAnimation(RangeBase.ValueProperty, CookPopularToolkit.Windows.AnimationHelper.CreateDoubleAnimation(newStepIndex));

            OnStepChanged(oldStepIndex, newStepIndex);
        }


        /// <summary>
        /// <see cref="StepKind"/>值为<see cref="StepControlKind.IconPattern"/>时的图标类型数据
        /// </summary>
        public IconPatternKind StepIconPattern
        {
            get => (IconPatternKind)GetValue(StepIconPatternProperty);
            set => SetValue(StepIconPatternProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StepIconPattern"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StepIconPatternProperty =
            DependencyProperty.Register(nameof(StepIconPattern), typeof(IconPatternKind), typeof(StepBar), new PropertyMetadata(IconPatternKind.Smile));


        /// <summary>
        /// <see cref="StepKind"/>值为<see cref="StepControlKind.CustomIcon"/>时的自定义内容
        /// </summary>
        public Geometry StepCustomIcon
        {
            get => (Geometry)GetValue(StepCustomIconProperty);
            set => SetValue(StepCustomIconProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StepCustomIcon"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StepCustomIconProperty =
            DependencyProperty.Register(nameof(StepCustomIcon), typeof(Geometry), typeof(StepBar), new PropertyMetadata(Geometry.Empty));


        /// <summary>
        /// <see cref="StepKind"/>值为<see cref="StepControlKind.Image"/>时的图片资源
        /// </summary>
        public ImageSource StepImageSource
        {
            get => (ImageSource)GetValue(StepImageSourceProperty);
            set => SetValue(StepImageSourceProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StepImageSource"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StepImageSourceProperty =
            DependencyProperty.Register(nameof(StepImageSource), typeof(ImageSource), typeof(StepBar), new PropertyMetadata(default(ImageSource)));


        /// <summary>
        /// 是否可点击选定步骤
        /// </summary>
        public bool IsClickSelectable
        {
            get => (bool)GetValue(IsClickSelectableProperty);
            set => SetValue(IsClickSelectableProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsClickSelectable"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsClickSelectableProperty =
            DependencyProperty.Register(nameof(IsClickSelectable), typeof(bool), typeof(StepBar), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 步骤改变事件
        /// </summary>
        [Category("Behavior")]
        [Description("步骤改变事件")]
        public event RoutedPropertyChangedEventHandler<int> StepChanged
        {
            add => this.AddHandler(StepChangedEvent, value);
            remove => this.RemoveHandler(StepChangedEvent, value);
        }
        public static readonly RoutedEvent StepChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(StepChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(StepBar));

        protected virtual void OnStepChanged(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> arg = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, StepChangedEvent);
            this.RaiseEvent(arg);
        }


        public StepBar()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.NextCommand, (s, e) => Next()));
            CommandBindings.Add(new CommandBinding(ControlCommands.PreviousCommand, (s, e) => Previous()));

            ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            AddHandler(SelectableItem.SelectedEvent, new RoutedEventHandler(OnStepBarItemSelected));
        }

        private void ItemContainerGenerator_StatusChanged(object? sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                var count = Items.Count;
                if (count <= 0) return;

                for (var i = 0; i < count; i++)
                {
                    if (ItemContainerGenerator.ContainerFromIndex(i) is StepBarItem stepBarItem)
                    {
                        stepBarItem.Index = i + 1;
                    }
                }

                if (_originStepIndex > 0)
                {
                    StepIndex = _originStepIndex;
                    _originStepIndex = -1;
                }
                else
                {
                    OnStepIndexChanged(_originStepIndex, StepIndex);
                }
            }
        }

        private void OnStepBarItemSelected(object sender, RoutedEventArgs e)
        {
            if (!IsClickSelectable) return;

            if (e.OriginalSource is StepBarItem item)
            {
                SetCurrentValue(StepIndexProperty, item.Index - 1);
            }
        }

        public void Next() => StepIndex++;

        public void Previous() => StepIndex--;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _progressBar = (ProgressBar)GetTemplateChild(ElementProgressBar);
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is StepBarItem;

        protected override DependencyObject GetContainerForItemOverride() => new StepBarItem();

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var itemsCount = Items.Count;
            if (_progressBar == null || itemsCount <= 0) return;
            _progressBar.Maximum = itemsCount - 1;
            _progressBar.Value = StepIndex;


            var stepBarItems = new List<StepBarItem>();
            foreach (var item in ItemContainerGenerator.Items)
            {
                if (IsItemItsOwnContainerOverride(item))
                    stepBarItems.Add((StepBarItem)item);
                else
                    stepBarItems.Add((StepBarItem)ItemContainerGenerator.ContainerFromItem(item));
            }

            var maxControlSize = stepBarItems.Max(t => t.ControlSize);
            stepBarItems.ForEach(item => { item.ControlSize = maxControlSize; });
            switch (Dock)
            {
                case Dock.Left:
                case Dock.Right:
                    _progressBar.Margin = new Thickness((maxControlSize - _progressBar.Width) / 2D, 0, 0, 0);
                    break;
                case Dock.Top:
                    _progressBar.Margin = new Thickness(0, (maxControlSize - _progressBar.Height) / 2D, 0, 0);
                    break;
                case Dock.Bottom:
                    _progressBar.Margin = new Thickness(0, 0, 0, (maxControlSize - _progressBar.Height) / 2D);
                    break;
                default:
                    break;
            }

            if (Dock == Dock.Top || Dock == Dock.Bottom)
            {
                _progressBar.Width = (itemsCount - 1) * (ActualWidth / itemsCount);
            }
            else
            {
                _progressBar.Height = (itemsCount - 1) * (ActualHeight / itemsCount);
            }
        }
    }
}
