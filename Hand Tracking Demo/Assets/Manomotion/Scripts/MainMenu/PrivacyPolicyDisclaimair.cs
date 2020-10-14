using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace ManoMotion.TermsAndServices
{
    public class PrivacyPolicyDisclaimair : MonoBehaviour
    {
        public enum AccessState
        {
            ShouldAsk = 0,
            Granted = 1,
            Denied = 2
        }

        string playerPrefsPolicy = "Policy";

        [SerializeField]
        GameObject privacyPolicyCanvas;

        bool hasUserApprovedUse;

        public event Action OnHasApprovedPrivacyPolicy;

        /// <summary>
        /// Initializes the usage disclaimer.
        /// </summary>
        public void InitializeUsageDisclaimer()
        {
            string canvasName = "PrivacyPolicyCanvas";
            if (!privacyPolicyCanvas)
            {
                privacyPolicyCanvas = GameObject.Find(canvasName);
            }
            RetrievePrivacyHistory();

            if (hasUserApprovedUse)
            {
                ClosePrivacyPolicy();
            }
            else
            {
                OpenPrivacyPolicy();
            }
        }

        /// <summary>
        /// Checks the PlayerPrefs for the record that keeps track of the user approval on privacy policy and and initializes the bool hasUserApprovedUse.
        /// </summary>
        void RetrievePrivacyHistory()
        {
            if (PlayerPrefs.HasKey(playerPrefsPolicy))
            {
                hasUserApprovedUse = PlayerPrefs.GetInt(playerPrefsPolicy) == (int)AccessState.Granted;
                if (hasUserApprovedUse)
                {
                    ApprovePrivacyPolicy();
                }
                else
                {
                    Debug.Log("User has they key but has not approved the policy");
                    Debug.LogFormat("The value of hasUserApprovedUse is {0}", hasUserApprovedUse);
                }

            }
            else
            {
                hasUserApprovedUse = false;
                PlayerPrefs.SetInt(playerPrefsPolicy, (int)AccessState.ShouldAsk);

            }
        }

        /// <summary>
        /// Opens a webpage and displays the privacy policy information
        /// </summary>
        public void NavigateToPrivacyPolicy()
        {
            string policyURL = "https://www.manomotion.com/terms-and-conditions/";
            Application.OpenURL(policyURL);
        }

        /// <summary>
        /// Approves the privacy policy conditions and sets the value in PlayerPrefs so it wont start again.
        /// </summary>
        public void ApprovePrivacyPolicy()
        {
            PlayerPrefs.SetInt(playerPrefsPolicy, (int)AccessState.Granted);
            hasUserApprovedUse = PlayerPrefs.GetInt(playerPrefsPolicy) == (int)AccessState.Granted;
            ClosePrivacyPolicy();
            if (OnHasApprovedPrivacyPolicy != null)
            {
                OnHasApprovedPrivacyPolicy();
                Debug.Log("User has Approved Privacy Policy");
            }
            else
            {
                Debug.Log("Nobody is listening");
            }
        }

        /// <summary>
        /// It stops displaying the Privacy Policy Canvas
        /// </summary>
        void ClosePrivacyPolicy()
        {
            privacyPolicyCanvas.SetActive(false);
        }

        /// <summary>
        /// Opens the privacy policy to display the visual information of the canvas regarding privacy policy.
        /// </summary>
        void OpenPrivacyPolicy()
        {
            privacyPolicyCanvas.SetActive(true);
        }
    }
}
