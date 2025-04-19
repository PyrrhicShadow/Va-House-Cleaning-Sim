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
        [SerializeField] internal GameObject setTable; 
        [SerializeField] internal HouseTask[] allTasks;
        [SerializeField] internal HouseTask[] dishesQueue;
        internal int taskIndex = 0; 
        internal int dishesIndex = 0;
        [SerializeField] internal HouseTask currentTask;

        void Awake()
        {
            currentTask = taskCleanTable;
            setTable.SetActive(false); 
        }


        // Start is called before the first frame update
        void Start()
        {
            frontDoor.InteractAction();
            currentTask.gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnInteract()
        {
            interact.Press();
        }

        public void TaskComplete()
        {
            if (currentTask.chain)
            {
                if (dishesIndex > 2)
                {
                    dishesIndex = 0;
                }
                else
                {
                    dishesIndex++;
                }
            }
            taskIndex++; 
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
        }

        private HouseTask RandomTask()
        {
            // find a way to prevent new task from being the same as the current task
            HouseTask temp = allTasks[Random.Range(0, allTasks.Length)];

            if (temp.chain)
            {
                temp = dishesQueue[dishesIndex];
                setTable.SetActive(false); 
            }
            return temp;
        }
    }
}