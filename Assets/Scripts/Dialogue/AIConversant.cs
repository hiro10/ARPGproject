using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Dialogue;

namespace ARPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        // 会話用ダイアログ（話す会話）
        public Dialogue dialogue = null;
        // キャラネーム
        [SerializeField] string conversantName;
        // プレイヤーの取得
        [SerializeField] PlayerController player;
        // 「話す」表示のui格納
        [SerializeField] GameObject talkUiIcon;
        // 会話可能か（trueで可能、falseで不可）
        bool isTaking = false;

        private void Start()
        {
            talkUiIcon.SetActive(false);
        }
        /// <summary>
        /// 会話可能範囲に入ったら
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            isTaking = true;
        }

        private void OnTriggerStay(Collider other)
        {
            // 会話範囲内にプレイヤーがいるなら
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
                    // 会話開始
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