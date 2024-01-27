using System;
using Managers;
using UnityEngine;

namespace Tools
{
    public class EventManager : Singleton<EventManager>
    {
        public event Action<Molecule, Region, Action<GameObject>> OnRequestMolecule;

        public void RequestMolecule(Molecule molecule, Region region, Action<GameObject> obj)
        {
            OnRequestMolecule?.Invoke(molecule, region, obj);
        }
    }
}