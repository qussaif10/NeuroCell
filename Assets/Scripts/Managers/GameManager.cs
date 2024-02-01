using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void KillCell()
        {
            // Initialize the output lists
            var springJoints = new List<SpringJoint2D>();
            var rigidbodies = new List<Rigidbody2D>();

            // Find all GameObjects in the scene, including inactive ones
            GameObject[] allObjects = FindObjectsOfType<GameObject>(true);

            // Iterate through each GameObject
            foreach (var obj in allObjects)
            {
                // Get SpringJoint2D components and add them to the springJoints list
                springJoints.AddRange(obj.GetComponents<SpringJoint2D>());

                // Get Rigidbody2D components and add them to the rigidbodies list
                rigidbodies.AddRange(obj.GetComponents<Rigidbody2D>());
            }

            // Disable all spring joints
            foreach (var springJoint in springJoints)
            {
                springJoint.enabled = false;
            }

            // Apply a random force to each rigidbody
            foreach (var rigidbody in rigidbodies)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized; // Get a random direction
                float randomMagnitude = Random.Range(50, 100); // Random magnitude between 50 and 100
                Vector2 force = randomDirection * randomMagnitude; // Calculate the force vector

                rigidbody.AddForce(force);
            }
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void LoadSimulation()
        {
            SceneManager.LoadScene("Scenes/NeuroCell");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.K))
            {
                KillCell();
            }
        }
    }
}