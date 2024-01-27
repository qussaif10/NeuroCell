using Managers;
using MouseEvents;
using UnityEngine;

namespace LogicManagers
{
    public class CytosolLogic : OrganelleLogic
    {
        private GameObject _molecule;

        protected override void Start()
        {
            base.Start();
            _molecule = moleculeManager.InstantiateMolecule(moleculeManager.moleculeTemplatesDictionary["Glucose"], Region.Outside);
            _molecule.GetComponent<AgentManager>().targetPosition =
                RegionManager.GetRandomPositionInRegion(Region.Mitochondrion);
        }

        private async void Update()
        {
            if (_molecule != null && RegionManager.GetRegionOfMolecule(_molecule) == Region.Mitochondrion)
            {
                var moleculeTask = moleculeManager.ConvertMolecule(_molecule, moleculeManager.moleculeTemplatesDictionary["ATP"], 2f);
                var newMolecule = await moleculeTask;
                newMolecule.GetComponent<AgentManager>().targetPosition = RegionManager.GetRandomPositionInRegion(Region.Nucleus);
                TrackRigidbody2D.Instance.selectedRigidbody = newMolecule.GetComponent<Rigidbody2D>();
            }
        }
    }
}