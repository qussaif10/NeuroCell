using Managers;
using UnityEngine;

namespace LogicManagers
{
    public class OrganelleLogic : MonoBehaviour
    {
         protected virtual int EnergyCount { get; set; }
         protected MoleculeManager moleculeManager;
         
         protected virtual void Start()
        {
            moleculeManager = MoleculeManager.Instance;
        }
    }
}