using System;
using System.Threading.Tasks;
using Managers;
using Tools;
using UnityEngine;

namespace LogicManagers
{
    public class EndoLogic : MonoBehaviour
    {
        private int _energyCount;
        private void Awake()
        {
            EventManager.Instance.OnRequestMolecule += OnReceiveRequest;
        }

        private void OnReceiveRequest(Molecule arg1, Region arg2, Action<Task<GameObject>> arg3)
        {
            
        }
    }
}