using System;

namespace KinectBoundingBox
{
    public interface IKinectService
    {
        void Initialize();
        void Cleanup();
        event EventHandler<SkeletonEventArgs> SkeletonUpdated;
    }
}
