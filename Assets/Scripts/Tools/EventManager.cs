using System;
using Managers;
using UnityEngine;

namespace Tools
{
    public class EventManager : Singleton<EventManager>
    {
        public event Action<Molecule> onRequestMolecule;

        public void RequestMolecule(Molecule molecule)
        {
            onRequestMolecule?.Invoke(molecule);
        }
    }
}