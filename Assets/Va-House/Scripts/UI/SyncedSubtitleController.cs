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
        [SerializeField] GameManager gameManager;
        [SerializeField] bool showSubtitles = false;
        [SerializeField] TMP_Text subtitleText;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] List<SubtitleEntry> subtitles = new List<SubtitleEntry>();
        private int currentSubtitleIndex = 0;
        public AudioSource audioSource { get { return _audioSource; } private set { _audioSource = value; } }
        public bool isPlaying { get { return _audioSource.isPlaying; } }
        public float time { get { return _audioSource.time; } set { _audioSource.time = value; } }

        void Start()
        {
            subtitleText.gameObject.SetActive(false);
            if (audioSource == null)
            {
                audioSource = gameObject.GetComponent<AudioSource>();
            }
            if (gameManager == null)
            {
                gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            }
        }

        void Update()
        {
            if (isPlaying && currentSubtitleIndex + 1 < subtitles.Count)
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
            currentSubtitleIndex = 0;
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
                if (currentSubtitleIndex != 0)
                {
                    currentSubtitleIndex--;
                }
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
            yield return new WaitForSeconds(0.1f);
        }

        public void DisplaySubtitle()
        {
            if (!string.IsNullOrEmpty(subtitles[currentSubtitleIndex].speakerName))
            {
                if (currentSubtitleIndex > 0)
                {
                    subtitleText.text = $"{subtitles[currentSubtitleIndex - 1].speakerName}: {subtitles[currentSubtitleIndex - 1].subtitle}<br>";
                }
                else
                {
                    subtitleText.text = string.Empty;
                }
                subtitleText.text += $"{subtitles[currentSubtitleIndex].speakerName}: {subtitles[currentSubtitleIndex].subtitle}";
            }
            else
            {
                subtitleText.text = subtitles[currentSubtitleIndex].subtitle;
            }
            subtitleText.gameObject.SetActive(gameManager.subtitlesOn);
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