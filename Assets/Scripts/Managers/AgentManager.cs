using System;
using System.Threading.Tasks;
using Tools;
using UnityEngine;

namespace Managers
{
    public class AgentManager : MonoBehaviour
    {
        public GameObject targetPrefab;
        private Vector2 _targetPosition;
        public Vector2 TargetPosition
        {
            set
            {
                _targetPosition = value;
                _agentController.Target.transform.position = TargetPosition;
                if (!_agentController.enabled)
                {
                    EnableController();
                }
            }
            get => _targetPosition;
        }

        public AgentController _agentController;
        public State currentState = State.Active;

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
        public void DisableController()
        {
            _agentController.enabled = false;
            _agentController.Target = null;
        }

        private void EnableController()
        {
            _agentController.Target = Instantiate(targetPrefab, _targetPosition, Quaternion.identity);
            _agentController.enabled = true;
        }
        public enum State
        {
            Idle,
            Active
        }
    }
}