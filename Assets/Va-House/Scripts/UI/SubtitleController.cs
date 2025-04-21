using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
        [SerializeField] AudioSource dialogueSounds;
        private string[] narration;
        private string[] narrationQueue;
        public bool playNarration { get; private set; } = false;
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

        void Update()
        {
            if (playNarration)
            {
                gameManager.narrationPlaying = true;
                StartCoroutine(PlayNarrationCo());
                Debug.Log("Narration playing.");
                playNarration = false;
            }
        }

        private void Setup()
        {
            canvas.gameObject.GetComponent<GraphicRaycaster>().enabled = false;
            HideSubtitles();

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

        /// <summary>Begins playback of multiple lines of narration with supplied speech sound.</summary>
        public void PlayNarration(string[] narration, AudioClip clip)
        {
            dialogueSounds.clip = clip;
            PlayNarration(narration);
        }

        /// <summary>Begins playback of multiple lines of narration.</summary>
        public void PlayNarration(string[] narration)
        {
            if (gameManager.narrationPlaying)
            {
                // queue next narration set 
                if (narrationQueue == null)
                {
                    narrationQueue = narration;
                }
                else
                {
                    for (int i = 0; i < narration.Length; i++)
                    {
                        narrationQueue.Append(narration[i]);
                    }
                }
                Debug.Log("Narration already playing.");
            }
            else
            {
                this.narration = narration;
                playNarration = true;
            }
        }

        protected IEnumerator PlayNarrationCo()
        {
            int soundGap = 3; 
            ShowSubtitles();
            for (int i = 0; i < narration.Length; i++)
            {
                if (i % soundGap == 0 && i < narration.Length - 2*soundGap)
                {
                    dialogueSounds.Play();
                }
                float length = 1.5f;
                length = (float)narration[i].Length * readingSpeed;
                DisplaySubtitles(narration[i]);
                yield return new WaitForSeconds(length);
                ShowSubtitles();
                dialogueSounds.Stop();
            }
            if (narrationQueue != null)
            {
                narration = narrationQueue;
                narrationQueue = null;
                StartCoroutine(PlayNarrationCo());
            }
            else
            {
                gameManager.narrationPlaying = false;
                playNarration = false;
                Debug.Log("Narration completed.");
                yield return new WaitForSeconds(2f);
                if (!playNarration && !gameManager.narrationPlaying)
                {
                    HideSubtitles();
                }
            }
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
            HideSubtitles();
        }

        public void ShowSubtitles()
        {
            canvas.enabled = true;
            // Debug.Log("Subtitles showing."); 
        }

        public void HideSubtitles()
        {
            canvas.enabled = false;
            // subtitleTextBox.text = string.Empty;
            // Debug.Log("Subtitles hidden"); 
        }
    }
}