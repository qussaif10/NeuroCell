using System.Collections;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public float forceMagnitude = 0.1f; // Magnitude of the force
    public float applyForceInterval = 0.5f; // Interval in seconds at which force is applied

    private Rigidbody2D rb;
    private Coroutine forceCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        forceCoroutine = StartCoroutine(ApplyRandomForce());
    }

    private IEnumerator ApplyRandomForce()
    {
        while (true)
        {
            // Apply a random force at regular intervals
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);

            // Wait for the specified interval before applying force again
            yield return new WaitForSeconds(applyForceInterval);
        }
    }

    private void OnDestroy()
    {
        // Stop the coroutine if the tail object is destroyed
        if (forceCoroutine != null)
        {
            StopCoroutine(forceCoroutine);
        }
    }
}