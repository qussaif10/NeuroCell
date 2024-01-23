using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class RegionManager : MonoBehaviour
    {
        public GameObject[] regionsArray;
        private static Dictionary<Region, Collider2D> regionCollidersDictionary = new();

        private void Awake()
        {
            var regionIndex = 1;
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
            foreach (var region in regionCollidersDictionary)
            {
                if (obj.GetComponent<Collider2D>().IsTouching(region.Value))
                {
                    return region.Key;
                }
            }

            return Region.NoRegion;
        }

        public static Vector2 GetRandomPositionInRegion(Region region)
        {
            if (regionCollidersDictionary.TryGetValue(region, out var collider2D))
            {
                var bounds = collider2D.bounds;
                const int maxAttempts = 100;
                for (var i = 0; i < maxAttempts; i++)
                {
                    var randomPosition = new Vector2(
                        Random.Range(bounds.min.x, bounds.max.x),
                        Random.Range(bounds.min.y, bounds.max.y)
                    );

                    if (collider2D.OverlapPoint(randomPosition))
                    {
                        // Return the position in world space
                        return randomPosition;
                    }
                }

                Debug.LogWarning("Could not find a random position in region " + region + " within " + maxAttempts +
                                 " attempts. center position is given");
                return bounds.center;
            }
            
            Debug.LogError("No collider found for region " + region);
            return Vector2.zero;
        }
    }

    public enum Region
    {
        NoRegion,
        Mitochondrion,
        Nucleus,
        Nucleolus,
        EndoplasmicRough,
        EndoplasmicSmooth,
        GolgiIn,
        GolgiOut,
        Outside
    }
}