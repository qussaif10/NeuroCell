using System.Threading.Tasks;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class LogicHandler : MonoBehaviour
    {
        private async void Start()
        {
            MoleculeManager.Instance.InstantiateMolecule(
                MoleculeManager.Instance.moleculeTemplatesDictionary["UnprocessedHemoglobin"], Region.Cytosol);
            
            var tcs = new TaskCompletionSource<GameObject>();
            EventManager.Instance.GetMolecule(MoleculeManager.Instance.moleculeTemplatesDictionary["UnprocessedHemoglobin"],
                async moleculeObjectTask => tcs.SetResult(await moleculeObjectTask)
            );

            var molecule = await tcs.Task;
            Debug.Log(molecule);
        }
    }
}