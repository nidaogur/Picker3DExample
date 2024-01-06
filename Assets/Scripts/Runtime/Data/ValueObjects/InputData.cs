using System;
using Unity.Mathematics;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        public float horizontalInputSpeed;
        public float2 clampValues;
        public float clampSpeed;

    }
}