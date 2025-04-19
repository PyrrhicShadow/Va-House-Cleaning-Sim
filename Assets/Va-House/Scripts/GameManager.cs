using System.Collections;
using System.Collections.Generic;
using PyrrhicSilva.Interactable;
using UnityEngine;

namespace PyrrhicSilva
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] internal Interact interact;
        [SerializeField] internal OpenAndClose frontDoor;
        [Header("Task objects")]
        [SerializeField] internal HouseTask taskCleanTable;
        [SerializeField] internal HouseTask taskMakeBed;
        [SerializeField] internal HouseTask taskStraightenRug;
        [SerializeField] internal HouseTask taskTidyCouch;
        [SerializeField] internal HouseTask taskCleanSpill;
        [SerializeField] internal HouseTask taskWashDishes;
        [SerializeField] internal HouseTask taskScrubTub;
        [SerializeField] internal HouseTask taskOrganizeBooks;
        [SerializeField] internal HouseTask[] allTasks; 
        [SerializeField] internal HouseTask[] dishesQueue;
        [SerializeField] private bool firstLoop = true;
        internal int dishesIndex = 0;
        [SerializeField] internal HouseTask currentTask;

        void Awake()
        {

            currentTask = taskCleanTable;
        }


        // Start is called before the first frame update
        void Start()
        {
            frontDoor.InteractAction();
            currentTask.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnInteract()
        {
            interact.Press();
        }

        void TaskComplete()
        {
            if (currentTask.chain)
            {
                dishesIndex++;
            }
            NextTask();
        }

        private void NextTask()
        {
            if (firstLoop)
            {

            }
            else
            {
                currentTask = RandomTask(); 
            }
        }

        private HouseTask RandomTask() {
            HouseTask temp = allTasks[Random.Range(0, allTasks.Length)];
            if (temp.chain) {
                temp = dishesQueue[dishesIndex]; 
            }
            return temp; 
        }
    }
}