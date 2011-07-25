using System;
using System.Linq;
using Microsoft.Research.Kinect.Nui;

namespace KinectBoundingBox
{
    public class ConcreteKinectService : IKinectService
    {
        Runtime runtime;

        public void Initialize()
        {
            runtime = new Runtime();
            runtime.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(runtime_SkeletonFrameReady);
            runtime.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(runtime_VideoFrameReady);
            runtime.Initialize(RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
            runtime.VideoStream.Open(ImageStreamType.Video, 2,
                  ImageResolution.Resolution640x480, ImageType.Color);
        }

        public PlanarImage LatestVideoImage { get; private set; }

        void runtime_VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            this.LatestVideoImage = e.ImageFrame.Image;
        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            var skeleton =
                           e.SkeletonFrame.Skeletons
                           .Where(s => s.TrackingState == SkeletonTrackingState.Tracked)
                           .FirstOrDefault();

            if (skeleton == null)
            {
                return;
            }

            if (this.SkeletonUpdated != null)
            {
                this.SkeletonUpdated(this, new SkeletonEventArgs()
                {
                    RightHandPosition = skeleton.Joints[JointID.HandRight].Position, 
                    TorsoPosition = skeleton.Joints[JointID.Spine].Position
                });
            }
        }

        public void Cleanup()
        {
            if (runtime != null)
            {
                runtime.Uninitialize();
            }
        }

        public event EventHandler<SkeletonEventArgs> SkeletonUpdated;
    }
}
