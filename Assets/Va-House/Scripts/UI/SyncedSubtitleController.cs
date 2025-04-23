using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEditor;

namespace PyrrhicSilva.UI
{
    [System.Serializable]
    public class SubtitleEntry
    {
        public float timeIndex;
        public string speakerName;
        public string subtitle;
    }

    public class SyncedSubtitleController : MonoBehaviour
    {
        public bool showSubtitles = false;
        [SerializeField] TMP_Text subtitleText;
        [SerializeField] List<SubtitleEntry> subtitles = new List<SubtitleEntry>();
        private int currentSubtitleIndex = -1;
        private AudioSource _audioSource;
        public AudioSource audioSource { get { return _audioSource; } private set { _audioSource = value; } }
        public bool isPlaying { get { return _audioSource.isPlaying; } }
        public float time { get { return _audioSource.time; } set { _audioSource.time = value; } }

        void Start()
        {
            subtitleText.gameObject.SetActive(false);
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                this.gameObject.SetActive(false);
            }
        }

        void Update()
        {

            if (isPlaying)
            {
                StartCoroutine(PlaySubtitlesCo());
            }
            else
            {
                HideSubtitle();
            }
        }

        public void Play()
        {
            audioSource.Play();
            currentSubtitleIndex = -1;
        }

        public void Pause()
        {
            audioSource.Pause();
        }

        public void UnPause()
        {
            if (time != 0)
            {
                while (subtitles[currentSubtitleIndex].timeIndex < time)
                {
                    currentSubtitleIndex++;
                }
                currentSubtitleIndex--;
            }
            audioSource.UnPause();

        }

        IEnumerator PlaySubtitlesCo()
        {
            if (subtitles[currentSubtitleIndex + 1].timeIndex < time)
            {
                currentSubtitleIndex++;
                DisplaySubtitle();
            }
            yield return new WaitForEndOfFrame();
        }

        public void DisplaySubtitle()
        {
            if (!string.IsNullOrEmpty(subtitles[currentSubtitleIndex].speakerName))
            {
                subtitleText.text = $"{subtitles[currentSubtitleIndex].speakerName}: {subtitles[currentSubtitleIndex].subtitle}";
            }
            else
            {
                subtitleText.text = subtitles[currentSubtitleIndex].subtitle;
            }
            subtitleText.gameObject.SetActive(true);
        }

        public void HideSubtitle()
        {
            subtitleText.gameObject.SetActive(false);
        }

        public void LoadSubtitlesFromFile(string filePath)
        {
            subtitles = new List<SubtitleEntry>(); 
            
            string[] lines = File.ReadAllLines(filePath);
            string timePart = "";
            string subtitleText = "";
            string speakerName = "";

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("[DURATION=\"") && trimmedLine.EndsWith("\"]"))
                {
                    timePart = trimmedLine.Replace("[DURATION=\"", "").Replace("\"]", "").Trim();
                }
                else if (trimmedLine.StartsWith("[SPEAKER=\"") && trimmedLine.EndsWith("\"]"))
                {
                    speakerName = trimmedLine.Replace("[SPEAKER=\"", "").Replace("\"]", "").Trim();
                }
                else if (trimmedLine.StartsWith("[SUBTITLE=\"") && trimmedLine.EndsWith("\"]"))
                {
                    subtitleText = trimmedLine.Replace("[SUBTITLE=\"", "").Replace("\"]", "").Trim();
                }
                else if (!string.IsNullOrEmpty(trimmedLine))
                {
                    Debug.LogWarning("Unrecognized format in line: " + trimmedLine);
                }

                if (!string.IsNullOrEmpty(timePart) && !string.IsNullOrEmpty(subtitleText))
                {
                    float timeIndexInSeconds = ParseTimeToSeconds(timePart);

                    subtitles.Add(new SubtitleEntry
                    {
                        timeIndex = timeIndexInSeconds,
                        speakerName = speakerName,
                        subtitle = subtitleText,
                    });
                    Debug.Log($"Subtitle added: {timeIndexInSeconds}s - {speakerName}: {subtitleText}");


                    timePart = "";
                    subtitleText = "";
                    speakerName = "";
                }
            }

            Debug.Log("Subtitles loaded: " + subtitles.Count);
        }

        private float ParseTimeToSeconds(string time)
        {
            string[] timeParts = time.Split(':');
            if (timeParts.Length == 2)
            {
                int minutes = int.Parse(timeParts[0]);
                int seconds = int.Parse(timeParts[1]);
                return minutes * 60 + seconds;
            }
            else
            {
                Debug.LogError("Time format is incorrect: " + time);
                return 0;
            }
        }
    }
}