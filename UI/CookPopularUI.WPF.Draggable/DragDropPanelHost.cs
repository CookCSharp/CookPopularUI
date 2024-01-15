/*
 *Description: DragDropPanelHost
 *Author: Chance.zheng
 *Creat Time: 2024/1/5 17:02:44
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

namespace CookPopularUI.WPF.Draggable
{
    [StyleTypedProperty(Property = "DefaultPanelStyle", StyleTargetType = typeof(DragDropPanel))]
    public class DragDropPanelHost : ItemsControl
    {
        #region Private members

        /// <summary>
        /// A local store of the number of rows
        /// </summary>
        private int rows = 1;

        /// <summary>
        /// A local store of the number of columns
        /// </summary>
        private int columns = 1;

        /// <summary>
        /// Stores the max columns (0 for no maximum). Max rows takes priority over max columns.
        /// </summary>
        private int maxColumns = 0;

        /// <summary>
        /// Stores the max rows (0 for no maximum). Max rows takes priority over max columns.
        /// </summary>
        private int maxRows = 0;

        /// <summary>
        /// The panel currently being dragged
        /// </summary>
        private DragDropPanel? draggingPanel;

        /// <summary>
        /// The currently maxmised panel
        /// </summary>
        private DragDropPanel? maximizedPanel;

        /// <summary>
        /// Stores the minimized column width.
        /// </summary>
        private double minimizedColumnWidth = 250.0;

        /// <summary>
        /// Stores the minimized row height.
        /// </summary>
        private double minimizedRowHeight = 75.0;

        /// <summary>
        /// Stores the dockiing position.
        /// </summary>
        private MinimizedPosition minimizedPosition = MinimizedPosition.Right;

        /// <summary>
        /// Stores the panels in the control.
        /// </summary>
        private List<DragDropPanel> panels = new List<DragDropPanel>();

        #endregion

        /// <summary>
        /// Gets or sets the width for the minimzed column on the right or left side.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the minimized column width.")]
        public double MinimizedColumnWidth
        {
            get => minimizedColumnWidth;
            set
            {
                minimizedColumnWidth = value;
                UpdatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the height for the minimized row on the top or bottom side.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the minimized row height.")]
        public double MinimizedRowHeight
        {
            get => minimizedRowHeight;
            set
            {
                minimizedRowHeight = value;
                UpdatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the docking position.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the minimized panels' position.")]
        public MinimizedPosition MinimizedPosition
        {
            get => minimizedPosition;
            set
            {
                minimizedPosition = value;
                AnimatePanelSizes();
                AnimatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the max rows. 0 for no maximum. Max rows takes priority over max columns.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the maximum rows in the host. Max rows takes priority over max columns.")]
        public int MaxRows
        {
            get => maxRows;
            set
            {
                maxRows = value;
                SetRowsAndColumns(GetOrderedPanels());
                AnimatePanelSizes();
                AnimatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the max columns. 0 for no maximum. Max rows takes priority over max columns.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the maximum columns in the host. Max rows takes priority over max columns.")]
        public int MaxColumns
        {
            get => maxColumns;
            set
            {
                maxColumns = value;
                SetRowsAndColumns(GetOrderedPanels());
                AnimatePanelSizes();
                AnimatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the default panel style.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Sets the default panel style.")]
        public Style DefaultPanelStyle
        {
            get { return (Style)GetValue(DefaultPanelStyleProperty); }
            set { SetValue(DefaultPanelStyleProperty, value); }
        }
        public static readonly DependencyProperty DefaultPanelStyleProperty =
            DependencyProperty.Register(nameof(DefaultPanelStyle), typeof(Style), typeof(DragDropPanelHost), new PropertyMetadata(null));


        public DragDropPanelHost()
        {
            DefaultStyleKey = typeof(DragDropPanelHost);
            SizeChanged += new SizeChangedEventHandler(DragDropPanelHost_SizeChanged);
            LayoutUpdated += new EventHandler(DragDropPanelHost_LayoutUpdated);
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is DragDropPanel;

        protected override DependencyObject GetContainerForItemOverride() => new DragDropPanel();

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var panel = (DragDropPanel)element;

            if (panel.Style == null && DefaultPanelStyle != null)
            {
                panel.Style = DefaultPanelStyle;
            }

            Dictionary<int, DragDropPanel> orderedPanels = GetOrderedPanels();
            orderedPanels.Add(panels.Count, panel);
            panels.Add(panel);
            PreparePanel(panel);
            SetRowsAndColumns(orderedPanels);
            AnimatePanelSizes();
            AnimatePanelLayout();
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            var panel = (DragDropPanel)element;
            UnpreparePanel(panel);
            panels.Remove(panel);
            SetRowsAndColumns(GetOrderedPanels());
            AnimatePanelSizes();
            AnimatePanelLayout();
        }

        #region Panel management methods

        /// <summary>
        /// Prepares a panel for the UI. Override for hooking custom events.
        /// </summary>
        /// <param name="panel">The panel to prepare.</param>
        protected virtual void PreparePanel(DragDropPanel panel)
        {
            // Hook up panel events
            panel.DragStarted += new DragEventHander(DragDropPanel_DragStarted);
            panel.DragFinished += new DragEventHander(DragDropPanel_DragFinished);
            panel.DragMoved += new DragEventHander(DragDropPanel_DragMoved);
            panel.Maximized += new EventHandler(DragDropPanel_Maximized);
            panel.Restored += new EventHandler(DragDropPanel_Restored);

            if (panel.PanelState == PanelState.Maximized)
            {
                maximizedPanel = panel;

                foreach (DragDropPanel DragDropPanel in panels)
                {
                    if (panel != DragDropPanel)
                    {
                        DragDropPanel.Minimize();
                    }
                }
            }
        }

        /// <summary>
        /// Unprepares a panel for the UI. Override for hooking custom events.
        /// </summary>
        /// <param name="panel">The panel to prepare.</param>
        protected virtual void UnpreparePanel(DragDropPanel panel)
        {
            if (panel.PanelState == PanelState.Maximized)
            {
                DragDropPanel_Restored(null, null);
            }

            // Hook up panel events
            panel.DragStarted -= new DragEventHander(DragDropPanel_DragStarted);
            panel.DragFinished -= new DragEventHander(DragDropPanel_DragFinished);
            panel.DragMoved -= new DragEventHander(DragDropPanel_DragMoved);
            panel.Maximized -= new EventHandler(DragDropPanel_Maximized);
            panel.Restored -= new EventHandler(DragDropPanel_Restored);
        }

        #endregion

        #region Panel layout methods

        /// <summary>
        /// Sets the rows and columns on an ordered list of panels.
        /// </summary>
        /// <param name="orderedPanels">The ordered panels.</param>
        private void SetRowsAndColumns(Dictionary<int, DragDropPanel> orderedPanels)
        {
            if (orderedPanels.Count == 0)
            {
                return;
            }

            // Calculate the number of rows and columns required
            rows =
                (int)Math.Floor(Math.Sqrt((double)panels.Count));

            if (maxRows > 0)
            {
                if (rows > maxRows)
                {
                    rows = maxRows;
                }

                columns =
                    (int)Math.Ceiling((double)panels.Count / (double)rows);
            }
            else if (maxColumns > 0)
            {
                columns =
                (int)Math.Ceiling((double)panels.Count / (double)rows);

                if (columns > maxColumns)
                {
                    columns = maxColumns;
                    rows = (int)Math.Ceiling((double)panels.Count / columns);
                }
            }
            else
            {
                columns =
                    (int)Math.Ceiling((double)panels.Count / (double)rows);
            }

            int childCount = 0;

            // Loop through the rows and columns and assign to children
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Grid.SetRow(orderedPanels[childCount], r);
                    Grid.SetColumn(orderedPanels[childCount], c);
                    childCount++;

                    // if we are on the last child, break out of the loop
                    if (childCount == panels.Count)
                    {
                        break;
                    }
                }

                // if we are on the last child, break out of the loop
                if (childCount == panels.Count)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the panels in order.
        /// </summary>
        /// <returns>The ordered panels.</returns>
        private Dictionary<int, DragDropPanel> GetOrderedPanels()
        {
            Dictionary<int, DragDropPanel> orderedPanels = new Dictionary<int, DragDropPanel>();
            List<DragDropPanel> addedPanels = new List<DragDropPanel>();
            for (int i = 0; i < panels.Count; i++)
            {
                DragDropPanel? lowestPanel = null;
                foreach (DragDropPanel panel in panels)
                {
                    if (!addedPanels.Contains(panel) && (lowestPanel == null || ((Grid.GetRow(panel) * columns) + Grid.GetColumn(panel) < (Grid.GetRow(lowestPanel) * columns) + Grid.GetColumn(lowestPanel))))
                    {
                        lowestPanel = panel;
                    }
                }

                if (lowestPanel != null)
                {
                    addedPanels.Add(lowestPanel);
                    orderedPanels.Add(i, lowestPanel);
                }
            }

            return orderedPanels;
        }

        #endregion

        #region DragDropPanelHost events

        /// <summary>
        /// Updates the panel layouts.
        /// </summary>
        /// <param name="sender">The drag dock panel host.</param>
        /// <param name="e">Size changed event args.</param>
        private void DragDropPanelHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePanelLayout();
        }

        /// <summary>
        /// Updates the layout when in design mode.
        /// </summary>
        /// <param name="sender">The drag dock panel host.</param>
        /// <param name="e">Event Args.</param>
        private void DragDropPanelHost_LayoutUpdated(object sender, EventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Dictionary<int, DragDropPanel> orderedPanels = new Dictionary<int, DragDropPanel>();

                for (int i = 0; i < panels.Count; i++)
                {
                    if (panels[i].GetType() == typeof(DragDropPanel))
                    {
                        DragDropPanel panel = (DragDropPanel)panels[i];
                        orderedPanels.Add(i, panel);
                    }
                }

                SetRowsAndColumns(orderedPanels);
                UpdatePanelLayout();
            }
        }

        #endregion

        #region Panel dragging events

        /// <summary>
        /// Keeps a reference to the dragging panel.
        /// </summary>
        /// <param name="sender">The dragging panel.</param>
        /// <param name="args">Drag event args.</param>
        private void DragDropPanel_DragStarted(object sender, DragEventArgs args)
        {
            DragDropPanel? panel = sender as DragDropPanel;

            // Keep reference to dragging panel
            draggingPanel = panel;
        }

        /// <summary>
        /// Shuffles the panels around.
        /// </summary>
        /// <param name="sender">The dragging panel.</param>
        /// <param name="args">Drag event args.</param>
        private void DragDropPanel_DragMoved(object sender, DragEventArgs args)
        {
            Point mousePosInHost = args.MouseEventArgs.GetPosition(this);

            int currentRow = (int)Math.Floor(mousePosInHost.Y / (ActualHeight / (double)rows));
            int currentColumn = (int)Math.Floor(mousePosInHost.X / (ActualWidth / (double)columns));

            // Stores the panel we will swap with
            DragDropPanel? swapPanel = null;

            // Loop through children to see if there is a panel to swap with
            foreach (DragDropPanel panel in panels)
            {
                // If the panel is not the dragging panel and is in the current row
                // or current column... mark it as the panel to swap with
                if (panel != draggingPanel && Grid.GetColumn(panel) == currentColumn && Grid.GetRow(panel) == currentRow)
                {
                    swapPanel = panel;
                    break;
                }
            }

            // If there is a panel to swap with
            if (swapPanel != null)
            {
                // Store the new row and column
                int draggingPanelNewColumn = Grid.GetColumn(swapPanel);
                int draggingPanelNewRow = Grid.GetRow(swapPanel);

                // Update the swapping panel row and column
                Grid.SetColumn(swapPanel, Grid.GetColumn(draggingPanel));
                Grid.SetRow(swapPanel, Grid.GetRow(draggingPanel));

                // Update the dragging panel row and column
                Grid.SetColumn(draggingPanel, draggingPanelNewColumn);
                Grid.SetRow(draggingPanel, draggingPanelNewRow);

                // Animate the layout to the new positions
                AnimatePanelLayout();
            }
        }

        /// <summary>
        /// Drops the dragging panel.
        /// </summary>
        /// <param name="sender">The dragging panel.</param>
        /// <param name="args">Drag event args.</param>
        private void DragDropPanel_DragFinished(object sender, DragEventArgs args)
        {
            // Set dragging panel back to null
            draggingPanel = null;

            // Update the layout (to reset all panel positions)
            UpdatePanelLayout();
        }

        #endregion

        #region Panel maximized / minimized events

        /// <summary>
        /// Puts all of the panel back to a grid view.
        /// </summary>
        /// <param name="sender">The minimising panel.</param>
        /// <param name="e">Event args.</param>
        private void DragDropPanel_Restored(object? sender, EventArgs? e)
        {
            // Set max'ed panel to null
            maximizedPanel = null;

            // Loop through children to disable dragging
            foreach (DragDropPanel panel in panels)
            {
                panel.Restore();
                panel.DraggingEnabled = true;
            }

            // Update sizes and layout
            AnimatePanelSizes();
            AnimatePanelLayout();
        }

        /// <summary>
        /// Maximises a panel.
        /// </summary>
        /// <param name="sender">the panel to maximise.</param>
        /// <param name="e">Event args.</param>
        private void DragDropPanel_Maximized(object sender, EventArgs e)
        {
            DragDropPanel? maxPanel = sender as DragDropPanel;

            // Store max'ed panel
            maximizedPanel = maxPanel;

            // Loop through children to disable dragging
            foreach (DragDropPanel panel in panels)
            {
                panel.DraggingEnabled = false;

                if (panel != maximizedPanel)
                {
                    panel.Minimize();
                }
            }

            // Update sizes and layout
            AnimatePanelSizes();
            AnimatePanelLayout();
        }

        #endregion

        #region Private layout methods

        /// <summary>
        /// Updates the panel layout without animation
        /// This does size and position without animation
        /// </summary>
        private void UpdatePanelLayout()
        {
            if (double.IsInfinity(ActualWidth) || double.IsNaN(ActualWidth) || ActualWidth == 0)
            {
                return;
            }

            // If we are not in max'ed panel mode...
            if (maximizedPanel == null)
            {
                // Layout children as per rows and columns
                foreach (DragDropPanel panel in panels)
                {
                    Canvas.SetLeft(panel, Grid.GetColumn(panel) * (ActualWidth / columns));
                    Canvas.SetTop(panel, Grid.GetRow(panel) * (ActualHeight / rows));

                    double width = (ActualWidth / (double)columns) - panel.Margin.Left - panel.Margin.Right;
                    double height = (ActualHeight / (double)rows) - panel.Margin.Top - panel.Margin.Bottom;

                    if (width < 0)
                        width = 0;

                    if (height < 0)
                        height = 0;

                    panel.Width = width;
                    panel.Height = height;
                }
            }
            else
            {
                Dictionary<int, DragDropPanel> orderedPanels = new Dictionary<int, DragDropPanel>();

                // Loop through children to order them according to their
                // current row and column...
                foreach (DragDropPanel panel in panels)
                {
                    orderedPanels.Add((Grid.GetRow(panel) * columns) + Grid.GetColumn(panel), panel);
                }

                // Set initial top of minimized panels to 0
                double currentOffset = 0.0;

                // For each of the panels (as ordered in the grid)
                for (int i = 0; i < orderedPanels.Count; i++)
                {
                    // If the current panel is not the maximized panel
                    if (orderedPanels[i] != maximizedPanel)
                    {
                        double newWidth = minimizedColumnWidth - orderedPanels[i].Margin.Left - orderedPanels[i].Margin.Right;
                        double newHeight = (ActualHeight / (double)(panels.Count - 1)) - orderedPanels[i].Margin.Top - orderedPanels[i].Margin.Bottom;
                        if (minimizedPosition.Equals(MinimizedPosition.Bottom) || minimizedPosition.Equals(MinimizedPosition.Top))
                        {
                            newWidth = (ActualWidth / (double)(panels.Count - 1)) - orderedPanels[i].Margin.Left - orderedPanels[i].Margin.Right;
                            newHeight = minimizedRowHeight - orderedPanels[i].Margin.Top - orderedPanels[i].Margin.Bottom;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.None))
                        {
                            newWidth = 0;
                            newHeight = 0;
                        }

                        if (newHeight < 0)
                            newHeight = 0;

                        if (newWidth < 0)
                            newWidth = 0;

                        // Set the size of the panel
                        orderedPanels[i].Width = newWidth;
                        orderedPanels[i].Height = newHeight;

                        double newX = 0;
                        double newY = currentOffset;
                        if (minimizedPosition.Equals(MinimizedPosition.Right))
                        {
                            newX = ActualWidth - minimizedColumnWidth;
                            newY = currentOffset;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Left))
                        {
                            newX = 0;
                            newY = currentOffset;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Bottom))
                        {
                            newX = currentOffset;
                            newY = ActualHeight - minimizedRowHeight;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Top))
                        {
                            newX = currentOffset;
                            newY = 0;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.None))
                        {
                            newX = ActualWidth / 2;
                            newY = ActualHeight / 2;
                        }

                        // Set the position of the panel
                        Canvas.SetLeft(orderedPanels[i], newX);
                        Canvas.SetTop(orderedPanels[i], newY);

                        if (minimizedPosition.Equals(MinimizedPosition.Left) || minimizedPosition.Equals(MinimizedPosition.Right))
                        {
                            // Increment current top
                            currentOffset += ActualHeight / (double)(panels.Count - 1);
                        }
                        else
                        {
                            // Increment current left
                            currentOffset += ActualWidth / (double)(panels.Count - 1);
                        }
                    }
                    else
                    {
                        double newWidth = ActualWidth - minimizedColumnWidth - orderedPanels[i].Margin.Left - orderedPanels[i].Margin.Right;
                        double newHeight = ActualHeight - orderedPanels[i].Margin.Top - orderedPanels[i].Margin.Bottom;
                        if (minimizedPosition.Equals(MinimizedPosition.Bottom) || minimizedPosition.Equals(MinimizedPosition.Top))
                        {
                            newWidth = ActualWidth - orderedPanels[i].Margin.Left - orderedPanels[i].Margin.Right;
                            newHeight = ActualHeight - minimizedRowHeight - orderedPanels[i].Margin.Top - orderedPanels[i].Margin.Bottom;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.None))
                        {
                            newWidth = ActualWidth - orderedPanels[i].Margin.Right - orderedPanels[i].Margin.Left;
                            newHeight = ActualHeight - orderedPanels[i].Margin.Bottom - orderedPanels[i].Margin.Top;
                        }

                        if (newHeight < 0)
                            newHeight = 0;

                        if (newWidth < 0)
                            newWidth = 0;

                        // Set the size of the panel
                        orderedPanels[i].Width = newWidth;
                        orderedPanels[i].Height = newHeight;

                        double newX = 0;
                        double newY = 0;
                        if (minimizedPosition.Equals(MinimizedPosition.Right))
                        {
                            newX = 0;
                            newY = 0;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Left))
                        {
                            newX = minimizedColumnWidth;
                            newY = 0;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Bottom))
                        {
                            newX = 0;
                            newY = 0;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Top))
                        {
                            newX = 0;
                            newY = minimizedRowHeight;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.None))
                        {
                            newX = 0;
                            newY = 0;
                        }

                        // Set the position of the panel
                        Canvas.SetLeft(orderedPanels[i], newX);
                        Canvas.SetTop(orderedPanels[i], newY);
                    }
                }
            }
        }

        /// <summary>
        /// Animates the panel sizes
        /// </summary>
        private void AnimatePanelSizes()
        {
            if (double.IsInfinity(ActualWidth) || double.IsNaN(ActualWidth) || ActualWidth == 0)
            {
                return;
            }

            // If there is not a maxmized panel
            if (maximizedPanel == null)
            {
                // Animate the panel sizes to row / column sizes
                foreach (DragDropPanel panel in panels)
                {
                    double width = (ActualWidth / (double)columns) - panel.Margin.Left - panel.Margin.Right;
                    double height = (ActualHeight / (double)rows) - panel.Margin.Top - panel.Margin.Bottom;

                    if (width < 0)
                        width = 0;

                    if (height < 0)
                        height = 0;

                    panel.AnimateSize(width, height);
                }
            }
            else
            {
                // Loop through the children
                foreach (DragDropPanel panel in panels)
                {
                    // Set the size of the non maximized children
                    if (panel != maximizedPanel)
                    {
                        double newWidth = minimizedColumnWidth - panel.Margin.Left - panel.Margin.Right;
                        double newHeight = (ActualHeight / (double)(panels.Count - 1)) - panel.Margin.Top - panel.Margin.Bottom;
                        if (minimizedPosition.Equals(MinimizedPosition.Bottom) || minimizedPosition.Equals(MinimizedPosition.Top))
                        {
                            newWidth = (ActualWidth / (double)(panels.Count - 1)) - panel.Margin.Left - panel.Margin.Right;
                            newHeight = minimizedRowHeight - panel.Margin.Top - panel.Margin.Bottom;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.None))
                        {
                            newWidth = newHeight = 0;
                        }

                        if (newHeight < 0)
                            newHeight = 0;

                        if (newWidth < 0)
                            newWidth = 0;

                        panel.AnimateSize(newWidth, newHeight);
                    }
                    else
                    {
                        double newWidth = ActualWidth - minimizedColumnWidth - panel.Margin.Left - panel.Margin.Right;
                        double newHeight = ActualHeight - panel.Margin.Top - panel.Margin.Bottom;
                        if (minimizedPosition.Equals(MinimizedPosition.Bottom) || minimizedPosition.Equals(MinimizedPosition.Top))
                        {
                            newWidth = ActualWidth - panel.Margin.Left - panel.Margin.Right;
                            newHeight = ActualHeight - minimizedRowHeight - panel.Margin.Top - panel.Margin.Bottom;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.None))
                        {
                            newWidth = ActualWidth - panel.Margin.Right - panel.Margin.Left;
                            newHeight = ActualHeight - panel.Margin.Bottom - panel.Margin.Top;
                        }

                        if (newHeight < 0)
                            newHeight = 0;

                        if (newWidth < 0)
                            newWidth = 0;

                        panel.AnimateSize(newWidth, newHeight);
                    }
                }
            }
        }

        /// <summary>
        /// Animate the panel positions
        /// </summary>
        private void AnimatePanelLayout()
        {
            if (double.IsInfinity(ActualWidth) || double.IsNaN(ActualWidth) || ActualWidth == 0)
            {
                return;
            }

            // If we are not in max'ed panel mode...
            if (maximizedPanel == null)
            {
                // Loop through children and size to row and columns
                foreach (UIElement child in panels)
                {
                    DragDropPanel panel = (DragDropPanel)child;

                    if (panel != draggingPanel)
                    {
                        panel.AnimatePosition(Grid.GetColumn(panel) * (ActualWidth / columns), Grid.GetRow(panel) * (ActualHeight / rows));
                    }
                }
            }
            else
            {
                Dictionary<int, DragDropPanel> orderedPanels = new Dictionary<int, DragDropPanel>();

                // Loop through children to order them according to their current row and column...
                foreach (UIElement child in panels)
                {
                    DragDropPanel panel = (DragDropPanel)child;

                    orderedPanels.Add((Grid.GetRow(panel) * columns) + Grid.GetColumn(panel), panel);
                }

                // Set initial top of minimized panels to 0
                double currentOffset = 0.0;

                // For each of the panels (as ordered in the grid)
                for (int i = 0; i < orderedPanels.Count; i++)
                {
                    // If the current panel is not the maximized panel
                    if (orderedPanels[i] != maximizedPanel)
                    {
                        double newX = 0;
                        double newY = currentOffset;
                        if (minimizedPosition.Equals(MinimizedPosition.Right))
                        {
                            newX = ActualWidth - minimizedColumnWidth;
                            newY = currentOffset;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Left))
                        {
                            newX = 0;
                            newY = currentOffset;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Bottom))
                        {
                            newX = currentOffset;
                            newY = ActualHeight - minimizedRowHeight;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Top))
                        {
                            newX = currentOffset;
                            newY = 0;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.None))
                        {
                            newX = ActualWidth / 2;
                            newY = ActualHeight / 2;
                        }

                        orderedPanels[i].AnimatePosition(newX, newY);

                        if (minimizedPosition.Equals(MinimizedPosition.Left) || minimizedPosition.Equals(MinimizedPosition.Right))
                        {
                            // Increment current top
                            currentOffset += ActualHeight / (double)(panels.Count - 1);
                        }
                        else
                        {
                            // Increment current left
                            currentOffset += ActualWidth / (double)(panels.Count - 1);
                        }
                    }
                    else
                    {
                        double newX = 0;
                        double newY = 0;
                        if (minimizedPosition.Equals(MinimizedPosition.Right))
                        {
                            newX = 0;
                            newY = 0;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Left))
                        {
                            newX = minimizedColumnWidth;
                            newY = 0;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Bottom))
                        {
                            newX = 0;
                            newY = 0;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.Top))
                        {
                            newX = 0;
                            newY = minimizedRowHeight;
                        }
                        else if (minimizedPosition.Equals(MinimizedPosition.None))
                        {
                            newX = 0;
                            newY = 0;
                        }

                        orderedPanels[i].AnimatePosition(newX, newY);
                    }
                }
            }
        }

        #endregion
    }
}
