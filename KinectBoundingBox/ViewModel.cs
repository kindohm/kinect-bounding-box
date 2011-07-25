//////////////////////////////////////////////////////////////
// This code is for example purposes only. There are a 
// number of optimizations that should be made in this 
// code but have been excluded to make for a simpler example.
// Examples of optimizations include the use of constants
// and/or pre-calculating half-values of depths and width
// when they change so that divisions by two don't have
// to be done in real time.
//
// Love,
// Mike 
// 7/25/2011
//////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KinectBoundingBox
{
    public class ViewModel : INotifyPropertyChanged
    {
        IKinectService kinectService;

        public ViewModel(IKinectService kinectService)
        {
            this.kinectService = kinectService;
            this.HandOffsetX = 100;
            this.HandOffsetY = 100;
            this.MinDistanceFromCamera = 1.0d;
            this.BoundsDepth = .5d;
            this.BoundsWidth = .5d;
            this.BoundsDisplaySize = 300;
            this.kinectService.SkeletonUpdated += new EventHandler<SkeletonEventArgs>(kinectService_SkeletonUpdated);
        }

        bool showBoundingBox = true;
        public bool ShowBoundingBox
        {
            get { return this.showBoundingBox; }
            set
            {
                this.showBoundingBox = value;
                this.OnPropertyChanged("ShowBoundingBox");
            }
        }

        double minDistanceFromCamera;
        public double MinDistanceFromCamera
        {
            get { return this.minDistanceFromCamera; }
            set
            {
                this.minDistanceFromCamera = value;
                this.OnPropertyChanged("MinDistanceFromCamera");
            }
        }

        double boundsDisplaySize;
        public double BoundsDisplaySize
        {
            get
            {
                return this.boundsDisplaySize;
            }
            set
            {
                this.boundsDisplaySize = value;
                this.OnPropertyChanged("BoundsDisplaySize");
            }
        }

        double boundsWidth;
        public double BoundsWidth
        {
            get { return this.boundsWidth; }
            set
            {
                this.boundsWidth = value;
                this.OnPropertyChanged("BoundsWidth");
            }
        }

        double boundsDepth;
        public double BoundsDepth
        {
            get { return this.boundsDepth; }
            set
            {
                this.boundsDepth = value;
                this.OnPropertyChanged("BoundsDepth");
            }
        }

        Color userPointColor = Colors.Green;
        public Color UserPointColor
        {
            get { return this.userPointColor; }
            set
            {
                if (this.userPointColor != value)
                {
                    this.userPointColor = value;
                    this.OnPropertyChanged("UserPointColor");
                }
            }
        }

        double handOffsetX;
        public double HandOffsetX
        {
            get { return this.handOffsetX; }
            set
            {
                this.handOffsetX = value;
                this.OnPropertyChanged("HandOffsetX");
            }
        }

        double handOffsetY;
        public double HandOffsetY
        {
            get { return this.handOffsetY; }
            set
            {
                this.handOffsetY = value;
                this.OnPropertyChanged("HandOffsetY");
            }
        }

        double torsoOffsetX;
        public double TorsoOffsetX
        {
            get { return this.torsoOffsetX; }
            set
            {
                this.torsoOffsetX = value;
                this.OnPropertyChanged("TorsoOffsetX");
            }
        }

        double torsoOffsetZ;
        public double TorsoOffsetZ
        {
            get { return this.torsoOffsetZ; }
            set
            {
                this.torsoOffsetZ = value;
                this.OnPropertyChanged("TorsoOffsetZ");
            }
        }

        bool userIsInRange;
        public bool UserIsInRange
        {
            get { return this.userIsInRange; }
            set
            {
                this.userIsInRange = value;
                this.OnPropertyChanged("UserIsInRange");
            }
        }

        public BitmapSource VideoImage
        {
            get
            {
                return GetColorVideoFrame();
            }
        }

        void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        void kinectService_SkeletonUpdated(object sender, SkeletonEventArgs e)
        {
            if (App.Current.MainWindow != null)
            {
                this.UserIsInRange = this.GetUserIsInRange(e.TorsoPosition);
                this.UserPointColor = this.UserIsInRange
                    ? Color.FromArgb(255, 0, 255, 0) : Color.FromArgb(255, 255, 0, 0);

                this.TorsoOffsetX =
                    (this.BoundsDisplaySize / 2) * e.TorsoPosition.X / (this.BoundsWidth / 2);
                this.TorsoOffsetZ = (this.BoundsDisplaySize / 2) * (e.TorsoPosition.Z
                    - (this.MinDistanceFromCamera + this.BoundsDepth / 2)) / (this.BoundsDepth / 2);

                if (this.UserIsInRange)
                {
                    var midpointX = App.Current.MainWindow.Width / 2;
                    var midpointY = App.Current.MainWindow.Height / 2;

                    this.HandOffsetX = midpointX + (e.RightHandPosition.X * 500);
                    this.HandOffsetY = midpointY - (e.RightHandPosition.Y * 500);
                }
            }
        }

        bool GetUserIsInRange(Microsoft.Research.Kinect.Nui.Vector torsoPosition)
        {
            return torsoPosition.Z > this.MinDistanceFromCamera &
                torsoPosition.Z < (this.MinDistanceFromCamera + this.BoundsDepth)
                & torsoPosition.X > -this.BoundsWidth / 2 &
                torsoPosition.X < this.BoundsWidth / 2;
        }

        public void Cleanup()
        {
            this.kinectService.SkeletonUpdated -= kinectService_SkeletonUpdated;
        }

        BitmapSource GetColorVideoFrame()
        {
            if (this.kinectService.LatestVideoImage.Bits == null)
            {
                return null;
            }

            return BitmapSource.Create(
                this.kinectService.LatestVideoImage.Width,
                this.kinectService.LatestVideoImage.Height,
                96,
                96,
                PixelFormats.Bgr32,
                null,
                this.kinectService.LatestVideoImage.Bits,
                this.kinectService.LatestVideoImage.Width * this.kinectService.LatestVideoImage.BytesPerPixel);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
