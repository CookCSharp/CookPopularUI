/*
 *Description: MessageDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/8/24 18:02:24
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Controls;
using CookPopularUI.WPFDemo.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class MessageDemoViewModel : ViewModelBase
    {
        public DelegateCommand<string> ShowToastMessageCommand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(OnShowToastMessageAction)).Value;
        public DelegateCommand<string> ShowBubbleMessageCommand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(OnShowBubbleMessageAction)).Value;
        public DelegateCommand<string> ShowPopupMessageCommand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(OnShowPopupMessageAction)).Value;


        private void OnShowToastMessageAction(string level)
        {
            if (Enum.TryParse(level, out MessageLevel messageLevel))
            {
                switch (messageLevel)
                {
                    case MessageLevel.Success:
                        ToastMessage.ShowSuccess(level);
                        break;
                    case MessageLevel.Info:
                        ToastMessage.ShowInfo(level);
                        break;
                    case MessageLevel.Warning:
                        ToastMessage.ShowWarning(level);
                        break;
                    case MessageLevel.Error:
                        ToastMessage.ShowError(level);
                        break;
                    case MessageLevel.Fatal:
                        ToastMessage.ShowFatal(level);
                        break;
                    case MessageLevel.Question:
                        ToastMessage.ShowQuestion(level);
                        break;
                    case MessageLevel.Custom:
                        ToastMessage.Show(new NotifyMessageInfo
                        {
                            Content = "Custom",
                            MessageIcon = ResourceHelper.GetResource<Geometry>("SinFuncGeometry"),
                            MessageIconBrush = Brushes.Red,
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnShowBubbleMessageAction(string level)
        {
            if (Enum.TryParse(level, out MessageLevel messageLevel))
            {
                switch (messageLevel)
                {
                    case MessageLevel.Success:
                        BubbleMessage.ShowSuccess(level);
                        break;
                    case MessageLevel.Info:
                        BubbleMessage.ShowInfo(level);
                        break;
                    case MessageLevel.Warning:
                        BubbleMessage.ShowWarning(level);
                        break;
                    case MessageLevel.Error:
                        BubbleMessage.ShowError(level);
                        break;
                    case MessageLevel.Fatal:
                        BubbleMessage.ShowFatal(level);
                        break;
                    case MessageLevel.Question:
                        BubbleMessage.ShowQuestion(level, isSure =>
                        {
                            BubbleMessage.ShowInfo($"Clicked the {isSure}");
                        });
                        break;
                    case MessageLevel.Custom:
                        BubbleMessage.Show(new NotifyMessageInfo
                        {
                            Content = "Custom",
                            MessageIcon = ResourceHelper.GetResource<Geometry>("SinFuncGeometry"),
                            MessageIconBrush = Brushes.Red,
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnShowPopupMessageAction(string name)
        {
            switch (name)
            {
                case "OpenPopupMessageNone":
                    PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftTop, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftCenter, PopupAnimationX.None); 
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftBottom, PopupAnimationX.None); 
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterTop, PopupAnimationX.None); 
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.AllCenter, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterBottom, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightTop, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightCenter, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightBottom, PopupAnimationX.None);
                    break;
                case "OpenPopupMessageFade":
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftTop, PopupAnimationX.Fade);
                    PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftCenter, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftBottom, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterTop, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.AllCenter, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterBottom, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightTop, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightCenter, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightBottom, PopupAnimationX.Fade);
                    break;
                case "OpenPopupMessageLeftHorizontalSlide":
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftTop, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftCenter, PopupAnimationX.LeftHorizontalSlide);
                    PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftBottom, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterTop, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.AllCenter, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterBottom, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightTop, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightCenter, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightBottom, PopupAnimationX.LeftHorizontalSlide);
                    break;
                case "OpenPopupMessageRightHorizontalSlide":
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftTop, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftCenter, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftBottom, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterTop, PopupAnimationX.RightHorizontalSlide);
                    PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.AllCenter, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterBottom, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightTop, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightCenter, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightBottom, PopupAnimationX.RightHorizontalSlide);
                    break;
                case "OpenPopupMessageTopVerticalSlide":
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftTop, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftCenter, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftBottom, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterTop, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.AllCenter, PopupAnimationX.TopVerticalSlide);
                    PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterBottom, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightTop, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightCenter, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightBottom, PopupAnimationX.TopVerticalSlide);
                    break;
                case "OpenPopupMessageBottomVerticalSlide":
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftTop, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftCenter, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftBottom, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterTop, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.AllCenter, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterBottom, PopupAnimationX.BottomVerticalSlide);
                    PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightTop, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightCenter, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightBottom, PopupAnimationX.BottomVerticalSlide);
                    break;
                case "OpenPopupMessageHVSlide":
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftTop, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftCenter, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftBottom, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterTop, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.AllCenter, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterBottom, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightTop, PopupAnimationX.HorizontalVerticalSlide);
                    PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightCenter, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightBottom, PopupAnimationX.HorizontalVerticalSlide);
                    break;
                case "OpenPopupMessageScroll":
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftTop, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftCenter, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.LeftBottom, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterTop, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.AllCenter, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.CenterBottom, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightTop, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightCenter, PopupAnimationX.Scroll);
                    PopupMessage.ShowOpen(new AnimationDemoView(), PopupPosition.RightBottom, PopupAnimationX.Scroll);
                    break;
                default:
                    break;
            }
        }
    }
}
