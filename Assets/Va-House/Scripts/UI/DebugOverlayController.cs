using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace PyrrhicSilva
{
    public class DebugOverlayController : MonoBehaviour
    {
        [SerializeField] internal GameManager gameManager;
        [SerializeField] protected Canvas canvas; 
        [SerializeField] protected TMP_Text taskIndex;
        [SerializeField] protected TMP_Text dishesIndex;
        [SerializeField] protected TMP_Text currentTask;
        [SerializeField] protected TMP_Text podcastProgress;
        private float time;

        // Awake is called before Start 
        void Awake()
        {
            if (gameManager == null)
            {
                gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            }
        }
        void Start()
        {
            time = gameManager.podcast.time;
        }

        // Update is called once per frame
        void Update()
        {
            canvas.enabled = gameManager.debug; 
            if (gameManager.debug)
            {
                taskIndex.text = "Task index: " + gameManager.taskIndex;
                dishesIndex.text = "Dishes index: " + gameManager.dishesIndex;
                currentTask.text = "Current task: " + gameManager.currentTask.desc;
                currentTask.enabled = gameManager.currentTask.interactable;
                if (gameManager.podcast.isPlaying)
                {
                    time = gameManager.podcast.time;
                }
                podcastProgress.text = "Podcast progress: 0:" + Math.Floor(time / 60).ToString("00") + ":" + Math.Floor(time % 60).ToString("00");
            }
            else {

            }

        }
    }
}