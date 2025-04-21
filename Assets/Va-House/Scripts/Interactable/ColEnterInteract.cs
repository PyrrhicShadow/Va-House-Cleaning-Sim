using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class ColEnterInteract : Interactable
    {
        [SerializeField] float maxExitTime = 10;
        [SerializeField] float exitTimer;

        protected override void Update()
        {
            base.Update();
            if (!interactable)
            {
                if (exitTimer < maxExitTime)
                {
                    exitTimer = exitTimer + Time.deltaTime;
                }
                else
                {
                    gameManager.EndGame();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log("Something exited the collider.");
            if (other.CompareTag("GameController"))
            {
                // Debug.Log("The player exited the collider.");
                InteractAction();
            }
        }

        /// <summary>OnTriggerExit, performs the corresponding action depending on whether this is the start or end game trigger</summary>
        public override void InteractAction()
        {
            if (interactable)
            {
                DisableTrigger();
                exitTimer = 0;
                gameManager.LeaveHouse();

            }
        }

        public override void DisableTrigger()
        {
            interactable = false;
            col.enabled = false;
        }

        public override void EnableTrigger()
        {
            interactable = true;
            col.enabled = true;
        }
    }
}