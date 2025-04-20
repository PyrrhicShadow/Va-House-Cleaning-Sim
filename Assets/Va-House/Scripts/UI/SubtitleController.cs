using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PyrrhicSilva.UI
{
    public class SubtitleController : MonoBehaviour
    {
        [SerializeField] protected Canvas canvas;
        [SerializeField] protected TMP_Text subtitleTextBox;
        [SerializeField] protected TMP_Text audioDescTextBox;
        [SerializeField] protected Image textWindow;
        [SerializeField] internal float readingSpeed;
        [SerializeField] GameManager gameManager;

        private int fontSize;
        private TMP_FontAsset font;
        private Color color;
        private Color windowColor;

        /// <summary>Start is called before the first frame update</summary>
        void Start()
        {
            if (canvas == null)
            {
                canvas = gameObject.GetComponent<Canvas>();
            }

            if (canvas == null || subtitleTextBox == null || textWindow == null)
            {
                Debug.Log(this.name + " is missing its canvas, text box, or text window.");
                gameObject.SetActive(false);
            }
            else
            {
                Setup();
            }
        }

        private void Setup()
        {
            canvas.gameObject.GetComponent<GraphicRaycaster>().enabled = false;
            Hide();

            // Load subtitle settings from PlayerPrefs

            // textBox.fontSize = fontSize; 
            // textBox.font = font; 
            // textBox.color = color;

            subtitleTextBox.overrideColorTags = true;
            subtitleTextBox.enableWordWrapping = true;
            subtitleTextBox.raycastTarget = false;

            // textWindow.color = windowColor; 
            textWindow.raycastTarget = false;

        }

        /// <summary>Shows a line of subtitles</summary>
        public void DisplaySubtitles(string line)
        {
            subtitleTextBox.text = line.Trim();
        }

        /// <summary>Adds a line of audio description to the subtitltes</summary>
        public void DisplayAudioDescription(string desc)
        {
            // if (gameManager.audioDescriptionOn) { 
            //     Show();  
            //     audioDescTextBox.text += "\n" + desc.Trim(); 
            //     Debug.Log(desc); 
            //     StartCoroutine(DescPlaying());  
            // }
        }

        private IEnumerator DescPlaying()
        {
            float dir = audioDescTextBox.text.Length * readingSpeed;
            yield return new WaitForSeconds(dir);
            Hide();
        }

        public void Show()
        {
            canvas.enabled = true;
            // Debug.Log("Subtitles showing."); 
        }

        public void Hide()
        {
            canvas.enabled = false;
            subtitleTextBox.text = string.Empty;
            // Debug.Log("Subtitles hidden"); 
        }
    }
}