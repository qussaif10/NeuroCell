using System;
using System.Threading.Tasks;
using Tools;
using UnityEngine;

namespace Managers
{
    public class AgentManager : MonoBehaviour
    {
        public GameObject targetPrefab;
        public Vector2 targetPosition;

        public AgentController _agentController;
        private State _currentState = State.Active;

        private void Awake()
        {
            EventManager.Instance.OnGetMolecule += OnGetMolecule;
            EnableController();
        }

        private void OnGetMolecule(Molecule molecule, Action<Task<GameObject>> moleculeObject)
        {
            if (!gameObject.CompareTag(molecule.moleculeType.ToString()))
            {
                return;
            }
            moleculeObject?.Invoke(Task.FromResult(gameObject));
        }
        private void DisableController()
        {
            _agentController.enabled = false;
            _agentController.Target = null;
        }

        private void EnableController()
        {
            _agentController.Target = Instantiate(targetPrefab, targetPosition, Quaternion.identity);
            _agentController.enabled = true;
        }
        private enum State
        {
            Idle,
            Active
        }
    }
}