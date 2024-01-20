using System.Collections;
using UnityEngine;

namespace Organelles
{
    public class Tail : MonoBehaviour
    {
        private float maxForceMagnitude; // Maximum magnitude of the force
        private float minForceMagnitude; // Minimum magnitude of the force
        private float applyForceInterval; // Interval in seconds at which force is applied
        private float maxTorque; // Maximum torque magnitude

        private Rigidbody2D _rb;
        private Coroutine _forceCoroutine;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();

            // Randomize properties for each tail
            maxForceMagnitude = Random.Range(0.1f, 0.2f);
            minForceMagnitude = Random.Range(0.02f, 0.1f);
            applyForceInterval = Random.Range(0.2f, 0.5f);
            maxTorque = Random.Range(0.05f, 0.15f);

            _forceCoroutine = StartCoroutine(ApplyRandomForce());
        }

        private IEnumerator ApplyRandomForce()
        {
            while (true)
            {
                // Apply a varying force at regular intervals
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                float randomMagnitude = Random.Range(minForceMagnitude, maxForceMagnitude);
                _rb.AddForce(randomDirection * randomMagnitude * Time.deltaTime, ForceMode2D.Impulse);

                // Gradually rotate the tail to simulate fluidity
                float randomTorque = Random.Range(-maxTorque, maxTorque);
                _rb.AddTorque(randomTorque * Time.deltaTime);

                // Wait for the specified interval before applying force again
                yield return new WaitForSeconds(applyForceInterval);
            }
        }

        private void OnDestroy()
        {
            // Stop the coroutine if the tail object is destroyed
            if (_forceCoroutine != null)
            {
                StopCoroutine(_forceCoroutine);
            }
        }
    }
}