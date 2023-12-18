/*
 * Description：PropertyGrid 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-05 17:06:28
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /***
     * 可参考Winform的PorpertyGrid
     * */

    /// <summary>
    /// 属性表格
    /// </summary>
    [TemplatePart(Name = PartItemsControl, Type = typeof(ItemsControl))]
    [TemplatePart(Name = PartSearchBar, Type = typeof(SearchBar))]
    public class PropertyGrid : Control
    {
        private const string PartItemsControl = "PART_ItemsControl";
        private const string PartSearchBar = "PART_SearchBar";
        private const string CategoryMiscellaneous = "杂项";
        private static readonly Dictionary<Type, PropertyItemEditorType> PropertyItemEditorTypeDic = new()
        {
            [typeof(string)] = PropertyItemEditorType.Text,
            [typeof(sbyte)] = PropertyItemEditorType.Number,
            [typeof(byte)] = PropertyItemEditorType.Number,
            [typeof(short)] = PropertyItemEditorType.Number,
            [typeof(ushort)] = PropertyItemEditorType.Number,
            [typeof(int)] = PropertyItemEditorType.Number,
            [typeof(uint)] = PropertyItemEditorType.Number,
            [typeof(long)] = PropertyItemEditorType.Number,
            [typeof(ulong)] = PropertyItemEditorType.Number,
            [typeof(float)] = PropertyItemEditorType.Number,
            [typeof(double)] = PropertyItemEditorType.Number,
            [typeof(bool)] = PropertyItemEditorType.SwitchBar,
            [typeof(bool?)] = PropertyItemEditorType.CheckBox,
            [typeof(object)] = PropertyItemEditorType.Button,
            [typeof(DateTime)] = PropertyItemEditorType.DateTime,
            [typeof(ImageSource)] = PropertyItemEditorType.Image,
            [typeof(HorizontalAlignment)] = PropertyItemEditorType.HorizontalAlignment,
            [typeof(VerticalAlignment)] = PropertyItemEditorType.VerticalAlignment,
        };

        private ItemsControl _itemsControl;
        private SearchBar _searchBar;
        private ICollectionView _dataView;

        private int _selectedIndex = 0; //针对ComboBox
        private bool _isReadOnly = false; //针对TextBox
        private (double, double) _numberRange = new(double.MinValue, double.MaxValue); //针对NumericUpDown


        #region Dependency Properties

        /// <summary>
        /// 当前选中的项
        /// </summary>
        public object SelectedObject
        {
            get => (object)GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }
        /// <summary>
        /// 提供<see cref="SelectedObject"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register(nameof(SelectedObject), typeof(object), typeof(PropertyGrid), new PropertyMetadata(null, OnSelectedObjectChanged));

        private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PropertyGrid pg)
            {
                pg.UpdateItems(e.NewValue);
                pg.OnSelectedObjectChanged(e.OldValue, e.NewValue);
            }
        }


        [Description("SelectedObject属性更改时发生")]
        public event RoutedPropertyChangedEventHandler<object> SelectedObjectChanged
        {
            add { this.AddHandler(SelectedObjectChangedEvent, value); }
            remove { this.RemoveHandler(SelectedObjectChangedEvent, value); }
        }
        public static readonly RoutedEvent SelectedObjectChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SelectedObjectChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(PropertyGrid));

        protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedObjectChangedEvent);
            this.RaiseEvent(arg);
        }


        /// <summary>
        /// 选中项的标题
        /// </summary>
        public string PropertyTitle
        {
            get => (string)GetValue(PropertyTitleProperty);
            set => SetValue(PropertyTitleProperty, value);
        }
        /// <summary>
        /// 提供<see cref="PropertyTitle"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PropertyTitleProperty =
            DependencyProperty.Register(nameof(PropertyTitle), typeof(string), typeof(PropertyGrid), new PropertyMetadata(string.Empty));


        /// <summary>
        /// 选中项的描述
        /// </summary>
        public string PropertyDescription
        {
            get => (string)GetValue(PropertyDescriptionProperty);
            set => SetValue(PropertyDescriptionProperty, value);
        }
        /// <summary>
        /// 提供<see cref="PropertyDescription"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PropertyDescriptionProperty =
            DependencyProperty.Register(nameof(PropertyDescription), typeof(string), typeof(PropertyGrid), new PropertyMetadata(string.Empty));


        /// <summary>
        /// 属性标题默认宽度
        /// </summary>
        public double PropertyTitleWidth
        {
            get => (double)GetValue(PropertyTitleWidthProperty);
            set => SetValue(PropertyTitleWidthProperty, value);
        }
        /// <summary>
        /// 提供<see cref="PropertyTitleWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PropertyTitleWidthProperty =
            DependencyProperty.Register(nameof(PropertyTitleWidth), typeof(double), typeof(PropertyGrid), new PropertyMetadata(CookPopularToolkit.ValueBoxes.Double0Box));

        #endregion


        public PropertyGrid()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.SortByCategoryCommand, SortByCategory, (s, e) => e.CanExecute = true));
            CommandBindings.Add(new CommandBinding(ControlCommands.SortByNameCommand, SortByName, (s, e) => e.CanExecute = true));

            //Themes.ThemeProvider.ThemeChanged += (s, e) =>
            //{
            //    var propertyDescriptors = TypeDescriptor.GetProperties(SelectedObject.GetType())
            //    .OfType<PropertyDescriptor>()
            //    .Where(p => p.PropertyType == typeof(bool));
            //    foreach (var propertyDescriptor in propertyDescriptors)
            //    {
            //        var switchButton = _propertySwitchEditor.GetElement(CreatePropertyItem(propertyDescriptor)) as SwitchButton;
            //        switchButton.SwitchOpenBackground = e.ThemeDictionary["ControlPressBrush"] as Brush;
            //    }
            //};
        }

        private void SortByName(object sender, ExecutedRoutedEventArgs e)
        {
            if (_dataView == null) return;

            using (_dataView.DeferRefresh())
            {
                _dataView.GroupDescriptions.Clear();
                _dataView.SortDescriptions.Clear();
                _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.PropertyNameProperty.Name, ListSortDirection.Ascending));
            }
        }

        private void SortByCategory(object? sender, ExecutedRoutedEventArgs? e)
        {
            if (_dataView == null) return;

            using (_dataView.DeferRefresh())
            {
                _dataView.GroupDescriptions.Clear();
                _dataView.SortDescriptions.Clear();
                _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.CategoryProperty.Name, ListSortDirection.Ascending));
                _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.DisplayNameProperty.Name, ListSortDirection.Ascending));
                _dataView.GroupDescriptions.Add(new PropertyGroupDescription(PropertyItem.CategoryProperty.Name));
            }
        }

        public override void OnApplyTemplate()
        {
            if (_searchBar != null)
            {
                _searchBar.StartSearch -= SearchBar_StartSearch;
            }

            base.OnApplyTemplate();

            _itemsControl = (ItemsControl)GetTemplateChild(PartItemsControl);
            _searchBar = (SearchBar)GetTemplateChild(PartSearchBar);

            if (_searchBar != null)
            {
                _searchBar.StartSearch += SearchBar_StartSearch;
            }

            UpdateItems(SelectedObject);
        }

        private void SearchBar_StartSearch(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (_dataView == null) return;

            string searchContent = e.NewValue;
            if (string.IsNullOrEmpty(searchContent))
            {
                foreach (UIElement? item in _dataView)
                {
                    item?.ToVisible();
                }
            }
            else
            {
                foreach (PropertyItem? item in _dataView)
                {
                    var isVisible = item!.PropertyName.ToLower().Contains(searchContent) || item.DisplayName.ToLower().Contains(searchContent);
                    if (isVisible)
                        item.ToVisible();
                    else
                        item.ToCollapse();
                }
            }
        }

        private void UpdateItems(object obj)
        {
            if (obj == null || _itemsControl == null) return;

            _dataView = (ICollectionView)CollectionViewSource.GetDefaultView(TypeDescriptor.GetProperties(obj.GetType())
                                                             .OfType<PropertyDescriptor>()
                                                             .Where(CheckIsBrowsable)
                                                             .Select(CreatePropertyItem))
                                                             .ForEach(InitElement);

            SortByCategory(null, null);
            _itemsControl.ItemsSource = _dataView;
        }

        protected virtual PropertyItem CreatePropertyItem(PropertyDescriptor propertyDescriptor)
        {
            InitPropertyItemValue(propertyDescriptor);

            return new PropertyItem
            {
                IsBrowsable = CheckIsBrowsable(propertyDescriptor),
                IsReadOnly = CheckIsReadOnly(propertyDescriptor),
                Category = GetPropertyCategory(propertyDescriptor),
                Description = GetPropertyDescription(propertyDescriptor),
                DisplayName = GetPropertyDisplayName(propertyDescriptor),
                Value = GetPropertyItemValue(propertyDescriptor),
                DefaultValue = GetElementDefaultValue(propertyDescriptor),
                PropertySource = SelectedObject,
                EditorFactory = GetEditorElementFactory(propertyDescriptor),
                PropertyType = propertyDescriptor.PropertyType,
                PropertyName = propertyDescriptor.Name,
                PropertyTypeName = $"{propertyDescriptor.PropertyType.Namespace}.{propertyDescriptor.PropertyType.Name}",
            };
        }

        protected virtual void InitElement(object item)
        {
            if (item == null) return;
            var propertyItem = (PropertyItem)item;

            if (propertyItem.EditorFactory == null) return;
            propertyItem.Element = propertyItem.EditorFactory.GetElement(propertyItem);
            propertyItem.EditorFactory.SetBinding(propertyItem.Element, propertyItem);
        }

        #region Init PropertyItem DependencyProperties

        private void InitPropertyItemValue(PropertyDescriptor propertyDescriptor)
        {
            var isEditor = PropertyItemEditorTypeDic.TryGetValue(propertyDescriptor.PropertyType, out var editorType);
            if (isEditor)
            {
                switch (editorType)
                {
                    case PropertyItemEditorType.Text:
                        var editableAttribute = propertyDescriptor.Attributes.OfType<EditableAttribute>().FirstOrDefault();
                        if (editableAttribute != null)
                        {
                            _isReadOnly = !editableAttribute.AllowEdit;
                        }
                        break;
                    case PropertyItemEditorType.Number:
                        var numberAttribute = propertyDescriptor.Attributes.OfType<RangeAttribute>().FirstOrDefault();
                        if (numberAttribute != null)
                        {
                            _numberRange.Item1 = double.Parse(numberAttribute.Minimum.ToString()!);
                            _numberRange.Item2 = double.Parse(numberAttribute.Maximum.ToString()!);
                        }
                        break;
                    case PropertyItemEditorType.SwitchBar:
                        break;
                    case PropertyItemEditorType.DateTime:
                        break;
                    case PropertyItemEditorType.Button:
                        break;
                    case PropertyItemEditorType.CheckBox:
                        break;
                    case PropertyItemEditorType.Image:
                        break;
                    case PropertyItemEditorType.HorizontalAlignment:
                        break;
                    case PropertyItemEditorType.VerticalAlignment:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if ((propertyDescriptor.PropertyType.IsSubclassOf(typeof(Enum)) || typeof(IEnumerable).IsAssignableFrom(propertyDescriptor.PropertyType)))
                {
                    var indexAttribute = propertyDescriptor.Attributes.OfType<IndexAttribute>().FirstOrDefault();
                    if (indexAttribute != null)
                    {
                        _selectedIndex = indexAttribute.SelectedIndex;
                    }
                }
                else
                {

                }
            }
        }

        private bool CheckIsBrowsable(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsBrowsable;

        private bool CheckIsReadOnly(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsReadOnly;

        private string GetPropertyCategory(PropertyDescriptor propertyDescriptor)
        {
            var categoryAttribute = propertyDescriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault();

            return categoryAttribute == null ? CategoryMiscellaneous : string.IsNullOrEmpty(categoryAttribute.Category) ? CategoryMiscellaneous : categoryAttribute.Category;
        }

        private string GetPropertyDescription(PropertyDescriptor propertyDescriptor) => propertyDescriptor.Description;

        private string GetPropertyDisplayName(PropertyDescriptor propertyDescriptor)
        {
            var displayName = propertyDescriptor.DisplayName;
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = propertyDescriptor.Name;
            }

            return displayName;
        }

        private object GetPropertyItemValue(PropertyDescriptor propertyDescriptor)
        {
            //var model = Activator.CreateInstance(propertyDescriptor.ComponentType);
            object? value = propertyDescriptor.GetValue(SelectedObject);

            return value;
        }

        private object? GetElementDefaultValue(PropertyDescriptor propertyDescriptor)
        {
            var defaultValueAttribute = propertyDescriptor.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
            return defaultValueAttribute?.Value;
        }

        private PropertyItemEditorFactory GetEditorElementFactory(PropertyDescriptor propertyDescriptor)
        {
            var editorAttribute = propertyDescriptor.Attributes.OfType<EditorAttribute>().FirstOrDefault();
            if (editorAttribute == null || string.IsNullOrEmpty(editorAttribute.EditorTypeName))
            {
                return CreateDefaultEditor(propertyDescriptor.PropertyType);
            }
            else
            {
                var editType = Type.GetType(editorAttribute.EditorTypeName);
                return editType == null ? CreateDefaultEditor(propertyDescriptor.PropertyType) : CreateCustomEditor(editType);
            }
        }

        private PropertyItemEditorFactory CreateDefaultEditor(Type type)
        {
            var isEditor = PropertyItemEditorTypeDic.TryGetValue(type, out var editorType);
            if (isEditor)
            {
                return editorType switch
                {
                    PropertyItemEditorType.Text => new PropertyTextEditor(_isReadOnly),
                    PropertyItemEditorType.Number => new PropertyNumberEditor(_numberRange.Item1, _numberRange.Item2),
                    PropertyItemEditorType.SwitchBar => new PropertySwitchEditor(),
                    PropertyItemEditorType.DateTime => new PropertyTimerPickerEditor(),
                    PropertyItemEditorType.Button => new PropertyButtonEditor(),
                    PropertyItemEditorType.CheckBox => new PropertyCheckBoxEditor(),
                    PropertyItemEditorType.Image => new PropertyImageEditor(),
                    PropertyItemEditorType.HorizontalAlignment => new PropertyComboBoxEditor(),
                    PropertyItemEditorType.VerticalAlignment => new PropertyComboBoxEditor(),
                    _ => new PropertyTextEditor(true)
                };
            }
            else
            {
                return (type.IsSubclassOf(typeof(Enum)) || typeof(IEnumerable).IsAssignableFrom(type)) ? new PropertyComboBoxEditor(_selectedIndex) : new PropertyTextEditor(true);
            }
        }

        private PropertyItemEditorFactory CreateCustomEditor(Type type) => Activator.CreateInstance(type) as PropertyItemEditorFactory ?? new PropertyTextEditor(true);

        #endregion
    }
}
