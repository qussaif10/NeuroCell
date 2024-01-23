using System;
using UnityEngine;
using UnityEngine.Serialization;

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
            EnableController();
        }

        private void DisableController()
        {
            _agentController.enabled = false;
            _agentController.Target = null;
        }

        private void EnableController()
        {
            _agentController.enabled = true;
            _agentController.Target = Instantiate(targetPrefab, targetPosition, Quaternion.identity);
        }
        private enum State
        {
            Idle,
            Active
        }
    }
}