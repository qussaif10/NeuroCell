using UnityEngine;

namespace Tools
{
    public class RandomMover : MonoBehaviour
    {
        public float speed = 1.0f; // Speed of the movement
        public float rotationSpeed = 30.0f; // Speed of the rotation

        private Vector3 direction; // Direction of the movement
        private float rotationDirection; // Direction of the rotation (1 for clockwise, -1 for counterclockwise)

        private float maxX = 8.8f; // Maximum X boundary
        private float maxY = 4f; // Maximum Y boundary

        void Start()
        {
            // Choose a random direction for movement
            float angle = Random.Range(0f, 360f);
            direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            // Choose a random direction for rotation (clockwise or counterclockwise)
            rotationDirection = Random.Range(0, 2) * 2 - 1; // Will be either 1 or -1
        }

        void Update()
        {
            // Move the object in the chosen direction
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // Rotate the object around its Z-axis
            transform.Rotate(new Vector3(0, 0, rotationDirection * rotationSpeed * Time.deltaTime));

            // Check boundaries and reverse direction if necessary
            CheckAndReverseDirectionIfNeeded();
        }

        void CheckAndReverseDirectionIfNeeded()
        {
            Vector3 pos = transform.position;

            // Check X boundaries
            if (pos.x > maxX || pos.x < -maxX)
            {
                direction.x = -direction.x; // Reverse X direction
                transform.position = new Vector3(Mathf.Clamp(pos.x, -maxX, maxX), pos.y, pos.z); // Clamp position within boundaries
            }

            // Check Y boundaries
            if (pos.y > maxY || pos.y < -maxY)
            {
                direction.y = -direction.y; // Reverse Y direction
                transform.position = new Vector3(pos.x, Mathf.Clamp(pos.y, -maxY, maxY), pos.z); // Clamp position within boundaries
            }
        }
    }
}