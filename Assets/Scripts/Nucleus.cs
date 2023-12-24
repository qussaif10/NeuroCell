using System;
using UnityEngine;

public class Nucleus : MonoBehaviour
{
    public GameObject nucleusSegment;
    public GameObject dna;
    public Rigidbody2D centerRigidBody;
    public float radius;
    public int numberOfSegments;

    private GameObject[] _segments;

    private void Awake()
    {
        _segments = new GameObject[numberOfSegments];
    }

    private void Start()
    {
        CreateNucleus();
    }

    private void CreateNucleus()
    {
        var angleStep = 360f / numberOfSegments;
        for (var i = 0; i < numberOfSegments; i++)
        {
            var angle = angleStep * i;
            var position = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0f
            ) * radius;

            _segments[i] = Instantiate(nucleusSegment, position, Quaternion.identity);
        }

        foreach (var segment in _segments)
        {
            var rb = segment.AddComponent<Rigidbody2D>();
            segment.AddComponent<SpringJoint2D>();
            segment.AddComponent<SpringJoint2D>();
        }

        for (var i = 0; i < _segments.Length; i++)
        {
            // configure components
        }
    }
}