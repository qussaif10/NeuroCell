using UnityEngine;

namespace Managers
{
    public class AgentManager : MonoBehaviour
    {
        public GameObject targetPrefab;
        
        private AgentController _agentController;
        private State _currentState = State.Idle;

        private Vector2 _targetPosition;

        private void Start()
        {
            _agentController = GetComponent<AgentController>();
        }

        private void Update()
        {
            switch (_currentState)
            {
                case State.Idle:
                    DisableController();
                    break;
                case State.Active:
                    EnableController();
                    break;
                default:
                    _agentController.enabled = _agentController.enabled;
                    break;
            }
        }

        private void DisableController()
        {
            _agentController.enabled = false;
        }

        private void EnableController()
        {
            _agentController.enabled = true;
            // _agentController.Target
        }
        private enum State
        {
            Idle,
            Active
        }
    }
}