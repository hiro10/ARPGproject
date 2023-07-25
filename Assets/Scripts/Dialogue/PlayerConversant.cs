using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace ARPG.Dialogue
{
    // �_�C�A���O���R���o�[�g����X�N���v�g
    public class PlayerConversant : MonoBehaviour
    {
        // npc
        AIConversant currentConversant;
        Dialogue currentDialogue;
        // ���݂̃m�[�h
        DialogueNode currentNode = null;
        bool isChoosing = false;
        public bool isTaking = false;
        public event Action OnConversationUpdate;

        [SerializeField] string playerName;
        private void Start()
        {
            isTaking = false;
        }
    
        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            currentConversant = newConversant;
            isTaking = true;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            OnConversationUpdate();
        }

        /// <summary>
        /// �I������
        /// </summary>
        public void Quit()
        {
            
            currentDialogue = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
            isTaking = false;
            currentConversant = null;
            OnConversationUpdate();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if(currentNode == null)
            {
                return "";
            }

            return currentNode.GetText();
        }

        public string GetCurrentConversantName()
        {
            if(isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetName();
            }
        }

        public IEnumerable<DialogueNode> GetChoice()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        /// <summary>
        /// ���̃m�[�h
        /// </summary>
        public void Next()
        {
            if(HasNext()==false||HasNow()==true)
            {
                Quit();
                return;
            }

            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                OnConversationUpdate();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            OnConversationUpdate();
        }

        /// <summary>
        /// ���̃m�[�h�����邩�̔���
        /// </summary>
        /// <returns></returns>
        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        /// <summary>
        /// ���̃m�[�h���Ōォ
        /// </summary>
        /// <returns></returns>
        public bool HasNow()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() == 0;
        }

        private void TriggerEnterAction()
        {
            if(currentNode != null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }
        private void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if(action == "")
            {
                return;
            }

            foreach(DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }
    }
}
