/*
 *Description: DragDropPanel
 *Author: Chance.zheng
 *Creat Time: 2024/1/5 17:20:49
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// 拖拽面板
    /// </summary>
    public class DragDropPanel : DragDropPanelBase
    {
        /// <summary>
        /// The template part name for the maxmize toggle button.
        /// </summary>
        private const string ElementMaximizeToggleButton = "MaximizeToggleButton";

        /// <summary>
        /// Panel maximised flag.
        /// </summary>
        private PanelState panelState = PanelState.Restored;

        /// <summary>
        /// Stores the panel index.
        /// </summary>
        private int panelIndex = 0;


        /// <summary>
        /// The maxmised event.
        /// </summary>
        public event EventHandler Maximized;

        /// <summary>
        /// The restored event.
        /// </summary>
        public event EventHandler Restored;

        /// <summary>
        /// The minimized event.
        /// </summary>
        public event EventHandler Minimized;

        /// <summary>
        /// Gets or sets the calculated panel index.
        /// </summary>
        public int PanelIndex
        {
            get { return panelIndex; }
            set { panelIndex = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the panel is maximised.
        /// </summary>
        [System.ComponentModel.Category("Panel Properties"), System.ComponentModel.Description("Gets whether the panel is maximised.")]
        public PanelState PanelState
        {
            get => panelState;

            set
            {
                panelState = value;

                switch (panelState)
                {
                    case PanelState.Restored:
                        Restore();
                        break;
                    case PanelState.Maximized:
                        Maximize();
                        break;
                    case PanelState.Minimized:
                        Minimize();
                        break;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the panel is maximised.
        /// </summary>
        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            private set { SetValue(IsMaximizedProperty, value); }
        }
        public static readonly DependencyProperty IsMaximizedProperty =
            DependencyProperty.Register(nameof(IsMaximized), typeof(bool), typeof(DragDropPanel), new PropertyMetadata(false));


        public DragDropPanel()
        {
            DefaultStyleKey = typeof(DragDropPanel);
        }

        /// <summary>
        /// Gets called once the template is applied so we can fish out the bits
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ToggleButton? maximizeToggle = GetTemplateChild(ElementMaximizeToggleButton) as ToggleButton;

            if (maximizeToggle != null)
            {
                maximizeToggle.Click += (s, e) =>
                {
                    if (IsMaximized)
                        Maximize();
                    else
                        Restore();
                };
            }
        }

        /// <summary>
        /// Override for updating the panel position.
        /// </summary>
        /// <param name="pos">The new position.</param>
        public override void UpdatePosition(Point pos)
        {
            Canvas.SetLeft(this, pos.X);
            Canvas.SetTop(this, pos.Y);
        }

        /// <summary>
        /// Override for when a panel is maximized.
        /// </summary>
        public virtual void Maximize()
        {
            // Bring the panel to the front
            Panel.SetZIndex(this, CurrentZIndex++);

            bool raiseEvent = panelState != PanelState.Maximized;
            panelState = PanelState.Maximized;

            IsMaximized = true;

            // Fire the panel maximized event
            if (raiseEvent && Maximized != null)
            {
                Maximized(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Override for when the panel minimizes.
        /// </summary>
        public virtual void Minimize()
        {
            bool raiseEvent = panelState != PanelState.Minimized;
            panelState = PanelState.Minimized;

            IsMaximized = false;

            // Fire the panel minimized event
            if (raiseEvent && Minimized != null)
            {
                Minimized(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Override for when the panel restores.
        /// </summary>
        public virtual void Restore()
        {
            bool raiseEvent = panelState != PanelState.Restored;
            panelState = PanelState.Restored;

            IsMaximized = false;

            // Fire the panel minimized event
            if (raiseEvent && Restored != null)
            {
                Restored(this, EventArgs.Empty);
            }
        }
    }
}
