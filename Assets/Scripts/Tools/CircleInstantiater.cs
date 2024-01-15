using System.Collections;
using UnityEngine;

public class CircleInstantiater : MonoBehaviour
{
    public GameObject circle;
    private void Start()
    {
        StartCoroutine(InstantiateCircle());
    }

    IEnumerator InstantiateCircle()
    {
        while (true)
        {
            var randomPosition = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            var instantiatedCircle = Instantiate(circle, randomPosition, Quaternion.identity);

            var randomScale = Random.Range(0.5f, 1f);
            instantiatedCircle.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            StartCoroutine(DestroyAfterDelay(instantiatedCircle, 6f));

            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator DestroyAfterDelay(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(objectToDestroy);
    }

}