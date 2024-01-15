using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Molecule", order = 1)]
public class Molecule : ScriptableObject
{
    public MoleculeType moleculeType;
    public GameObject prefab;
}

public enum MoleculeType
{
    ATP,
    Ribosome,
    Glucose
}