using System;
using System.Threading.Tasks;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class GolgiLogic : MonoBehaviour
    {
        private int _energyCount;
        private void Awake()
        {
            EventManager.Instance.OnRequestMolecule += OnReceiveRequest;
        }

        private void OnReceiveRequest(Molecule molecule, Region region, Action<Task<GameObject>> task)
        {
            switch (molecule.moleculeType)
            {
                case MoleculeType.Hemoglobin:
                    break;
                case MoleculeType.Cholesterol:
                    break;
            }
        }
    }
}