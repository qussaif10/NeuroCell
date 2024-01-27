using System;
using System.Collections;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class MitochondrionLogic : MonoBehaviour
    {
        private readonly MoleculeManager _moleculeManager = MoleculeManager.Instance;
        private GameObject _molecule;


        private void Start()
        {
            EventManager.Instance.OnRequestMolecule += OnReceiveRequest;
            SendRequest();
        }

        private void OnReceiveRequest(Molecule molecule, Region region, Action<GameObject> obj)
        {
            if (molecule.moleculeType != MoleculeType.ATP) return;
            StartCoroutine(HandleRequest());
        }

        private void SendRequest()
        {
            EventManager.Instance.RequestMolecule(_moleculeManager.moleculeTemplatesDictionary["Glucose"],
                Region.Mitochondrion, o => { _molecule = o; });
        }

        private IEnumerator HandleRequest()
        {
            yield return new WaitUntil(() => Region.Mitochondrion == RegionManager.GetRegionOfMolecule(_molecule));
            _moleculeManager.ConvertMolecule(_molecule, _moleculeManager.moleculeTemplatesDictionary["ATP"]);
        }
    }
}