namespace Collidables
{
    public interface ICollidable
    {
        CollidableType Type { get; }
    }

    public enum CollidableType
    {
        Nucleus,
        Nucleolus,
        Mitochondrion,
        GolgiIn,
        GolgiOut,
        Phospholipid,
        SmoothEr,
        RoughEr
    }
}