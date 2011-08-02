using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectBoundingBox
{
    public interface IKinectService
    {
        void Initialize();
        void Cleanup();
        event EventHandler<SkeletonEventArgs> SkeletonUpdated;
        
    }
}
