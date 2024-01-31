/*
 *Description: WrapPanelFill
 *Author: Chance.zheng
 *Creat Time: 2024/1/30 19:59:40
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Size = System.Windows.Size;

namespace CookPopularUI.WPF.Controls
{
    public class WrapPanelFill : WrapPanel
    {
        private struct UVSize
        {
            internal double U;
            internal double V;
            private Orientation _orientation;

            internal double Width
            {
                get { return (_orientation == Orientation.Horizontal ? U : V); }
                set { if (_orientation == Orientation.Horizontal) U = value; else V = value; }
            }
            internal double Height
            {
                get { return (_orientation == Orientation.Horizontal ? V : U); }
                set { if (_orientation == Orientation.Horizontal) V = value; else U = value; }
            }

            internal UVSize(Orientation orientation, double width, double height)
            {
                U = V = 0d;
                _orientation = orientation;
                Width = width;
                Height = height;
            }

            internal UVSize(Orientation orientation)
            {
                U = V = 0d;
                _orientation = orientation;
            }
        }

        private class LineInfo
        {
            public List<UIElement> ElementsWithNoWidthSet = new List<UIElement>();
            public UVSize Size;
            public double SpaceLeft = 0;
            public double WidthCorrectionPerElement = 0;
            public double Correction = 0;
        }


        private List<LineInfo> _lineInfos = new List<LineInfo>();
        private bool _atLeastOneElementCanHasItsWidthExpanded = false;


        public static bool GetSetChildFill(DependencyObject obj) => (bool)obj.GetValue(SetChildFillProperty);
        public static void SetSetChildFill(DependencyObject obj, bool value) => obj.SetValue(SetChildFillProperty, value);
        public static readonly DependencyProperty SetChildFillProperty =
            DependencyProperty.RegisterAttached("SetChildFill", typeof(bool), typeof(WrapPanelFill), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsParentMeasure));


        protected override Size MeasureOverride(Size constraint)
        {
            UVSize curLineSize = new UVSize(Orientation);
            UVSize panelSize = new UVSize(Orientation);
            UVSize uvConstraint = new UVSize(Orientation, constraint.Width, constraint.Height);
            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            bool itemWidthSet = !double.IsNaN(itemWidth);
            bool itemHeightSet = !double.IsNaN(itemHeight);

            Size childConstraint = new Size(itemWidthSet ? itemWidth : constraint.Width, itemHeightSet ? itemHeight : constraint.Height);

            UIElementCollection children = InternalChildren;

            LineInfo currentLineInfo = new LineInfo();
            _lineInfos.Clear();
            _atLeastOneElementCanHasItsWidthExpanded = false;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                UIElement child = children[i];
                if (child == null) continue;

                child.Measure(childConstraint);


                UVSize sz = new UVSize(Orientation,
                                       itemWidthSet ? itemWidth : child.DesiredSize.Width,
                                       itemHeightSet ? itemHeight : child.DesiredSize.Height);

                if (DoubleGreaterThan(curLineSize.U + sz.U, uvConstraint.U))
                {
                    currentLineInfo.Size = curLineSize;
                    _lineInfos.Add(currentLineInfo);

                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;

                    currentLineInfo = new LineInfo();
                    var feChild = (FrameworkElement)child;
                    if (GetSetChildFill(feChild))
                    {
                        currentLineInfo.ElementsWithNoWidthSet.Add(feChild);
                        _atLeastOneElementCanHasItsWidthExpanded = true;
                    }

                    if (DoubleGreaterThan(sz.U, uvConstraint.U))
                    {
                        currentLineInfo = new LineInfo();

                        panelSize.U = Math.Max(sz.U, panelSize.U);
                        panelSize.V += sz.V;
                        curLineSize = new UVSize(Orientation);
                    }
                }
                else
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);

                    var feChild = (FrameworkElement)child;
                    if (GetSetChildFill(feChild))
                    {
                        currentLineInfo.ElementsWithNoWidthSet.Add(feChild);
                        _atLeastOneElementCanHasItsWidthExpanded = true;
                    }
                }
            }

            if (curLineSize.U > 0)
            {
                currentLineInfo.Size = curLineSize;
                _lineInfos.Add(currentLineInfo);
            }

            panelSize.U = Math.Max(curLineSize.U, panelSize.U);
            panelSize.V += curLineSize.V;

            if (_atLeastOneElementCanHasItsWidthExpanded)
            {
                return new Size(constraint.Width, panelSize.Height);
            }

            return new Size(panelSize.Width, panelSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int lineIndex = 0;
            int firstInLine = 0;
            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            double accumulatedV = 0;
            double itemU = (Orientation == Orientation.Horizontal ? itemWidth : itemHeight);
            UVSize curLineSize = new UVSize(Orientation);
            UVSize uvFinalSize = new UVSize(Orientation, finalSize.Width, finalSize.Height);
            bool itemWidthSet = !double.IsNaN(itemWidth);
            bool itemHeightSet = !double.IsNaN(itemHeight);
            bool useItemU = (Orientation == Orientation.Horizontal ? itemWidthSet : itemHeightSet);

            UIElementCollection children = InternalChildren;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                UIElement child = children[i] as UIElement;
                if (child == null) continue;

                UVSize sz = new UVSize(Orientation,
                                       itemWidthSet ? itemWidth : child.DesiredSize.Width,
                                       itemHeightSet ? itemHeight : child.DesiredSize.Height);

                if (DoubleGreaterThan(curLineSize.U + sz.U, uvFinalSize.U))
                {
                    ArrangeLine(lineIndex, accumulatedV, curLineSize.V, firstInLine, i, useItemU, itemU, uvFinalSize);
                    lineIndex++;

                    accumulatedV += curLineSize.V;
                    curLineSize = sz;

                    if (DoubleGreaterThan(sz.U, uvFinalSize.U))
                    {
                        ArrangeLine(lineIndex, accumulatedV, sz.V, i, ++i, useItemU, itemU, uvFinalSize);

                        accumulatedV += sz.V;
                        curLineSize = new UVSize(Orientation);
                    }
                    firstInLine = i;
                }
                else
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                }
            }

            if (firstInLine < children.Count)
            {
                ArrangeLine(lineIndex, accumulatedV, curLineSize.V, firstInLine, children.Count, useItemU, itemU, uvFinalSize);
            }

            return finalSize;
        }

        private void ArrangeLine(int lineIndex, double v, double lineV, int start, int end, bool useItemU, double itemU, UVSize uvFinalSize)
        {
            double u = 0;
            bool isHorizontal = (Orientation == Orientation.Horizontal);

            if (lineIndex >= _lineInfos.Count) return;

            LineInfo lineInfo = _lineInfos[lineIndex];
            double lineSpaceAvailableForCorrection = Math.Max(uvFinalSize.U - lineInfo.Size.U, 0);
            double perControlCorrection = 0;
            if (lineSpaceAvailableForCorrection > 0 && lineInfo.Size.U > 0)
            {
                perControlCorrection = lineSpaceAvailableForCorrection / lineInfo.ElementsWithNoWidthSet.Count;
                if (double.IsInfinity(perControlCorrection))
                {
                    perControlCorrection = 0;
                }
            }
            int indexOfControlToAdjustSizeToFill = 0;
            UIElement? uIElementToAdjustNext = indexOfControlToAdjustSizeToFill < lineInfo.ElementsWithNoWidthSet.Count ? lineInfo.ElementsWithNoWidthSet[indexOfControlToAdjustSizeToFill] : null;

            UIElementCollection children = InternalChildren;
            for (int i = start; i < end; i++)
            {
                UIElement child = children[i];
                if (child != null)
                {
                    UVSize childSize = new UVSize(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);
                    double layoutSlotU = (useItemU ? itemU : childSize.U);

                    if (perControlCorrection > 0 && child == uIElementToAdjustNext)
                    {
                        layoutSlotU += perControlCorrection;

                        indexOfControlToAdjustSizeToFill++;
                        uIElementToAdjustNext = indexOfControlToAdjustSizeToFill < lineInfo.ElementsWithNoWidthSet.Count ? lineInfo.ElementsWithNoWidthSet[indexOfControlToAdjustSizeToFill] : null;
                    }

                    child.Arrange(new Rect(isHorizontal ? u : v,
                                           isHorizontal ? v : u,
                                           isHorizontal ? layoutSlotU : lineV,
                                           isHorizontal ? lineV : layoutSlotU));
                    u += layoutSlotU;
                }
            }
        }

        private bool DoubleAreClose(double value1, double value2)
        {
            if (value1 == value2) return true;

            // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < DBL_EPSILON
            double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * MathHelper.DBL_EPSILON;
            double delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        private bool DoubleGreaterThan(double value1, double value2)
        {
            return (value1 > value2) && !DoubleAreClose(value1, value2);
        }
    }
}
