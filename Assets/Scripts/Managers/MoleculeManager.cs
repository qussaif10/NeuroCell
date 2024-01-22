using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Managers
{
    public class MoleculeManager : Singleton<MoleculeManager>
    {
        public Molecule[] moleculeTemplatesArray;
        public Dictionary<string, Molecule> moleculeTemplatesDictionary = new();

        protected override void Awake()
        {
            base.Awake();
            
            foreach (var moleculeTemplate in moleculeTemplatesArray)
            {
                moleculeTemplatesDictionary.Add(moleculeTemplate.moleculeType.ToString(), moleculeTemplate);
            }
        }

        public GameObject InstantiateMolecule(Molecule molecule, Region region)
        {
            return Instantiate(molecule.prefab, RegionManager.GetRandomPositionInRegion(region), Quaternion.identity);
        }

        public Region GetMoleculeRegion(GameObject molecule)
        {
            return RegionManager.GetRegionOfMolecule(molecule);
        }

        public void ConvertMolecule(GameObject molecule, Molecule type, float delay)
        {
            var region = GetMoleculeRegion(molecule);
            Destroy(molecule);
            StartCoroutine(DelayedInstantiateMolecule(type, region, delay));
        }
        
        public void ConvertMolecule(GameObject molecule, Molecule type, float delay, Region otherRegion)
        {
            Destroy(molecule);
            StartCoroutine(DelayedInstantiateMolecule(type, otherRegion, delay));
        }
        
        private IEnumerator DelayedInstantiateMolecule(Molecule type, Region region, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            InstantiateMolecule(type, region);
        }
    }
}