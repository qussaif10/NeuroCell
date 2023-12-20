using UnityEngine;

public class PhospholipidTail : MonoBehaviour
{
    public float forceMagnitude = 0.001f;

    void Start()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);
    }
}