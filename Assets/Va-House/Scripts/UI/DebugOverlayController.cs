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
        [SerializeField] protected TMP_Text taskIndex;
        [SerializeField] protected TMP_Text dishesIndex;
        [SerializeField] protected TMP_Text currentTask; 
        [SerializeField] protected TMP_Text podcastProgress; 

        // Awake is called before Start 
        void Awake()
        {
            if (gameManager == null)
            {
                gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            }
        }

        // Update is called once per frame
        void Update()
        {

            taskIndex.text = "Task index: " + gameManager.taskIndex;
            dishesIndex.text = "Dishes index: " + gameManager.dishesIndex;
            currentTask.text = "Current task: " + gameManager.currentTask.desc; 
            currentTask.enabled = gameManager.currentTask.interactable; 
            podcastProgress.enabled = gameManager.podcast.isPlaying;
            if (gameManager.podcast.isPlaying) { 
                float time = gameManager.podcast.time; 
                podcastProgress.text = "Podcast progress: " + Math.Floor(time / 60) + ":" + time % 60; 
            }

        }
    }
}