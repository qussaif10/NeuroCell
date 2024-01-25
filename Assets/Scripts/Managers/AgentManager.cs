using UnityEngine;

namespace Managers
{
    public class AgentManager : MonoBehaviour
    {
        public GameObject targetPrefab;
        public Vector2 targetPosition;

        public AgentController _agentController;
        private State _currentState = State.Active;

        private void Start()
        {
            EnableController();
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