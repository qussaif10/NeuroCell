using System;
using System.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Tools
{
    public class EventManager : Singleton<EventManager>
    {
        public event Action<Molecule, Region, Action<Task<GameObject>>> OnRequestMolecule;

        public void RequestMolecule(Molecule molecule, Region region, Action<Task<GameObject>> task)
        {
            OnRequestMolecule?.Invoke(molecule, region, task);
        }
    }
}