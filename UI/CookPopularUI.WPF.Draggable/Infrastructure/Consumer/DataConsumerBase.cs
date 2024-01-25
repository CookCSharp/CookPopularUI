/*
 *Description: DataConsumerBase
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 15:10:26
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

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// This class provides some default implementations for
    /// IDataConsumer that can be used by derived classes.
    /// This class represents dragged data that can be consumed.
    /// </summary>
    public abstract class DataConsumerBase : IDataConsumer
    {
        /// <summary>
        /// A list of formats this data object consumer supports
        /// </summary>
        private string[] _dataFormats;

        public virtual DataConsumerActions DataConsumerActions
            => DataConsumerActions.DragEnter | DataConsumerActions.DragOver | DataConsumerActions.Drop | DataConsumerActions.None;

        /// <summary>
        /// Create a Data Consumer that supports the specified data formats
        /// </summary>
        /// <param name="dataFormats">Data formats supported by this data consumer</param>
        public DataConsumerBase(string[] dataFormats)
        {
            _dataFormats = dataFormats;
            Debug.Assert((dataFormats != null) && (dataFormats.Length > 0), "Must have at least one format string");
        }

        /// <summary>
        /// Search the available data formats for a
        /// supported data format and return the first match
        /// </summary>
        /// <param name="e">DragEventArgs from one of the four Drag events</param>
        /// <returns>Returns first available/supported data object match; null when no match is found</returns>
        public virtual object? GetData(System.Windows.DragEventArgs e)
        {
            object? data = null;
            string[] dataFormats = e.Data.GetFormats();
            foreach (string dataFormat in dataFormats)
            {
                foreach (string dataFormatString in _dataFormats)
                {
                    if (dataFormat.Equals(dataFormatString))
                    {
                        try
                        {
                            data = e.Data.GetData(dataFormat);
                        }
                        catch /*(COMException e2)*/
                        {
                        }
                    }
                    if (data != null) return data;
                }
            }

            return null;
        }

        public virtual void DropTarget_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            DragOverOrDrop(false, sender, e);
        }

        public virtual void DropTarget_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            DragOverOrDrop(false, sender, e);
        }

        public virtual void DropTarget_Drop(object sender, System.Windows.DragEventArgs e)
        {
            DragOverOrDrop(true, sender, e);
        }

        public virtual void DropTarget_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            DragOverOrDrop(false, sender, e);
        }

        public virtual void DragOverOrDrop(bool bDrop, object sender, System.Windows.DragEventArgs e)
        {
            throw new NotImplementedException("DragOverOrDrop not implemented");
        }
    }
}
