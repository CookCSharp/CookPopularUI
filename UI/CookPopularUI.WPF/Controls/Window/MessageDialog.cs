/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-19 16:23:14
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CookPopularUI.WPF.Windows
{
    /// <summary>
    /// 模态消息框
    /// </summary>
    /// <remarks>与系统控件<see cref="MessageBox"/>类似</remarks>
    [TemplatePart(Name = ButtonsPanel, Type = typeof(Panel))]
    public class MessageDialog : NormalWindow
    {
        private const string ButtonsPanel = "PART_Panel";

        private static MessageDialog CurrentMessageDialog;
        private MessageBoxButton currentMessageBoxButton;
        private MessageBoxResult currentMessageBoxResult;
        private Panel currentPanel;
        private IntPtr _lastActiveWindowIntPtr;


        /// <summary>
        /// <see cref="MessageDialog"/>消息内容
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(MessageDialog), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="MessageDialog"/>图标
        /// </summary>
        public Geometry Image
        {
            get { return (Geometry)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(nameof(Image), typeof(Geometry), typeof(MessageDialog), new PropertyMetadata(Geometry.Empty));

        /// <summary>
        /// <see cref="MessageDialog"/>图标颜色
        /// </summary>
        public Brush ImageBrush
        {
            get { return (Brush)GetValue(ImageBrushProperty); }
            set { SetValue(ImageBrushProperty, value); }
        }
        public static readonly DependencyProperty ImageBrushProperty =
            DependencyProperty.Register(nameof(ImageBrush), typeof(Brush), typeof(MessageDialog), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 是否显示<see cref="MessageDialog"/>信息的类型图标
        /// </summary>
        public bool IsShowImage
        {
            get { return (bool)GetValue(IsShowImageProperty); }
            set { SetValue(IsShowImageProperty, value); }
        }
        public static readonly DependencyProperty IsShowImageProperty =
            DependencyProperty.Register(nameof(IsShowImage), typeof(bool), typeof(MessageDialog), new PropertyMetadata(CookPopularToolkit.ValueBoxes.TrueBox));


        static MessageDialog()
        {
            StyleProperty.OverrideMetadata(typeof(MessageDialog), new FrameworkPropertyMetadata(ResourceHelper.GetResource<Style>("MessageDialogStyle")));
        }

        protected MessageDialog()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.ConfirmCommand, Executed, (s, e) => e.CanExecute = true));
            CommandBindings.Add(new CommandBinding(ControlCommands.YesCommand, Executed, (s, e) => e.CanExecute = true));
            CommandBindings.Add(new CommandBinding(ControlCommands.NoCommand, Executed, (s, e) => e.CanExecute = true));
            CommandBindings.Add(new CommandBinding(ControlCommands.CancelCommand, Executed, (s, e) => e.CanExecute = true));
            InputMethod.SetIsInputMethodEnabled(this, false); //屏蔽输入法
        }

        private static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = (MessageDialog)sender;

            if (e.Command == ControlCommands.ConfirmCommand)
                dialog.currentMessageBoxResult = MessageBoxResult.OK;
            else if (e.Command == ControlCommands.YesCommand)
                dialog.currentMessageBoxResult = MessageBoxResult.Yes;
            else if (e.Command == ControlCommands.NoCommand)
                dialog.currentMessageBoxResult = MessageBoxResult.No;
            else if (e.Command == ControlCommands.CancelCommand)
                dialog.currentMessageBoxResult = MessageBoxResult.Cancel;

            dialog.Close();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            var hMenu = InteropMethods.GetSystemMenu(this.EnsureHandle(), false);
            if (hMenu != IntPtr.Zero)
            {
                InteropMethods.EnableMenuItem(hMenu, InteropValues.SC_CLOSE, InteropValues.MF_BYCOMMAND | InteropValues.MF_GRAYED);
            }

            base.OnSourceInitialized(e);

            CurrentMessageDialog._lastActiveWindowIntPtr = InteropMethods.GetForegroundWindow();
            Activate();
        }

        protected override void OnClosed(EventArgs e)
        {
            InteropMethods.SetForegroundWindow(CurrentMessageDialog._lastActiveWindowIntPtr);

            base.OnClosed(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                var builder = new StringBuilder();
                var line = new string('-', 27);
                builder.Append(line);
                builder.Append(Environment.NewLine);
                builder.Append(Title);
                builder.Append(Environment.NewLine);
                builder.Append(line);
                builder.Append(Environment.NewLine);
                builder.Append(Message);
                builder.Append(Environment.NewLine);
                builder.Append(line);
                builder.Append(Environment.NewLine);
                try
                {
                    Clipboard.SetDataObject(builder.ToString());
                }
                catch
                {

                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CurrentMessageDialog.currentPanel = (Panel)GetTemplateChild(ButtonsPanel);
            if (CurrentMessageDialog.currentPanel != null)
            {
                foreach (var btn in CreateButton(CurrentMessageDialog.currentMessageBoxButton))
                {
                    CurrentMessageDialog.currentPanel.Children.Add(btn);
                }
            }
        }

        public static MessageBoxResult ShowSuccess(string messageBoxText, string? caption = default)
        {
            DispatcherHelper.AppInvoke(() =>
            {
                CurrentMessageDialog = CreateMessageDialog(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
                CurrentMessageDialog.currentMessageBoxButton = MessageBoxButton.OK;
                CurrentMessageDialog.Image = ResourceHelper.GetResource<Geometry>("SmileGeometry")!;
                CurrentMessageDialog.ImageBrush = ResourceHelper.GetResource<Brush>("MessageDialogSuccessBrush")!;
                SystemSounds.Beep.Play();
                CurrentMessageDialog.ShowDialog();
            });

            return CurrentMessageDialog.currentMessageBoxResult;
        }

        public static MessageBoxResult ShowInfo(string messageBoxText, string? caption = default)
        {
            DispatcherHelper.AppInvoke(() =>
            {
                CurrentMessageDialog = CreateMessageDialog(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                CurrentMessageDialog.currentMessageBoxButton = MessageBoxButton.OK;
                CreateImage(MessageBoxImage.Information);
                SystemSounds.Asterisk.Play();
                CurrentMessageDialog.ShowDialog();
            });

            return CurrentMessageDialog.currentMessageBoxResult;
        }

        public static MessageBoxResult ShowWarning(string messageBoxText, string? caption = default)
        {
            DispatcherHelper.AppInvoke(() =>
            {
                CurrentMessageDialog = CreateMessageDialog(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                CurrentMessageDialog.currentMessageBoxButton = MessageBoxButton.OK;
                CreateImage(MessageBoxImage.Warning);
                SystemSounds.Exclamation.Play();
                CurrentMessageDialog.ShowDialog();
            });

            return CurrentMessageDialog.currentMessageBoxResult;
        }

        public static MessageBoxResult ShowError(string messageBoxText, string? caption = default)
        {
            DispatcherHelper.AppInvoke(() =>
            {
                CurrentMessageDialog = CreateMessageDialog(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                CurrentMessageDialog.currentMessageBoxButton = MessageBoxButton.OK;
                CreateImage(MessageBoxImage.Error);
                SystemSounds.Hand.Play();
                CurrentMessageDialog.ShowDialog();
            });

            return CurrentMessageDialog.currentMessageBoxResult;
        }

        public static MessageBoxResult ShowFatal(string messageBoxText, string? caption = default)
        {
            DispatcherHelper.AppInvoke(() =>
            {
                CurrentMessageDialog = CreateMessageDialog(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                CurrentMessageDialog.currentMessageBoxButton = MessageBoxButton.OK;
                CurrentMessageDialog.Image = ResourceHelper.GetResource<Geometry>("FatalGeometry")!;
                CurrentMessageDialog.ImageBrush = ResourceHelper.GetResource<Brush>("MessageDialogFatalBrush")!;
                SystemSounds.Hand.Play();
                CurrentMessageDialog.ShowDialog();
            });

            return CurrentMessageDialog.currentMessageBoxResult;
        }

        public static MessageBoxResult ShowQuestion(string messageBoxText, string? caption = default)
        {
            DispatcherHelper.AppInvoke(() =>
            {
                CurrentMessageDialog = CreateMessageDialog(null, messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.None);
                CurrentMessageDialog.currentMessageBoxButton = MessageBoxButton.OKCancel;
                CreateImage(MessageBoxImage.Question);
                SystemSounds.Question.Play();
                CurrentMessageDialog.ShowDialog();
            });

            return CurrentMessageDialog.currentMessageBoxResult;
        }

        /// <summary>
        /// 自定义展示信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(CustomMessageDialog info)
        {
            DispatcherHelper.AppInvoke(() =>
            {
                CurrentMessageDialog = CreateMessageDialog(null, info.Message, info.Caption, info.Button, MessageBoxImage.None, info.DefaultResult, info.DefaultOptions);
                CurrentMessageDialog.currentMessageBoxButton = info.Button;

                if (!string.IsNullOrEmpty(info.IconKey))
                {
                    CurrentMessageDialog.Image = ResourceHelper.GetResource<Geometry>(info.IconKey) ?? Geometry.Empty;
                    CurrentMessageDialog.ImageBrush = ResourceHelper.GetResource<Brush>(info.IconBrushKey) ?? Brushes.Transparent;
                }

                if (info.StyleKey != null)
                {
                    CurrentMessageDialog.Style = ResourceHelper.GetResource<Style>(info.StyleKey);
                }
                SystemSounds.Asterisk.Play();
                CurrentMessageDialog.ShowDialog();
            });

            return CurrentMessageDialog.currentMessageBoxResult;
        }

        /// <summary>
        /// 自定义展示信息
        /// </summary>
        public static MessageBoxResult Show(string messageBoxText, string? caption = null,
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage image = MessageBoxImage.None,
                                            MessageBoxResult result = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None)
            => Show(null, messageBoxText, caption, button, image, result, options);

        /// <summary>
        /// 自定义展示信息
        /// </summary>
        public static MessageBoxResult Show(Window? owner, string messageBoxText, string? caption = default,
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage image = MessageBoxImage.None,
                                            MessageBoxResult result = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            DispatcherHelper.AppInvoke(() =>
            {
                CurrentMessageDialog = CreateMessageDialog(owner, messageBoxText, caption, button, image, result, options);
                CurrentMessageDialog.currentMessageBoxButton = button;
                CreateImage(image);
                SystemSounds.Beep.Play();
                CurrentMessageDialog.ShowDialog();
            });

            return CurrentMessageDialog.currentMessageBoxResult;
        }

        private static MessageDialog CreateMessageDialog(Window? owner, string messageBoxText, string? caption, MessageBoxButton button, MessageBoxImage image, MessageBoxResult result, MessageBoxOptions options)
        {
            if (!IsValidMessageBoxButton(button))
                throw new InvalidEnumArgumentException(nameof(button), (int)button, typeof(MessageBoxButton));
            if (!IsValidMessageBoxImage(image))
                throw new InvalidEnumArgumentException(nameof(image), (int)image, typeof(MessageBoxImage));
            if (!IsValidMessageBoxResult(result))
                throw new InvalidEnumArgumentException(nameof(result), (int)result, typeof(MessageBoxResult));
            if (!IsValidMessageBoxOptions(options))
                throw new InvalidEnumArgumentException(nameof(options), (int)options, typeof(MessageBoxOptions));

            var ownerWindow = owner is null ? WindowHelper.GetActiveWindowWithWin32Api() : owner;
            var hasWindow = ownerWindow is null;

            return new MessageDialog
            {
                Owner = ownerWindow,
                Message = messageBoxText,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = caption ?? string.Empty,
                currentMessageBoxResult = result,
                Topmost = hasWindow,
            };
        }

        private static List<Button> CreateButton(MessageBoxButton button)
        {
            var listButton = new List<Button>();
            switch (button)
            {
                case MessageBoxButton.OK:
                    var btn_OK_1 = new Button()
                    {
                        IsDefault = true,
                        Content = $"{Properties.Language.Ok}(_O)",
                        Command = ControlCommands.ConfirmCommand,
                        Style = ResourceHelper.GetResource<Style>("MessageDialogSelectedButtonStyle"),
                    };
                    listButton.Add(btn_OK_1);
                    break;
                case MessageBoxButton.OKCancel:
                    var btn_OK_2 = new Button()
                    {
                        IsDefault = true,
                        Content = $"{Properties.Language.Ok}(_O)",
                        Command = ControlCommands.ConfirmCommand,
                        Style = ResourceHelper.GetResource<Style>("MessageDialogSelectedButtonStyle"),
                    };
                    var btn_Cancel_1 = new Button()
                    {
                        IsCancel = true,
                        Content = $"{Properties.Language.Cancel}(_C)",
                        Command = ControlCommands.CancelCommand,
                        Style = ResourceHelper.GetResource<Style>("MessageDialogUnSelectedButtonStyle"),
                    };
                    listButton.Add(btn_OK_2);
                    listButton.Add(btn_Cancel_1);
                    break;
                case MessageBoxButton.YesNo:
                    var btn_Yes_1 = new Button()
                    {
                        IsDefault = true,
                        Content = $"{Properties.Language.Yes}(_Y)",
                        Command = ControlCommands.YesCommand,
                        Style = ResourceHelper.GetResource<Style>("MessageDialogSelectedButtonStyle"),
                    };
                    var btn_No_1 = new Button()
                    {
                        IsCancel = true,
                        Content = $"{Properties.Language.No}(_N)",
                        Command = ControlCommands.NoCommand,
                        Style = ResourceHelper.GetResource<Style>("MessageDialogUnSelectedButtonStyle"),
                    };
                    listButton.Add(btn_Yes_1);
                    listButton.Add(btn_No_1);
                    break;
                case MessageBoxButton.YesNoCancel:
                    var btn_Yes_2 = new Button()
                    {
                        IsDefault = true,
                        Content = $"{Properties.Language.Yes}(_Y)",
                        Command = ControlCommands.YesCommand,
                        Style = ResourceHelper.GetResource<Style>("MessageDialogSelectedButtonStyle"),
                    };
                    var btn_No_2 = new Button()
                    {
                        Content = $"{Properties.Language.No}(_N)",
                        Command = ControlCommands.NoCommand,
                        Style = ResourceHelper.GetResource<Style>("MessageDialogUnSelectedButtonStyle"),
                    };
                    var btn_Cancel_2 = new Button()
                    {
                        IsCancel = true,
                        Content = $"{Properties.Language.Cancel}(_C)",
                        Command = ControlCommands.CancelCommand,
                        Style = ResourceHelper.GetResource<Style>("MessageDialogUnSelectedButtonStyle"),
                    };
                    listButton.Add(btn_Yes_2);
                    listButton.Add(btn_No_2);
                    listButton.Add(btn_Cancel_2);
                    break;
                default:
                    break;
            }

            return listButton;
        }

        private static void CreateImage(MessageBoxImage messageBoxImage)
        {
            string imageKey = string.Empty;
            string imageBrushKey = string.Empty;

            switch (messageBoxImage)
            {
                case MessageBoxImage.Information:
                    imageKey = "InfoGeometry";
                    imageBrushKey = "MessageDialogInfoBrush";
                    break;
                case MessageBoxImage.Warning:
                    imageKey = "WarningGeometry";
                    imageBrushKey = "MessageDialogWarningBrush";
                    break;
                case MessageBoxImage.Error:
                    imageKey = "ErrorGeometry";
                    imageBrushKey = "MessageDialogErrorBrush";
                    break;
                case MessageBoxImage.Question:
                    imageKey = "QuestionGeometry";
                    imageBrushKey = "MessageDialogQuestionBrush";
                    break;
                default:
                    break;
            }

            CurrentMessageDialog.Image = ResourceHelper.GetResource<Geometry>(imageKey)!;
            CurrentMessageDialog.ImageBrush = ResourceHelper.GetResource<Brush>(imageBrushKey)!;
        }

        private static bool IsValidMessageBoxButton(MessageBoxButton button)
        {
            return button == MessageBoxButton.OK ||
                   button == MessageBoxButton.OKCancel ||
                   button == MessageBoxButton.YesNo ||
                   button == MessageBoxButton.YesNoCancel;
        }

        private static bool IsValidMessageBoxImage(MessageBoxImage image)
        {
            return image == MessageBoxImage.Asterisk ||
                   image == MessageBoxImage.Error ||
                   image == MessageBoxImage.Exclamation ||
                   image == MessageBoxImage.Hand ||
                   image == MessageBoxImage.Information ||
                   image == MessageBoxImage.None ||
                   image == MessageBoxImage.Question ||
                   image == MessageBoxImage.Stop ||
                   image == MessageBoxImage.Warning;
        }

        private static bool IsValidMessageBoxResult(MessageBoxResult result)
        {
            return result == MessageBoxResult.Cancel ||
                   result == MessageBoxResult.No ||
                   result == MessageBoxResult.None ||
                   result == MessageBoxResult.OK ||
                   result == MessageBoxResult.Yes;
        }

        private static bool IsValidMessageBoxOptions(MessageBoxOptions options)
        {
            return options == MessageBoxOptions.DefaultDesktopOnly ||
                   options == MessageBoxOptions.None ||
                   options == MessageBoxOptions.RightAlign ||
                   options == MessageBoxOptions.RtlReading ||
                   options == MessageBoxOptions.ServiceNotification;
        }
    }

    /// <summary>
    /// 自定义显示信息
    /// </summary>
    public class CustomMessageDialog
    {
        public string Message { get; set; }

        public string Caption { get; set; }

        public MessageBoxButton Button { get; set; } = MessageBoxButton.OK;

        public string IconKey { get; set; }

        public string IconBrushKey { get; set; }

        public MessageBoxResult DefaultResult { get; set; } = MessageBoxResult.None;

        public MessageBoxOptions DefaultOptions { get; set; } = MessageBoxOptions.None;

        public string StyleKey { get; set; }
    }
}
