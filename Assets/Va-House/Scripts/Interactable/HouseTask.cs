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
                    gameManager.TaskComplete();
                }
                SetMessy(); 
            }
        }

        public void ActivateTask()
        {
            interactable = true; 
            messy = true;
            SetMessy(); 
            display.enabled = true;
            gameManager.UpdateTaskDisplay(desc); 
        }

        void SetMessy()
        {
            tidy.gameObject.SetActive(!messy);
            mess.gameObject.SetActive(messy);
        }
    }
}