using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Dialogue;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace ARPG.UI
{
    public class DialogueUi : MonoBehaviour
    {
        PlayerConversant playerConversant;

        [SerializeField] TextMeshProUGUI AIText;

        [SerializeField] Button nextButton;

        [SerializeField] GameObject AIResponse;

        [SerializeField] Transform choiceRoot;

        [SerializeField] GameObject choicePrefab;

        [SerializeField] Button quitButton;

        [SerializeField] float textSpeed = 0.4f;

        [SerializeField] TextMeshProUGUI conversantName;

        // Start is called before the first frame update
        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.OnConversationUpdate += UpdateUI;
            nextButton.onClick.AddListener(() => playerConversant.Next());
            quitButton.onClick.AddListener(() => playerConversant.Quit());

            UpdateUI();
        }

        void Next()
        {
            playerConversant.Next();
        }
        


        /// <summary>
        /// UiÇÃçXêVèàóù
        /// </summary>
        void UpdateUI()
        {
            AIText.text = string.Empty;
            gameObject.SetActive(playerConversant.IsActive());
            if(!playerConversant.IsActive())
            {
                return;
            }
            conversantName.text = playerConversant.GetCurrentConversantName();
            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
              
                AIText.DOText(playerConversant.GetText(), textSpeed).SetEase(Ease.Linear);
                nextButton.gameObject.SetActive(playerConversant.HasNext());
                if(playerConversant.HasNow())
                {
                    nextButton.gameObject.SetActive(true);
                    //quitButton.gameObject.SetActive(true);
                }
            }
        }

        private void BuildChoiceList()
        {
            choiceRoot.DetachChildren();
            foreach (DialogueNode choice in playerConversant.GetChoice())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComp.text = choice.GetText();
                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() =>
                {
                    playerConversant.SelectChoice(choice);
                    UpdateUI();
                });
            }
        }
    }
}
