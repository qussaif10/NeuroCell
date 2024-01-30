using System;
using System.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Tools
{
    public class EventManager : Singleton<EventManager>
    {
        public event Action<Molecule, Action<Task<GameObject>>> OnGetMolecule;

        public void GetMolecule(Molecule molecule, Action<Task<GameObject>> moleculeObject)
        {
            OnGetMolecule?.Invoke(molecule, moleculeObject);
        }
    }
}