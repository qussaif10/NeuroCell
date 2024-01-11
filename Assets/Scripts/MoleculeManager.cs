using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour
{
    public Molecule[] moleculeTemplatesArray;
    public Dictionary<string, Molecule> moleculeTemplatesDictionary = new Dictionary<string, Molecule>();
    
    private void Start()
    {
        foreach (var moleculeTemplate in moleculeTemplatesArray)
        {
            moleculeTemplatesDictionary.Add(moleculeTemplate.moleculeType.ToString(), moleculeTemplate);
        }
    }

    public static GameObject SpawnMolecule(Molecule molecule, Region region)
    { 
        return null;
    }

    public static Region CheckMoleculeRegion(GameObject molecule)
    {
        return Region.NoRegion;
    }
}