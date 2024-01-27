using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // public GameObject ConvertMolecule(GameObject molecule, Molecule type, float delay = 0)
        // {
        //     GameObject newMolecule;
        //     var region = GetMoleculeRegion(molecule);
        //     Destroy(molecule);
        //     StartCoroutine(DelayedInstantiateMolecule(type, region, obj => { newMolecule = obj; }, delay));
        //     return newMolecule;
        // }
        //
        // public GameObject ConvertMolecule(GameObject molecule, Molecule type, Region otherRegion, float delay = 0)
        // {
        //     GameObject newMolecule;
        //     Destroy(molecule);
        //     StartCoroutine(DelayedInstantiateMolecule(type, otherRegion, obj => { newMolecule = obj; }, delay));
        //     return newMolecule;
        // }
        //
        // private IEnumerator DelayedInstantiateMolecule(Molecule type, Region region, Action<GameObject> callback, float delay)
        // {
        //     yield return new WaitForSeconds(delay);
        //     
        //     callback?.Invoke(InstantiateMolecule(type, region));
        // }
        
        public Task<GameObject> ConvertMolecule(GameObject molecule, Molecule type, float delay = 0)
        {
            var region = GetMoleculeRegion(molecule);
            Destroy(molecule);

            var tcs = new TaskCompletionSource<GameObject>();
            StartCoroutine(DelayedInstantiateMolecule(type, region, delay, tcs));
            return tcs.Task;
        }

        public Task<GameObject> ConvertMolecule(GameObject molecule, Molecule type, Region otherRegion, float delay = 0)
        {
            Destroy(molecule);

            var tcs = new TaskCompletionSource<GameObject>();
            StartCoroutine(DelayedInstantiateMolecule(type, otherRegion, delay, tcs));
            return tcs.Task;
        }

        private IEnumerator DelayedInstantiateMolecule(Molecule type, Region region, float delay, TaskCompletionSource<GameObject> tcs)
        {
            yield return new WaitForSeconds(delay);

            var newMolecule = InstantiateMolecule(type, region);
            tcs.SetResult(newMolecule);
        }
    }
}