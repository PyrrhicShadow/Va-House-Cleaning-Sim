using System.Collections;
using System.Collections.Generic;
using PyrrhicSilva.Interactable;
using UnityEngine;
using StarterAssets;
using PyrrhicSilva.UI;
using System.Linq;

namespace PyrrhicSilva
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] internal bool debug = false;
        [SerializeField] Interact interact;
        [SerializeField] OpenAndClose frontDoor;
        [SerializeField] FirstPersonController character;
        [SerializeField] internal UI.SubtitleController subtitles;
        [SerializeField] CreditsController endGameCanvas;
        [Header("Dialogue lines")]
        [SerializeField] string[] gameOpeningLines;
        [SerializeField] string[] introPodcastLines;
        [SerializeField] string[] introMusicLines;
        [SerializeField] string[] exitHouseLines;
        [SerializeField] string[] returnLines;
        public bool narrationPlaying { get; internal set; } = false;
        [SerializeField] bool _subtitlesOn = true;
        public bool subtitlesOn { get { return _subtitlesOn; } internal set { _subtitlesOn = value; } }
        [Header("Audio")]
        [SerializeField] AudioSource outdoors;
        [SerializeField] SyncedSubtitleController _podcast;
        [SerializeField] AudioSource music;
        public SyncedSubtitleController podcast { get { return _podcast; } private set { _podcast = value; } }
        [Header("Entering Exiting")]
        [SerializeField] ColExitInteract entering;
        [SerializeField] ColEnterInteract exiting;
        [Header("Wait between tasks")]
        [SerializeField] bool waitBeforeTask = false;
        [SerializeField] float minWaitTime;
        [SerializeField] float maxWaitTime;
        [Header("Task objects")]
        [SerializeField] GameObject setTable;
        [SerializeField] HouseTask[] allTasks;
        [SerializeField] HouseTask[] dishesQueue;
        [SerializeField] int _taskIndex = 0;
        public int taskIndex { get { return _taskIndex; } private set { _taskIndex = value; } }
        [SerializeField] int _dishesIndex = 0;
        public int dishesIndex { get { return _dishesIndex; } private set { _dishesIndex = value; } }
        [SerializeField] HouseTask _currentTask;
        public HouseTask currentTask { get { return _currentTask; } private set { _currentTask = value; } }

        void Awake()
        {
            currentTask = allTasks[0];
            endGameCanvas.gameObject.SetActive(false);
            Load();
        }


        // Start is called before the first frame update
        void Start()
        {
            exiting.gameObject.SetActive(false);
            frontDoor.InteractAction();
        }

        // Update is called once per frame
        void Update()
        {
            if (waitBeforeTask && !narrationPlaying)
            {
                StartCoroutine(WaitBetweenTasks());
                waitBeforeTask = false;
            }
        }

        void OnInteract()
        {
            interact.Press();
            // currentTask.ActivateTask();
        }

        internal void PlayerMovement()
        {
            if (character.enabled)
            {
                character.enabled = false;
            }
            else
            {
                character.enabled = true;
            }
        }


        public void StartTasks()
        {
            if (taskIndex == 0)
            {
                setTable.SetActive(false);
                dishesQueue[1].gameObject.SetActive(false);
                subtitles.PlayNarration(gameOpeningLines);
            }
            else
            {
                subtitles.PlayNarration(returnLines);
                if (taskIndex >= allTasks.Length)
                {
                    podcast.UnPause();
                }
            }
            currentTask.gameObject.SetActive(true);
            currentTask.ActivateTask();
        }

        public void TaskComplete()
        {
            taskIndex++;
            if (taskIndex == allTasks.Length)
            {
                subtitles.PlayNarration(introPodcastLines);
                podcast.UnPause();
            }
            NextTask();
        }

        private void NextTask()
        {
            if (taskIndex < allTasks.Length)
            {
                currentTask = allTasks[taskIndex];
            }
            else
            {
                currentTask = RandomTask();
                if (!podcast.isPlaying && !music.isPlaying)
                {
                    subtitles.PlayNarration(introMusicLines);
                    music.PlayDelayed(4f);
                }
            }
            waitBeforeTask = true;
        }

        IEnumerator WaitBetweenTasks()
        {
            if (taskIndex < allTasks.Length)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            }
            currentTask.gameObject.SetActive(true);
            currentTask.ActivateTask();
            waitBeforeTask = false;
        }

        private HouseTask RandomTask()
        {
            // find a way to prevent new task from being the same as the current task
            HouseTask temp = allTasks[Random.Range(0, allTasks.Length)];

            int i = 0;
            while (temp.Equals(currentTask) && i < 100)
            {
                temp = allTasks[Random.Range(0, allTasks.Length)];
                i++;
            }

            if (temp.chain)
            {
                if (dishesIndex > 1)
                {
                    dishesIndex = 0;
                }

                temp = dishesQueue[dishesIndex];
                setTable.SetActive(false);

                dishesIndex++;

            }
            return temp;
        }

        public void CloseFrontDoor()
        {
            outdoors.Pause();
            exiting.gameObject.SetActive(true);
            exiting.EnableTrigger();
            frontDoor.InteractAction();
            StartTasks();
        }

        public void LeaveHouse()
        {
            outdoors.UnPause();
            entering.EnableTrigger();
            if (podcast.isPlaying)
            {
                podcast.Pause();
                Save();
            }
            else if (music.isPlaying)
            {
                music.Pause();
            }

            subtitles.PlayNarration(exitHouseLines, false); // cuts off whatever dialogue that was playing before leaving 
        }

        public void EndGame()
        {
            PlayerMovement();
            endGameCanvas.gameObject.SetActive(true);
            music.Play();
            // Debug.Log("End game.");
        }

        public void Save()
        {
            if (podcast != null)
            {
                float podcastProgress = podcast.time - 5;
                if (podcastProgress < 0)
                {
                    podcastProgress = 0;
                }
                PlayerPrefs.SetFloat("podcast progress", podcastProgress);
            }
        }

        public void Load()
        {
            podcast.Play();
            if (PlayerPrefs.HasKey("podcast progress"))
            {
                podcast.time = PlayerPrefs.GetFloat("podcast progress");
            }
            podcast.Pause();
        }

        private void OnDisable()
        {
            Save();
        }

        private void OnApplicationPause()
        {
            Save();
        }

        private void OnApplicationQuit()
        {
            Save();
        }
    }
}