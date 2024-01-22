using UnityEngine;

namespace Collidables
{
    public class OutsideCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.Outside;
    }
}