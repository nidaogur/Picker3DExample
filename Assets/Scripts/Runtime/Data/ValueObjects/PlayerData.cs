using System;
using Unity.Mathematics;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerMovementData playerMovementData;
        public PlayerMeshData playerMeshData;
        public PlayerForceData playerForceData;
    }
    [Serializable]
    public class PlayerMovementData
    {
        public float forwardSpeed;
        public float sideWaySpeed;
    }
    [Serializable]
    public class PlayerMeshData
    {
        public float scaleCounter;
    }
    [Serializable]
    public class PlayerForceData
    {
        public float3 forceParameters;
    }

 

  
}