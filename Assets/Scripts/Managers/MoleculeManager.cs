using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour
{
    public Molecule[] moleculeTemplatesArray;
    private readonly Dictionary<string, Molecule> _moleculeTemplatesDictionary = new();
    
    private void Start()
    {
        foreach (var moleculeTemplate in moleculeTemplatesArray)
        {
            _moleculeTemplatesDictionary.Add(moleculeTemplate.moleculeType.ToString(), moleculeTemplate);
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

    public static void ConvertMolecule(GameObject molecule, Molecule type)
    {
        
    }
}