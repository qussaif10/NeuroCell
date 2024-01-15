using UnityEngine;

namespace Collidables
{
    public class GolgiInCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.GolgiIn;
    }
}
