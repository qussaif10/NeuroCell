using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Molecule", order = 1)]
public class Molecule : ScriptableObject
{
    public Sprite sprite;
    public MoleculeType moleculeType;
    public GameObject prefab;
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