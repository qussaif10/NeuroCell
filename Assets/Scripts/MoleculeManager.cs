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

    public static GameObject InstantiateMolecule(Molecule molecule, Region region)
    {
        return Instantiate(molecule.prefab, RegionManager.GetRandomPositionInRegion(region), Quaternion.identity);
    }

    public static Region GetMoleculeRegion(GameObject molecule)
    {
        return RegionManager.GetRegionOfMolecule(molecule);
    }
}