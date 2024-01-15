using UnityEngine;

namespace Collidables
{
    public class RoughErCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.RoughEr;
    }
}
