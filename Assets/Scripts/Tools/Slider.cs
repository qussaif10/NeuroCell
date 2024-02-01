using System;
using UnityEngine;

namespace Tools
{
    public class Slider : MonoBehaviour
    {
        public UnityEngine.UI.Slider slider;
        private bool _toggle;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Toggle the _toggle state
                _toggle = !_toggle;

                // Set Time.timeScale and slider.value based on the _toggle state
                Time.timeScale = _toggle ? 0 : 1;
                slider.value = Time.timeScale; // This will set the slider to 0 or 1 depending on the _toggle state
            }
            else
            {
                // Continuously update Time.timeScale to match the slider value
                // This is outside the if statement to ensure Time.timeScale is updated based on the slider's position
                Time.timeScale = slider.value;
            }
        }
    }
}