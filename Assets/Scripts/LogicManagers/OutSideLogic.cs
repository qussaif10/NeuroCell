using System;
using System.Collections;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class OutSideLogic : MonoBehaviour
    {
        private readonly MoleculeManager _moleculeManager = MoleculeManager.Instance;
        private void Start()
        {
            EventManager.Instance.OnRequestMolecule += OnReceiveRequest;
        }
        
        private void OnReceiveRequest(Molecule molecule, Region region, Action<GameObject> obj)
        {
            if (molecule.moleculeType != MoleculeType.Glucose) return;
            
            var instantiatedMolecule = _moleculeManager.InstantiateMolecule(_moleculeManager.moleculeTemplatesDictionary["Glucose"], Region.Outside);
            instantiatedMolecule.GetComponent<AgentManager>().targetPosition =
                RegionManager.GetRandomPositionInRegion(region);
            obj(instantiatedMolecule);
        }
        
        private IEnumerator HandleRequest(GameObject molecule)
        {
            yield return new WaitUntil(() => Region.Mitochondrion == RegionManager.GetRegionOfMolecule(molecule));
            // _moleculeManager.ConvertMolecule()
        }
    }
}