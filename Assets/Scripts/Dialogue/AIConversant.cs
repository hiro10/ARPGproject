using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Dialogue;

namespace ARPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] string conversantName;
        [SerializeField] PlayerController player;
        bool isTaking = false;

        private void OnTriggerEnter(Collider other)
        {
            isTaking = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (dialogue == null)
                {
                    return;
                }
                if (Input.GetKeyDown("joystick button 2") && isTaking)
                {
                    player.GetComponent<PlayerConversant>().StartDialogue(this,dialogue);
                    isTaking = false;
                }
            }
            else
            {
                return;
            }



        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                isTaking = false;
            }
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}