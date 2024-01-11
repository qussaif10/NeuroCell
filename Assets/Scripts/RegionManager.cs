using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    public GameObject[] regionsArray;
    private static Dictionary<Region, Collider2D> regionCollidersDictionary = new();

    private void Start()
    {
        int regionIndex = 0;
        foreach (var regionGameObject in regionsArray)
        {
            var collador = regionGameObject.GetComponent<Collider2D>();
            if (collador != null)
            {
                var region = (Region)regionIndex;
                regionCollidersDictionary.Add(region, collador);
            }

            regionIndex++;
        }
    }
    
    public static Region GetRegionOfMolecule(GameObject obj)
    {
        var objPosition = obj.transform.position;
        foreach (var region in regionCollidersDictionary)
        {
            if (region.Value.OverlapPoint(objPosition))
            {
                return region.Key;
            }
        }

        return Region.NoRegion;
    }
}

public enum Region
{
    NoRegion,
    Mitochondrion
}