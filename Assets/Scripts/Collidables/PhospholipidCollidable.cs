using UnityEngine;

namespace Collidables
{
    public class PhospholipidCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.Phospholipid;
    }
}
