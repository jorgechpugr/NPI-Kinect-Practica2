// ***********************************************************************
// <copyright file="MainWindowViewModel.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace KinectCam
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Microsoft.Kinect;

    /// <summary>
    /// Class MainWindowViewModel
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Is Color Stream Enabled
        /// </summary>
        private bool isColorStreamEnabledValue;

        
        /// <summary>
        /// Can Start the sensor
        /// </summary>
        private bool canStartValue;

        /// <summary>
        /// Can Stop the sensor
        /// </summary>
        private bool canStopValue;

        
        /// <summary>
        /// Collection of Color image format
        /// </summary>
        private ObservableCollection<ColorImageFormat> colorImageFormatvalue;

        /// <summary>
        /// current image format
        /// </summary>
        private ColorImageFormat currentImageFormatValue;

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        
        /// <summary>
        /// Gets the color image formats.
        /// </summary>
        /// <value>
        /// The color image formats.
        /// </value>
        public ObservableCollection<ColorImageFormat> ColorImageFormats
        {
            get
            {
                this.colorImageFormatvalue = new ObservableCollection<ColorImageFormat>();
                foreach (ColorImageFormat colorImageFormat in Enum.GetValues(typeof(ColorImageFormat)))
                {
                    this.colorImageFormatvalue.Add(colorImageFormat);
                }

                return this.colorImageFormatvalue;
            }
        }

        /// <summary>
        /// Gets or sets the current image format.
        /// </summary>
        /// <value>
        /// The current image format.
        /// </value>
        public ColorImageFormat CurrentImageFormat
        {
            get
            {
                return this.currentImageFormatValue;
            }

            set
            {
                if (this.currentImageFormatValue != value)
                {
                    this.currentImageFormatValue = value;
                    this.OnNotifyPropertyChange("CurrentImageFormat");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is color stream enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is color stream enabled; otherwise, <c>false</c>.</value>
        public bool IsColorStreamEnabled
        {
            get
            {
                return this.isColorStreamEnabledValue;
            }

            set
            {
                if (this.isColorStreamEnabledValue != value)
                {
                    this.isColorStreamEnabledValue = value;
                    this.OnNotifyPropertyChange("IsColorStreamEnabled");
                    this.OnNotifyPropertyChange("FrameNumber");
                }
            }
        }

        
        /// <summary>
        /// Gets or sets a value indicating whether this instance can start.
        /// </summary>
        /// <value><c>true</c> if this instance can start; otherwise, <c>false</c>.</value>
        public bool CanStart
        {
            get
            {
                return this.canStartValue;
            }

            set
            {
                if (this.canStartValue != value)
                {
                    this.canStartValue = value;
                    this.OnNotifyPropertyChange("CanStart");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can stop.
        /// </summary>
        /// <value><c>true</c> if this instance can stop; otherwise, <c>false</c>.</value>
        public bool CanStop
        {
            get
            {
                return this.canStopValue;
            }

            set
            {
                if (this.canStopValue != value)
                {
                    this.canStopValue = value;
                    this.OnNotifyPropertyChange("CanStop");
                }
            }
        }

        
        /// <summary>
        /// Called when [notify property change].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnNotifyPropertyChange(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
