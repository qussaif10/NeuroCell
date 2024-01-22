using UnityEngine;

namespace Tools
{
    public class Repeller : MonoBehaviour
    {
        public LayerMask repellableLayer;

        private void FixedUpdate()
        {
            RepelObjects();
        }

        private void RepelObjects(float repelRange = 0.1f, float repelForce = 0.3f)
        {
            var hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, repelRange, repellableLayer);
            
            foreach (var hitCollider in hitColliders)
            {
                var rb = hitCollider.GetComponent<Rigidbody2D>();
                if (rb == null) continue;
                var direction = hitCollider.transform.position - gameObject.transform.position;
                rb.AddForce(direction.normalized * repelForce, ForceMode2D.Impulse);
            }
        }
    }
}