using TMPro;
using UnityEngine;

namespace Tools
{
    public class FrameRate : MonoBehaviour
    {
        public TextMeshProUGUI framerateText;
        private float deltaTime = 0.0f;

        private void Start()
        {
            Application.targetFrameRate = -1;
        }

        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            var fps = 1.0f / deltaTime;
            framerateText.text = "FPS: " + fps.ToString("F2");
        }
    }
}
