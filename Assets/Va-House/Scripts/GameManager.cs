using System.Collections;
using System.Collections.Generic;
using PyrrhicSilva.Interactable;
using UnityEngine;
using TMPro;
using StarterAssets;
using System.Linq;

namespace PyrrhicSilva
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] Interact interact;
        [SerializeField] OpenAndClose frontDoor;
        [SerializeField] FirstPersonController character;
        [SerializeField] internal UI.SubtitleController subtitles;
        [Header("Dialogue lines")]
        [SerializeField] string[] gameOpeningLines;
        [SerializeField] string[] introPodcastLines;
        private string[] narration;
        private string[] narrationQueue;
        public bool playNarration { get; private set; } = false;
        public bool narrationPlaying { get; private set; } = false;
        internal bool subtitlesOn = true;
        [Header("Audio")]
        [SerializeField] AudioSource outdoors; 
        [SerializeField] AudioSource podcast; 
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
        }


        // Start is called before the first frame update
        void Start()
        {
            frontDoor.InteractAction();
        }

        // Update is called once per frame
        void Update()
        {
            if (playNarration)
            {
                narrationPlaying = true;
                StartCoroutine(PlayNarrationCo());
                Debug.Log("Narration playing.");
                playNarration = false;
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
            setTable.SetActive(false);
            dishesQueue[1].gameObject.SetActive(false);
            currentTask.gameObject.SetActive(true);
            PlayNarration(gameOpeningLines);
            currentTask.ActivateTask();
        }

        public void TaskComplete()
        {
            taskIndex++;
            if (taskIndex == allTasks.Length)
            {
                PlayNarration(introPodcastLines);
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
            }
            currentTask.gameObject.SetActive(true);
            currentTask.ActivateTask();
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

        /// <summary>Begins playback of multiple lines of narration.</summary>
        public void PlayNarration(string[] narration)
        {
            if (narrationPlaying)
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
                subtitles.Show();
                playNarration = true;
            }
        }

        protected IEnumerator PlayNarrationCo()
        {
            for (int i = 0; i < narration.Length; i++)
            {
                float length = 1.5f;
                length = (float)narration[i].Length * subtitles.readingSpeed;
                subtitles.DisplaySubtitles(narration[i]);
                yield return new WaitForSeconds(length);
            }
            // subtitles.Hide();
            if (narrationQueue != null)
            {
                narration = narrationQueue;
                narrationQueue = null;
                StartCoroutine(PlayNarrationCo());
            }
            else
            {
                narrationPlaying = false;
                playNarration = false;
                Debug.Log("Narration completed.");
            }
        }
        public void CloseFrontDoor()
        {
            outdoors.Stop(); 
            frontDoor.InteractAction();
            StartTasks();
        }

    }
}