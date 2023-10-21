using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Dialogue;

namespace ARPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        // ��b�p�_�C�A���O�i�b����b�j
        public Dialogue dialogue = null;
        // �L�����l�[��
        [SerializeField] string conversantName;
        // �v���C���[�̎擾
        [SerializeField] PlayerController player;
        // �u�b���v�\����ui�i�[
        [SerializeField] GameObject talkUiIcon;
        // ��b�\���itrue�ŉ\�Afalse�ŕs�j
        bool isTaking = false;

        private void Start()
        {
            talkUiIcon.SetActive(false);
        }
        /// <summary>
        /// ��b�\�͈͂ɓ�������
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            isTaking = true;
        }

        private void OnTriggerStay(Collider other)
        {
            // ��b�͈͓��Ƀv���C���[������Ȃ�
            if (other.gameObject.tag == "Player")
            {
                if (dialogue == null)
                {
                    return;
                }
                if (isTaking)
                {
                    talkUiIcon.SetActive(true);
                }
                if (Input.GetKeyDown("joystick button 2") && isTaking)
                {
                    talkUiIcon.SetActive(false);
                    // ��b�J�n
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
                talkUiIcon.SetActive(false);
                isTaking = false;
            }
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}