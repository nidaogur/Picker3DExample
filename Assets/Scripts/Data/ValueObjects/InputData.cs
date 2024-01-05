using System;
using System.Numerics;
using Unity.Mathematics;

namespace Data.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        public float horizontalInputSpeed;
        public float2 clampValues;
        public float clampSpeed;

    }
}