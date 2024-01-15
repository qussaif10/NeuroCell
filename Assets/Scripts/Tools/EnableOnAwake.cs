using UnityEngine;

namespace Tools
{
    public class EnableOnAwake : MonoBehaviour
    {
        public GameObject[] objects;

        private void Awake()
        {
            foreach (var o in objects)
            {
                o.SetActive(true);
            }
        }
    }
}