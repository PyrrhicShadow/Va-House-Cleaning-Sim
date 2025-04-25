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
        [SerializeField] AudioClip defaultDialogueClip;
        private string[] narration;
        private string[] narrationQueue;
        private AudioClip soundQueue;
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

        /// <summary>Begins playback of multiple lines of narration with supplied speech sound, adding line to queue if dialogue is already playing.</summary>
        /// <param name="narration">Lines to play</param>
        /// <param name="clip">Speech sound</param>
        public void PlayNarration(string[] narration, AudioClip clip)
        {
            PlayNarration(narration, clip, true);
        }

        /// <summary>Begins playback of multiple lines of narration with default speech sound.</summary>
        /// <param name="narration">Lines to play</param>
        /// <param name="queue">True to add to queue, false to cut off previously playing dialogue</param>
        public void PlayNarration(string[] narration, bool queue)
        {
            PlayNarration(narration, defaultDialogueClip, queue);
        }

        /// <summary>Begins playback of multiple lines of narration.</summary>
        /// <param name="narration">Lines to play</param>
        // /// <param name="clip">Speech sound</param>
        /// <param name="queue">True to add to queue, false to cut off previously playing dialogue</param>
        public void PlayNarration(string[] narration, AudioClip clip, bool queue)
        {
            if (/* queue && */ gameManager.narrationPlaying)
            {
                // queue next narration set 
                if (narrationQueue == null)
                {
                    soundQueue = clip;
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
                dialogueSounds.clip = clip;
                playNarration = true;
            }
        }

        /// <summary>Begins playback of multiple lines of narration with default speech sound, adding line to queue if dialogue is already playing.</summary>
        /// <param name="narration">Lines to play</param>
        public void PlayNarration(string[] narration)
        {
            PlayNarration(narration, defaultDialogueClip, true);
        }

        protected IEnumerator PlayNarrationCo()
        {
            for (int i = 0; i < narration.Length; i++)
            {
                ShowSubtitles();
                // dialogueSounds.Play(); 
                // float length = (float)narration[i].Length * readingSpeed;
                // DisplaySubtitles(narration[i]);
                // yield return new WaitForSeconds(length); 

                for (int j = 0; j < narration[i].Length; j++)
                {
                    if (j % 2 == 0)
                    {
                        dialogueSounds.Play();
                    }
                    DisplaySubtitles(narration[i][..j]);
                    yield return new WaitForSeconds(readingSpeed);
                }
                DisplaySubtitles(narration[i]);
                yield return new WaitForSeconds(readingSpeed * 25);
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
                    narrationQueue = null;
                    if (!gameManager.subtitlesOn)
                    {
                        HideSubtitles();
                    }
                }
            }
        }

        /// <summary>Shows a line of subtitles</summary>
        private void DisplaySubtitles(string line)
        {
            subtitleTextBox.text = line.Trim();
        }

        /// <summary>Adds a line of audio description to the subtitltes</summary>
        private void DisplayAudioDescription(string desc)
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

        private void ShowSubtitles()
        {
            canvas.enabled = true;
            // Debug.Log("Subtitles showing."); 
        }

        private void HideSubtitles()
        {
            canvas.enabled = false;
            // subtitleTextBox.text = string.Empty;
            // Debug.Log("Subtitles hidden"); 
        }
    }
}