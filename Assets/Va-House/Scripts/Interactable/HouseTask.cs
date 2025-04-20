using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PyrrhicSilva.Interactable
{
    public class HouseTask : Interactable
    {
        [SerializeField] string desc;
        [SerializeField] internal bool messy;
        [SerializeField] internal bool chain;
        [SerializeField] internal GameObject mess;
        [SerializeField] internal GameObject tidy;
        [SerializeField] AudioSource effectSource;
        [SerializeField] AudioClip[] effects;
        [SerializeField] internal float animTime = 0.5f;
        [SerializeField] internal TMP_Text display;
        [SerializeField] int tasksTillReset = 3;
        int lastTaskIndex; 

        protected override void Awake()
        {
            base.Awake();

            if (desc.Trim() == string.Empty)
            {
                desc = "Open and close";
            }
        }

        protected virtual void Start()
        {
            if (mess == null)
            {
                Debug.Log(this.name + " is missing its messy state.");
            }
            if (tidy == null)
            {
                Debug.Log(this.name + " is missing its cleaned state.");
            }
            interactable = false; 
            repeatable = false;
            SetMessy();
        }

        protected override void Update() {
            if (!messy && !chain) {
                if (gameManager.taskIndex - lastTaskIndex > tasksTillReset) {
                    messy = true; 
                    SetMessy(); 
                }
            }
        }

        /// <summary>Opens or closes this OpenAndClose object </summary>
        public override void InteractAction()
        {
            if (interactable)
            {
                if (messy == false)
                {

                    // if (effects != null && effects[0] != null) { 
                    //     effectSource.PlayOneShot(effects[0]); 
                    //     // gameManager.subtitles.DisplayAudioDescription(desc + " opening."); 
                    // }
                    messy = true;
                }
                else
                {

                    // if (effects != null && effects[1] != null) {
                    //     effectSource.PlayOneShot(effects[1]); 
                    //     // gameManager.subtitles.DisplaySubtitles(desc + " closing."); 
                    // }
                    
                    display.enabled = false;
                    messy = false;
                    lastTaskIndex = gameManager.taskIndex; 
                    gameManager.TaskComplete();
                }
                SetMessy(); 
            }
        }

        public override void EnableTrigger()
        {
            base.EnableTrigger();
            display.enabled = true; 
        }

        public override void DisableTrigger()
        {
            base.DisableTrigger();
            display.enabled = false; 
        }

        public void ActivateTask()
        {
            interactable = true; 
            messy = true;
            SetMessy(); 
            gameManager.UpdateTaskDisplay(desc); 
            EnableTrigger(); 
        }

        void SetMessy()
        {
            tidy.gameObject.SetActive(!messy);
            mess.gameObject.SetActive(messy);
            interactable = false; 
        }
    }
}