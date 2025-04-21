using System.Collections;
using System.Collections.Generic;
using PyrrhicSilva.Interactable;
using UnityEngine;
using TMPro;

namespace PyrrhicSilva
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] Interact interact;
        [SerializeField] OpenAndClose frontDoor;
        [SerializeField] HomeownerController homeownerController; 
        [Header("Task objects")]
        [SerializeField] GameObject setTable;
        [SerializeField] HouseTask[] allTasks;
        [SerializeField] HouseTask[] dishesQueue;
        [SerializeField] int _taskIndex = 0;
        public int taskIndex { get { return _taskIndex; } internal set { _taskIndex = value; } }
        [SerializeField] internal int dishesIndex = 0;
        [SerializeField] HouseTask currentTask;
        [SerializeField] TMP_Text taskDisplay;

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

        }

        void OnInteract()
        {
            interact.Press();
            currentTask.ActivateTask();
        }

        void StartTasks()
        {
            setTable.SetActive(false);
            dishesQueue[1].gameObject.SetActive(false);
            currentTask.gameObject.SetActive(true);
            currentTask.ActivateTask();
        }

        public void TaskComplete()
        {
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
            currentTask.ActivateTask();
        }

        private HouseTask RandomTask()
        {
            // find a way to prevent new task from being the same as the current task
            HouseTask temp = allTasks[Random.Range(0, allTasks.Length)];

            int i = 0; 
            while (temp.Equals(currentTask) && i < 100) {
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

        public void UpdateTaskDisplay(string taskName)
        {
            taskDisplay.text = "" + taskName;
        }

        public void CloseFrontDoor()
        {
            frontDoor.InteractAction();
            StartTasks();
        }
    }
}