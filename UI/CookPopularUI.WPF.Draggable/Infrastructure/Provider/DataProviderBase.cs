/*
 *Description: DataProviderBase
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 14:44:12
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// This class provides some default implementations for
    /// IDataProvider that can be used by most derived classes.
    /// This class defines a data object that can be dragged.
    /// </summary>
    /// <typeparam name="TSourceContainer">Type of the source container, e.g. TabControl</typeparam>
    /// <typeparam name="TSourceObject">Type of the source object, e.g. TabItem</typeparam>
    public abstract class DataProviderBase<TSourceContainer, TSourceObject> : IDataProvider where TSourceContainer : UIElement where TSourceObject : UIElement
    {
        public virtual bool AddAdorner => false;
        public virtual bool NeedsCaptureMouse => false;
        public abstract DragDropEffects AllowedEffects { get; }
        public abstract DataProviderActions DataProviderActions { get; }
        public DefaultAdorner DragAdorner { get; set; }
        public Point StartPosition { get; set; }
        public object SourceContainer { get; set; }
        public object SourceObject { get; set; }
        public object OriginalSourceObject { get; set; }

        /// <summary>
        /// Name of the dragged data object
        /// </summary>
        public string? SourceDataFormat { get; private set; }

        /// <summary>
        /// KeyStates saved from QueryContinueDrag
        /// </summary>
        private DragDropKeyStates? _keyStates = null;
        public DragDropKeyStates KeyStates
        {
            get
            {
                if (_keyStates != null)
                    return (DragDropKeyStates)_keyStates;
                else
                    throw new NotImplementedException("No KeyState value to return");
            }
        }

        /// <summary>
        /// EscapePressed saved from QueryContinueDrag
        /// </summary>
        private bool? _escapePressed = null;
        public bool EscapePressed
        {
            get
            {
                if (_escapePressed != null)
                    return (bool)_escapePressed;
                else
                    throw new NotImplementedException("No EscapePressed value to return");
            }
        }

        /// <summary>
        /// Create a Data Provider for specified SourceContainer/SourceObject
        /// identified by the specified data format string
        /// </summary>
        /// <param name="dataFormatString">Identifies the data object</param>
        public DataProviderBase(string dataFormatString)
        {
            Debug.Assert((dataFormatString != null) && (dataFormatString.Length > 0), "dataFormatString cannot be null and must not be an empty string");
            SourceDataFormat = dataFormatString;
        }

        public void Init()
        {
            _keyStates = null;
            _escapePressed = null;
        }

        public virtual bool IsSupportedContainerAndObject(bool initFlag, object dragSourceContainer, object dragSourceObject, object dragOriginalSourceObject)
        {
            if (initFlag)
            {
                Init();
                SourceContainer = dragSourceContainer;
                SourceObject = dragSourceObject;
                OriginalSourceObject = dragOriginalSourceObject;
            }

            return (dragSourceObject is TSourceObject) && (dragSourceContainer is TSourceContainer);
        }

        public virtual void SetData(ref DataObject data)
        {
            Debug.Assert(data.GetDataPresent(SourceDataFormat) == false, "Shouldn't set data more than once");
            data.SetData(SourceDataFormat, this);
        }

        public virtual void DragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            _escapePressed = e.EscapePressed;
            _keyStates = e.KeyStates;
        }

        public virtual void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            throw new NotImplementedException("GiveFeedback not implemented");
        }

        public virtual void DoDragDrop_Done(DragDropEffects resultEffects)
        {
            throw new NotImplementedException("DoDragDropFinished not implemented");
        }

        public virtual void Unparent()
        {
            throw new NotImplementedException("Unparent not implemented");
        }
    }
}
