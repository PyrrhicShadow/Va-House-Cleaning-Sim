using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace PyrrhicSilva
{
    public class CreditsController : MonoBehaviour
    {
        // [SerializeField] float creditsRunTime;
        [SerializeField] SplashController endGameCanvas;
        [SerializeField] Animator animator;
        [SerializeField] float animationTime;
        [SerializeField] string[] headers;
        [SerializeField] string[] bodies;
        [SerializeField] protected Image textWindow;
        [SerializeField] protected TMP_Text headerTextBox;
        [SerializeField] protected TMP_Text bodyTextBox;
        [SerializeField] internal float creditsSpeed;
        private bool playCredits;

        void Start()
        {
            headerTextBox.enabled = false;
            bodyTextBox.enabled = false;
            playCredits = true;
        }

        void Update()
        {
            if (playCredits)
            {
                StartCoroutine(PlayCreditsCo());
                Debug.Log("Credits playing.");
                playCredits = false;
            }
        }

        void SetUp()
        {
            textWindow.raycastTarget = false;
        }

        protected IEnumerator PlayCreditsCo()
        {
            yield return new WaitForSeconds(creditsSpeed);
            for (int i = 0; i < headers.Length; i++)
            {
                yield return new WaitForSeconds(animationTime);

                DisplayHeader(headers[i]);
                DisplayBody(bodies[i]);
                headerTextBox.enabled = true;
                bodyTextBox.enabled = true;

                yield return new WaitForSeconds(creditsSpeed);

                // animator.Play("fadeInOut"); 
                headerTextBox.enabled = false;
                bodyTextBox.enabled = false;
            }
            endGameCanvas.StartGame();
        }

        /// <summary>Shows the header line</summary>
        private void DisplayHeader(string line)
        {
            headerTextBox.text = line.Trim();
        }
        /// <summary>Shows the body line</summary>
        private void DisplayBody(string line)
        {
            bodyTextBox.text = line.Trim(); 
        }
    }
}