using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectBoundingBox
{
    public class SkeletonEventArgs : EventArgs
    {
        public Vector RightHandPosition { get; set; }
        public Vector TorsoPosition { get; set; }
    }
}
