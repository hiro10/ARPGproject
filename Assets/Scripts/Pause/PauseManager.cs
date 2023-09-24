using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
//�|�[�Y�@�\
//���j���[��ʂ̕\���S��
public class PauseManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] OptionUiManager optionUi;
    [SerializeField] GameObject statusImage;
    [SerializeField] GameObject menuMaskImage;
    [SerializeField] BattleSceneManager sceneManager;
    // Start is called before the first frame update

    // �X�e�[�^�X��ʁA�I�v�V������ʂ��J���Ă��邩�ǂ���
    public bool OpenMenuPanel;
    void Start()
    {
        // TODO:���W�̏C��
        menuPanel.transform.localPosition = new Vector3(-600, 0, 0);
        statusImage.transform.localPosition = new Vector3(1200, 0, 0);
        gameManager = GameManager.Instance;
        pausePanel.SetActive(false);
        menuPanel.SetActive(false);
        menuMaskImage.SetActive(false);
        OpenMenuPanel = false;

    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started&&!sceneManager.Result)
        {
            if (!gameManager.isPause)
            {
                PauseGame();    
            }
            else if (gameManager.isPause)
            {
                if(OpenMenuPanel == false)
                ResumeGame();
            }
        }
    }
    void PauseGame()
    {
        pausePanel.SetActive(true);
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // �Q�[���̎��Ԃ��~
        menuPanel.transform.DOMoveX(0, 0.4f).SetUpdate(true).SetLink(gameObject).SetEase(Ease.OutBack); 
        gameManager.isPause = true;
    }

    void ResumeGame()
    {
        menuPanel.transform.DOMoveX(-170, 0.2f).SetUpdate(true).SetLink(gameObject).OnComplete(Reset);
        
    }

    private void Reset()
    {
        pausePanel.SetActive(false);
        menuPanel.SetActive(false);
        Time.timeScale = 1f; // �Q�[���̎��Ԃ�ʏ�ɖ߂�
        gameManager.isPause = false;
    }
    public void OnClickMenuButtons()
    {
        menuMaskImage.SetActive(true);
    }
    public void ClickReturnMenuButton()
    {
        menuMaskImage.SetActive(false);
    }

    public void OnClickStatusButton()
    {
        OpenMenuPanel = true;
        //menuMaskImage.SetActive(true);
        statusImage.transform.DOLocalMoveX(400, 0.4f).SetUpdate(true).SetLink(gameObject).SetEase(Ease.OutBack);
    }
    public void OnClickStatusCloseButton()
    {
        OpenMenuPanel = false;
        //menuMaskImage.SetActive(false);
        statusImage.transform.DOLocalMoveX(1200, 0.4f).SetUpdate(true).SetLink(gameObject).SetEase(Ease.InOutBack);
    }
}
