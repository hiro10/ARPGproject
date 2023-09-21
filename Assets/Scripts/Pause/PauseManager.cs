using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
//ポーズ機能
//メニュー画面の表示全般
public class PauseManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] OptionUiManager optionUi;
    [SerializeField] GameObject statusImage;
    [SerializeField] GameObject menuMaskImage;
    // Start is called before the first frame update
    void Start()
    {
        // TODO:座標の修正
        menuPanel.transform.localPosition = new Vector3(-600, 0, 0);
        statusImage.transform.localPosition = new Vector3(1200, 0, 0);
        gameManager = GameManager.Instance;
        pausePanel.SetActive(false);
        menuPanel.SetActive(false);
        menuMaskImage.SetActive(false);


    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!gameManager.isPause)
            {
                PauseGame();    
            }
            else if (gameManager.isPause)
            {
                ResumeGame();
            }
        }
    }
    void PauseGame()
    {
        pausePanel.SetActive(true);
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // ゲームの時間を停止
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
        Time.timeScale = 1f; // ゲームの時間を通常に戻す
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
        //menuMaskImage.SetActive(true);
        statusImage.transform.DOLocalMoveX(400, 0.4f).SetUpdate(true).SetLink(gameObject).SetEase(Ease.OutBack);
    }
    public void OnClickStatusCloseButton()
    {
        //menuMaskImage.SetActive(false);
        statusImage.transform.DOLocalMoveX(1200, 0.4f).SetUpdate(true).SetLink(gameObject).SetEase(Ease.InOutBack);
    }
}
