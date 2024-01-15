using UnityEngine;

namespace Collidables
{
    public class GolgiOutCollidable : MonoBehaviour, ICollidable
    {
        public CollidableType Type => CollidableType.GolgiOut;
    }
}
