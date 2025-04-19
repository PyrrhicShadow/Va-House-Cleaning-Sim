using System.Collections;
using System.Collections.Generic;
using PyrrhicSilva.Interactable;
using UnityEngine;
using TMPro;

namespace PyrrhicSilva
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] internal Interact interact;
        [SerializeField] internal OpenAndClose frontDoor;
        [Header("Task objects")]
        [SerializeField] internal GameObject setTable;
        [SerializeField] internal HouseTask[] allTasks;
        [SerializeField] internal HouseTask[] dishesQueue;
        [SerializeField] internal int taskIndex = 0;
        [SerializeField] internal int dishesIndex = 0;
        [SerializeField] internal HouseTask currentTask;
        [SerializeField] internal TMP_Text taskDisplay;

        void Awake()
        {
            StartTasks(); 
        }


        // Start is called before the first frame update
        void Start()
        {
            frontDoor.InteractAction();
            currentTask.gameObject.SetActive(true);
            currentTask.ActivateTask();
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

        void StartTasks() {
            currentTask = allTasks[0];
            setTable.SetActive(false);
            dishesQueue[1].gameObject.SetActive(false);
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
            taskDisplay.text = "Could you please " + taskName + "?";
        }
    }
}