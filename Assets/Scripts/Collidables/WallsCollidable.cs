using UnityEngine;

namespace Collidables
{
    public class WallsCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.Wall;
    }
}