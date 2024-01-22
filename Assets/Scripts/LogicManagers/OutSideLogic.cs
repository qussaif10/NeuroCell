using System.Collections;
using Managers;
using UnityEngine;

namespace LogicManagers
{
    public class OutSideLogic : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(GlucoseProduction());
        }

        private IEnumerator GlucoseProduction()
        {
            while (true)
            {
                MoleculeManager.Instance.InstantiateMolecule(MoleculeManager.Instance.moleculeTemplatesDictionary["Glucose"], Region.Outside);
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }
    }
}