using UnityEngine;

namespace Collidables
{
    public class CytosolCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.Cytosol;
    }
}