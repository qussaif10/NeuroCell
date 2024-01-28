using UnityEngine;

namespace Managers
{
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
        Glucose,
        mRNA,
        Hemoglobin,
        ADP,
        Cholesterol,
        Oxygen,
        CO2,
        H2O
    }
}