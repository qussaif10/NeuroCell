using System;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class NucleusLogic : MonoBehaviour
    {
        private readonly MoleculeManager _moleculeManager = MoleculeManager.Instance;
        private const Region ThisRegion = Region.Nucleus;

        private void Start()
        {
            GameObject requestedMolecule;
            EventManager.Instance.OnRequestMolecule += OnReceiveRequest;
            EventManager.Instance.RequestMolecule(_moleculeManager.moleculeTemplatesDictionary["ATP"], ThisRegion,
                obj => { requestedMolecule = obj; });
        }

        private void OnReceiveRequest(Molecule molecule, Region region, Action<GameObject> obj)
        {
            
        }
    }
}