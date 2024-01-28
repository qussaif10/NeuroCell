using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public Task<GameObject> ConvertMolecule(GameObject molecule, Molecule type, float delay = 0)
        {
            var region = GetMoleculeRegion(molecule);
            var tcs = new TaskCompletionSource<GameObject>();
            StartCoroutine(DelayedInstantiateMolecule(type, molecule, region, delay, tcs));
            return tcs.Task;
        }

        public Task<GameObject> ConvertMolecule(GameObject molecule, Molecule type, Region otherRegion, float delay = 0)
        {
            var tcs = new TaskCompletionSource<GameObject>();
            StartCoroutine(DelayedInstantiateMolecule(type, molecule, otherRegion, delay, tcs));
            return tcs.Task;
        }

        private IEnumerator DelayedInstantiateMolecule(Molecule type, GameObject moleculeObject, Region region,
            float delay, TaskCompletionSource<GameObject> tcs)
        {
            yield return new WaitForSeconds(delay);

            Destroy(moleculeObject);
            var newMolecule = InstantiateMolecule(type, region);
            tcs.SetResult(newMolecule);
        }
        
        public void SendToOblivion(Rigidbody2D rb, float radius, float forceStrength)
        {
            var angle = Random.Range(0f, Mathf.PI * 2);
        
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        
            var targetPosition = direction * radius;
        
            var forceDirection = (targetPosition - rb.position).normalized;
        
            rb.AddForce(forceDirection * forceStrength, ForceMode2D.Impulse);
        }
    }
}