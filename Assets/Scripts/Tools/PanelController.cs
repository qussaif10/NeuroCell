using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools
{
    public class PanelController : Singleton<PanelController>
    {
        public GameObject atPpanel;
        public GameObject adPpanel;
        public GameObject glucosePanel;
        public GameObject mRnApanel;
        public GameObject triglyceridePanel;
        public GameObject insulinPanel;
        public GameObject button;

        public void DisableAll()
        {
            atPpanel.SetActive(false);
            adPpanel.SetActive(false);
            glucosePanel.SetActive(false);
            mRnApanel.SetActive(false);
            triglyceridePanel.SetActive(false);
            insulinPanel.SetActive(false);
            button.SetActive(false);
        }

        public void EnablePanel(Rigidbody2D rb)
        {
            DisableAll();
            switch (rb.gameObject.tag)
            {
                case "preproinsulin":
                    insulinPanel.SetActive(true);
                    break;
                case "Glucose":
                    glucosePanel.SetActive(true);
                    break;
                case "Insulin":
                    insulinPanel.SetActive(true);
                    break;
                case "Triglyceride":
                    triglyceridePanel.SetActive(true);
                    break;
                case "ATP":
                    atPpanel.SetActive(true);
                    break;
                case "ADP":
                    adPpanel.SetActive(true);
                    break;
                case "mRNA":
                    mRnApanel.SetActive(true);
                    break;
            }
            button.SetActive(true);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DisableAll();
            }
        }
    }
}