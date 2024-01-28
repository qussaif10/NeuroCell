using System;
using System.Collections;
using System.Threading.Tasks;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class NucleusLogic : MonoBehaviour
    {
        private const Region ThisRegion = Region.Nucleus;
        private int _energyCount;
        private GameObject _molecule;

        private void Awake()
        {
            EventManager.Instance.OnRequestMolecule += OnReceiveRequest;
        }

        private void OnReceiveRequest(Molecule molecule, Region region, Action<Task<GameObject>> task)
        {
            
        }

        private async void Start()
        {
            var tcs = new TaskCompletionSource<GameObject>();
            
            EventManager.Instance.RequestMolecule(MoleculeManager.Instance.moleculeTemplatesDictionary["ATP"],
                ThisRegion,
                 async task => { tcs.SetResult(await task); });
            
            var requestedMolecule = await tcs.Task;
            requestedMolecule.GetComponent<AgentManager>().targetPosition = RegionManager.GetRandomPositionInRegion(ThisRegion);
            StartCoroutine(WaitUntilMoleculeReached(requestedMolecule));
        }

        private IEnumerator DelayedDestroy(GameObject destroye, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(destroye);
        }

        private IEnumerator WaitUntilMoleculeReached(GameObject malecole)
        {
            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(malecole) == ThisRegion);
            StartCoroutine(DelayedDestroy(_molecule, 1f));
            _energyCount++;
            Debug.Log(_energyCount);
        }
    }
}