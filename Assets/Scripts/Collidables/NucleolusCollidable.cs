using UnityEngine;

namespace Collidables
{
    public class NucleolusCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.Nucleolus;
    }
}
