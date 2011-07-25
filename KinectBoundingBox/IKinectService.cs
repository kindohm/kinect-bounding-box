using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectBoundingBox
{
    public interface IKinectService
    {
        void Initialize();
        void Cleanup();
        PlanarImage LatestVideoImage { get; }
        event EventHandler<SkeletonEventArgs> SkeletonUpdated;
        
    }
}
