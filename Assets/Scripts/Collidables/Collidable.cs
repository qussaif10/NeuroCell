using UnityEngine;

public interface ICollidable
{
    CollidableType Type { get; }    
}

public enum CollidableType
{
    Nucleus,
    Mitochondrion,
    GolgiIn,
    GolgiOut,
    Phospholipid,
    SmoothEr,
    RoughEr
}