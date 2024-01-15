using UnityEngine;

namespace Collidables
{
    public class SmoothErCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.SmoothEr;
    }
}
