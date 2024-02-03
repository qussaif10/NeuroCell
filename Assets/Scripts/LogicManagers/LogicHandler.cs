using System.Collections;
using  Managers;
using MouseEvents;
using Tools;
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
            var glucose = MoleculeManager.Instance.InstantiateMolecule(
                    MoleculeManager.Instance.moleculeTemplatesDictionary["Glucose"], Region.Outside);
            
            glucose.GetComponent<AgentManager>().TargetPosition =
                RegionManager.GetRandomPositionInRegion(Region.Mitochondrion);

            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(glucose) == Region.Mitochondrion);

            var regions = new[] { Region.Nucleus, Region.GolgiIn, Region.EndoplasmicRough };
            var coroutines = new Coroutine[3];

            for (var i = 0; i < regions.Length; i++)
            {
                var atp = MoleculeManager.Instance.InstantiateMolecule(
                    MoleculeManager.Instance.moleculeTemplatesDictionary["ATP"], Region.Mitochondrion);
                atp.GetComponent<AgentManager>().TargetPosition = RegionManager.GetRandomPositionInRegion(regions[i]);
                coroutines[i] = StartCoroutine(WaitForAndUseAtp(atp, regions[i]));

                if (i == 0)
                {
                    Destroy(glucose);
                }

                yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            }

            yield return coroutines[0];
            yield return coroutines[1];
            yield return coroutines[2];

            yield return new WaitForSeconds(Random.Range(1f, 3f));
            
            var mRNA = MoleculeManager.Instance.InstantiateMolecule(
                MoleculeManager.Instance.moleculeTemplatesDictionary["mRNA"], Region.Nucleus);
            mRNA.GetComponent<AgentManager>().TargetPosition =
                RegionManager.GetRandomPositionInRegion(Region.EndoplasmicRough);


            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(mRNA) == Region.EndoplasmicRough);

            Destroy(mRNA);

            StartCoroutine(InsulinIterator());
            StartCoroutine(TriglycerideIterator());
            
            StartCoroutine(StartSimulation());
        }

        private IEnumerator InsulinIterator()
        {
            var preproinsulin = MoleculeManager.Instance.InstantiateMolecule(
                MoleculeManager.Instance.moleculeTemplatesDictionary["preproinsulin"], Region.EndoplasmicRough);

            preproinsulin.GetComponent<AgentManager>().TargetPosition =
                RegionManager.GetRandomPositionInRegion(Region.GolgiIn);

            
            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(preproinsulin) == Region.GolgiIn);

            var task1 = MoleculeManager.Instance.ConvertMolecule(preproinsulin,
                MoleculeManager.Instance.moleculeTemplatesDictionary["Insulin"], Region.GolgiOut, Random.Range(0.5f, 2f));

            yield return new WaitUntil(() => task1.IsCompleted);

            var insulin = task1.Result;
            insulin.GetComponent<AgentManager>().TargetPosition =
                RegionManager.GetRandomPositionInRegion(Region.Outside);
            
            StartCoroutine(MoleculeManager.Instance.SendToOblivion(insulin.GetComponent<Rigidbody2D>(), 50, 0.1f));
        }

        private IEnumerator TriglycerideIterator()
        {
            var triglyceride1 = MoleculeManager.Instance.InstantiateMolecule(
                MoleculeManager.Instance.moleculeTemplatesDictionary["Triglyceride"], Region.EndoplasmicSmooth);

            triglyceride1.GetComponent<AgentManager>().TargetPosition =
                RegionManager.GetRandomPositionInRegion(Region.GolgiIn);

            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(triglyceride1) == Region.GolgiIn);

            Destroy(triglyceride1);

            var triglyceride2 =
                MoleculeManager.Instance.InstantiateMolecule(
                    MoleculeManager.Instance.moleculeTemplatesDictionary["Triglyceride"], Region.GolgiOut);

            StartCoroutine(MoleculeManager.Instance.SendToOblivion(triglyceride2.GetComponent<Rigidbody2D>(), 50, 0.1f));
        }
        
        private IEnumerator WaitForAdp(GameObject adp)
        {
            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(adp) == Region.Mitochondrion);
            yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
            Destroy(adp);
        }
        
        
        private IEnumerator WaitForAndUseAtp(GameObject atp, Region targetRegion)
        {
            yield return new WaitUntil(() => RegionManager.GetRegionOfMolecule(atp) == targetRegion);

            var task = MoleculeManager.Instance.ConvertMolecule(atp,
                MoleculeManager.Instance.moleculeTemplatesDictionary["ADP"], Random.Range(0.25f, 1f));

            yield return new WaitUntil(() => task.IsCompleted);

            task.Result.GetComponent<AgentManager>().TargetPosition =
                RegionManager.GetRandomPositionInRegion(Region.Mitochondrion);
            StartCoroutine(WaitForAdp(task.Result));
        }
    }
}