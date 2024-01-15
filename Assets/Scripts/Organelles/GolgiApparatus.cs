using UnityEngine;

namespace Organelles
{
    public class GolgiApparatus : MonoBehaviour
    {
        public GameObject golgiApparatus;
        private void Start()
        {
            CreateGolgiApparatus(); 
        }

        private void CreateGolgiApparatus()
        {
            Instantiate(golgiApparatus);
        }
    }
}