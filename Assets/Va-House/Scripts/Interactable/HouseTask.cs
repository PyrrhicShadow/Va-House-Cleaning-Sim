using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class HouseTask : Interactable
    {
        [SerializeField] string desc;
        [SerializeField] internal bool open;
        [SerializeField] internal bool chain;
        [SerializeField] internal GameObject mess; 
        [SerializeField] internal GameObject tidy; 
        [SerializeField] AudioSource effectSource;
        [SerializeField] AudioClip[] effects;
        [SerializeField] internal float animTime = 0.5f;

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
            if (tidy == null) {
                Debug.Log(this.name + " is missing its cleaned state.");
            }
            open = false;
            repeatable = false; 
        }

        /// <summary>Opens or closes this OpenAndClose object </summary>
        public override void InteractAction()
        {
            if (interactable)
            {
                if (open == false)
                {
                    tidy.SetActive(true); 
                    mess.SetActive(false); 

                    // if (effects != null && effects[0] != null) { 
                    //     effectSource.PlayOneShot(effects[0]); 
                    //     // gameManager.subtitles.DisplayAudioDescription(desc + " opening."); 
                    // }
                }
                else
                {
                    if (open == true)
                    {
                        tidy.SetActive(false); 
                        mess.SetActive(true); 
                        // if (effects != null && effects[1] != null) {
                        //     effectSource.PlayOneShot(effects[1]); 
                        //     // gameManager.subtitles.DisplaySubtitles(desc + " closing."); 
                        // }
                    }
                }
            }
        }
    }
}