using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

using System.ComponentModel;

namespace ImagingLibrary.Types
{
    public class MappedBitmapImage : INotifyPropertyChanged
    {
        #region Constructors
        public MappedBitmapImage()
        {

        }
        
        #endregion Constructors

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        #endregion Events

        #region Properties
        /// <summary>
        /// The source image that we map from.
        /// </summary>
        public BitmapSource Source
        {
            get
            {
                return _source;
            }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    NotifyChanged(nameof(Source));
                }
            }
        } BitmapSource _source;

        /// <summary>
        /// The starting X position for the mapping
        /// </summary>
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    NotifyChanged(nameof(X));
                }
            }
        } int _x = 0;

        /// <summary>
        /// The starting Y position for the mapping.
        /// </summary>
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    NotifyChanged(nameof(Y));
                }
            }
        } private int _y = 0;

        /// <summary>
        /// The width for the mapping.
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    NotifyChanged(nameof(Width));
                }
            }
        } private int _width = 32;

        /// <summary>
        /// The height for the mapping.
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    NotifyChanged(nameof(Height));
                }
            }
        } private int _height = 32;

        /// <summary>
        /// A read-only property to get the mapped image of the source.
        /// </summary>
        public BitmapSource MappedImage
        {
            get
            {
                return _mappedImage;
            }
        } private BitmapSource _mappedImage;
        #endregion Properties
        
        #region Methods
        protected EImageOperationResult Remap()
        {

            try
            {
                // Make sure we have a valid source image.
                if (Source == null) return EImageOperationResult.NullImage;

                // Make sure the positions and dimensions are correct.
                if (Width == 0 || Height == 0) return EImageOperationResult.InvalidPosition;
                if (X < 0 || Y < 0) return EImageOperationResult.InvalidPosition;

                // Cache the new remap.
                _mappedImage = ImageFactory.GetSubsection(Source, new Point(X, Y), Width, Height);

                // Notify any listeners that the MappedImage has changed.
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(MappedImage)));
                }

                // Return success.
                return EImageOperationResult.Success;
            }
            catch
            {
                return EImageOperationResult.UnspecifiedError;
            }
        }
        public void SaveToFile(string path)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(MappedImage));
            using (var filestream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                encoder.Save(filestream);    
        }
        #endregion Methods

        #region Helper Methods
        void NotifyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
            Console.WriteLine($"Remap status was: {Remap().ToString()}");
        }
        #endregion Helper Methods
    }
}
