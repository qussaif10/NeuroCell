using UnityEngine;

namespace MouseEvents
{
    public class DragRigidbody2D : MonoBehaviour
    {
        public float radius = 1.0f; // Radius to detect Rigidbody2D
        public float springForce = 10.0f; // Strength of the spring force

        private Rigidbody2D _selectedRigidbody;

        void Update()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
            // Select Rigidbody when mouse is clicked
            if (Input.GetMouseButtonDown(0))
            {
                _selectedRigidbody = FindNearestRigidbody(mousePosition);
            }

            // Release Rigidbody when mouse is released
            if (Input.GetMouseButtonUp(0))
            {
                _selectedRigidbody = null;
            }

            // Apply spring force to drag Rigidbody towards the mouse
            if (_selectedRigidbody != null && Input.GetMouseButton(0))
            {
                Vector2 forceDirection = mousePosition - _selectedRigidbody.position;
                _selectedRigidbody.AddForce(forceDirection * springForce, ForceMode2D.Force);
            }
        }

        private Rigidbody2D FindNearestRigidbody(Vector2 position)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
            Rigidbody2D nearestRigidbody = null;
            float minDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                Rigidbody2D rb = collider.attachedRigidbody;
                if (rb != null)
                {
                    float distance = Vector2.Distance(position, rb.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestRigidbody = rb;
                    }
                }
            }

            return nearestRigidbody;
        }
    }
}