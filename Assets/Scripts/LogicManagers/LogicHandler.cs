using System.Collections;
using Managers;
using UnityEngine;

namespace LogicManagers
{
    public class LogicHandler : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(StartSimulation());
        }

        private IEnumerator StartSimulation()
        {
            var glucose = MoleculeManager.Instance.InstantiateMolecule(MoleculeManager.Instance.moleculeTemplatesDictionary["Glucose"], Region.Outside);
            glucose.GetComponent<AgentManager>().TargetPosition = RegionManager.GetRandomPositionInRegion(Region.Mitochondrion);
            
            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(glucose) == Region.Mitochondrion);

            var regions = new[] { Region.Nucleus, Region.GolgiIn, Region.EndoplasmicRough };
            var coroutines = new Coroutine[3];
            
            for (var i = 0; i < regions.Length; i++)
            {
                var atp = MoleculeManager.Instance.InstantiateMolecule(
                    MoleculeManager.Instance.moleculeTemplatesDictionary["ATP"], Region.Mitochondrion);
                atp.GetComponent<AgentManager>().TargetPosition = RegionManager.GetRandomPositionInRegion(regions[i]);
                coroutines[i] = StartCoroutine(WaitForAndUseAtp(atp, regions[i]));
            }

            yield return coroutines[0];
            yield return coroutines[1];
            yield return coroutines[2];
        }

        private IEnumerator WaitForAndUseAtp(GameObject atp, Region targetRegion)
        {
            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(atp) == targetRegion);
            var task = MoleculeManager.Instance.ConvertMolecule(atp,
                MoleculeManager.Instance.moleculeTemplatesDictionary["ADP"]);
            yield return new WaitUntil(() => task.IsCompleted);
            task.Result.GetComponent<AgentManager>().TargetPosition =
                RegionManager.GetRandomPositionInRegion(Region.Mitochondrion);
        }
    }
}