using UnityEngine;

public class RegionManager : MonoBehaviour
{
    public Collider2D[] regionColliders;

    public Collider2D GetCollider(Region region)
    {
        int index = (int)region;
        if (index >= 0 && index < regionColliders.Length)
        {
            return regionColliders[index];
        }
        else
        {
            Debug.LogError("Region index out of range: " + index);
            return null;
        }
    }
}

public enum Region
{
    Mitochondrion
}