using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PyrrhicSilva.Interactable
{
    public class HouseTask : Interactable
    {
        [SerializeField] string _desc;
        public string desc { get { return _desc; } private set { _desc = value; } }
        [SerializeField] internal string[] preNarration;
        [SerializeField] internal string[] postNarration;
        [SerializeField] internal bool messy;
        [SerializeField] internal bool chain;
        [SerializeField] internal bool queueActive;
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

            display.enabled = false;
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

        protected override void Update()
        {
            if (!messy && !chain)
            {
                if (gameManager.taskIndex - lastTaskIndex > tasksTillReset)
                {
                    messy = true;
                    SetMessy();
                }
            }
            if (queueActive && !gameManager.narrationPlaying && !gameManager.subtitles.playNarration)
            {
                if (messy)
                {
                    gameManager.subtitles.PlayNarration(preNarration);
                    EnableTrigger();
                    display.enabled = true; 
                }
                else
                {
                    gameManager.TaskComplete();
                    interactable = false;
                }
                queueActive = false;
            }
        }

        /// <summary>Opens or closes this OpenAndClose object </summary>
        public override void InteractAction()
        {
            if (interactable)
            {
                if (messy == false)
                {
                    messy = true;
                }
                else
                {

                    if (effects != null && effects[0] != null) {
                        effectSource.PlayOneShot(effects[0]); 
                        // gameManager.subtitles.DisplaySubtitles(desc + " closing."); 
                    }
                    queueActive = true;
                    gameManager.subtitles.PlayNarration(postNarration);
                    lastTaskIndex = gameManager.taskIndex;
                    messy = false;
                    DisableTrigger(); 
                }
                SetMessy();
            }
        }

        public override void EnableTrigger()
        {
            base.EnableTrigger();
            // display.enabled = true;
        }

        public override void DisableTrigger()
        {
            base.DisableTrigger();
            display.enabled = false;
        }

        public void ActivateTask()
        {
            messy = true;
            SetMessy();
            interactable = false;
            // gameManager.UpdateTaskDisplay(desc); 
            queueActive = true;
        }

        void SetMessy()
        {
            tidy.gameObject.SetActive(!messy);
            mess.gameObject.SetActive(messy);
        }
    }
}