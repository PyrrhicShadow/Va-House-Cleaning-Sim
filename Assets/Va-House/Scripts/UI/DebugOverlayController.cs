using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PyrrhicSilva
{
    public class DebugOverlayController : MonoBehaviour
    {
        [SerializeField] internal GameManager gameManager;
        [SerializeField] protected TMP_Text taskIndex;
        [SerializeField] protected TMP_Text dishesIndex;

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

        }
    }
}