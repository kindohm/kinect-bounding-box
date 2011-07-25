using System.ComponentModel;
using Microsoft.Research.Kinect.Nui;

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

            this.kinectService.SkeletonUpdated += new System.EventHandler<SkeletonEventArgs>(kinectService_SkeletonUpdated);
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
                this.TorsoOffsetX =
                    (this.BoundsWidth / 2) * e.TorsoPosition.X / (this.BoundsDisplaySize / 2);
                this.TorsoOffsetZ = (this.BoundsDepth / 2) * (e.TorsoPosition.Z 
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

        bool GetUserIsInRange(Vector torsoPosition)
        {
            return torsoPosition.Z > this.MinDistanceFromCamera &&
                torsoPosition.Z < (this.MinDistanceFromCamera + this.BoundsDepth)
                && torsoPosition.X > -this.BoundsWidth &&
                torsoPosition.X < this.BoundsWidth;
        }

        public void Cleanup()
        {
            this.kinectService.SkeletonUpdated -= kinectService_SkeletonUpdated;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
