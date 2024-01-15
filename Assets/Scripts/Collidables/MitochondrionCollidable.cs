using UnityEngine;

namespace Collidables
{
    public class MitochondrionCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.Mitochondrion;
    }
}
