using UnityEngine;

namespace Collidables
{
    public class NucleusCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.Nucleus;
    }
}
