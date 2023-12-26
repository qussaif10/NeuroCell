using UnityEngine;

public class Mitochondrion : MonoBehaviour
{
    public GameObject MitochondrionSegment;
    public GameObject DNA;
    public int numberOfSegments;
    public float scale;

    private GameObject[] segments;

    private void Awake()
    {
        segments = new GameObject[numberOfSegments];
    }

    private void Start()
    {
        CreateMitochondrion();
    }

    private void CreateMitochondrion()
    {
        var stepAngle = Mathf.PI * 2 / numberOfSegments;

        for (var i = 0; i < segments.Length; i++)
        {
            var theta = stepAngle * i;

            var position = new Vector3(
                scale * Mathf.Cos(theta) * (Mathf.Pow(Mathf.Sin(theta), 3) + Mathf.Pow(Mathf.Cos(theta), 3)) - 1.7f,
                scale * Mathf.Sin(theta) * (Mathf.Pow(Mathf.Sin(theta), 3) + Mathf.Pow(Mathf.Cos(theta), 3)) - 2.2f,
                0);

            segments[i] = Instantiate(MitochondrionSegment, position, Quaternion.identity, transform);
        }
    }
}