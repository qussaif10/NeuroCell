using Managers;
using UnityEngine;

namespace LogicManagers
{
    public class MitochondrionLogic : OrganelleLogic
    {
        
        private GameObject _molecule;

        protected override void Start()
        {
            base.Start();
            _molecule = moleculeManager.InstantiateMolecule(moleculeManager.moleculeTemplatesDictionary["Glucose"], Region.Outside);
            _molecule.GetComponent<AgentManager>().targetPosition =
                RegionManager.GetRandomPositionInRegion(Region.Mitochondrion);
        }

        private void Update()
        {
            if (RegionManager.GetRegionOfMolecule(_molecule) == Region.Mitochondrion)
            {
                moleculeManager.ConvertMolecule(_molecule, moleculeManager.moleculeTemplatesDictionary["ATP"]);
            }
        }
    }
}