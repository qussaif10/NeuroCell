using System;
using System.Threading.Tasks;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class MitochondrionLogic : MonoBehaviour
    {
        private const Region ThisRegion = Region.Mitochondrion;
        private GameObject _molecule;

        private void Awake()
        {
            EventManager.Instance.OnRequestMolecule += OnReceiveRequest;
        }

        private void OnReceiveRequest(Molecule molecule, Region region, Action<Task<GameObject>> obj)
        {
            if (molecule.moleculeType != MoleculeType.ATP) return;
            SendRequest();
            obj?.Invoke(HandleRequest());
        }

        private async void SendRequest()
        {
            var tcs = new TaskCompletionSource<GameObject>();
            
            EventManager.Instance.RequestMolecule(MoleculeManager.Instance.moleculeTemplatesDictionary["Glucose"],
                ThisRegion, async task => { tcs.SetResult(await task); });
            
            _molecule = await tcs.Task;
            
        }

        private async Task<GameObject> HandleRequest()
        {
            while (RegionManager.GetRegionOfMolecule(_molecule) != ThisRegion)
            {
                await Task.Delay(10);
            }
            
            if (ThisRegion == RegionManager.GetRegionOfMolecule(_molecule))
            {
                return await MoleculeManager.Instance.ConvertMolecule(_molecule, MoleculeManager.Instance.moleculeTemplatesDictionary["ATP"], 1);
            }

            return null;
        }
    }
}