using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.CustomVector
{
    public class CustomVector3
    {
        public float X;
        public float Y;
        public float Z;

        public CustomVector3(Vector3 vector3)
        {
            X = vector3.x;
            Y = vector3.y;
            Z = vector3.z;
        }
        
        public static implicit operator CustomVector3(Vector3 vector)
        {
            return new CustomVector3(vector);            
        }
        
        public static implicit operator Vector3(CustomVector3 vector)
        {
            return new()
            {
                x = vector.X,
                y = vector.Y,
                z = vector.Z
            };
        }
    }
}