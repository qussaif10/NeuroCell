using System;
using System.Threading.Tasks;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class OutSideLogic : MonoBehaviour
    {
        private const Region ThisRegion = Region.Outside;
        private GameObject _molecule;

        private void Awake()
        {
            EventManager.Instance.OnRequestMolecule += OnReceiveRequest;
        }
        
        private void OnReceiveRequest(Molecule molecule, Region region, Action<Task<GameObject>> task)
        {
            if (molecule.moleculeType != MoleculeType.Glucose) return;
            task?.Invoke(HandleRequest());
        }
        
        private async Task<GameObject> HandleRequest()
        {
            var molecule =MoleculeManager.Instance.InstantiateMolecule(
                MoleculeManager.Instance.moleculeTemplatesDictionary["Glucose"], ThisRegion);
            molecule.GetComponent<AgentManager>().targetPosition = RegionManager.GetRandomPositionInRegion(Region.Mitochondrion);
            return molecule;
        }
    }
}