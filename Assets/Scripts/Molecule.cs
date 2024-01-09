using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Molecule", order = 1)]
public class Molecule : ScriptableObject
{
    public Sprite sprite;
    public MoleculeType type;
}

public enum MoleculeType
{
    DNA,
    RNA,
    ATP,
    Protein,
    Lipid,
    Carbohydrate
}