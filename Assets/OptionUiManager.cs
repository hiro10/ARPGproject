using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class OptionUiManager : MonoBehaviour
{
    // オプション画面用(DoTween)
    [SerializeField] private GameObject optionPanel;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] PauseManager pauseManager;
    // Start is called before the first frame update
    void Start()
    {
        //// オプションパネルのnullチェックと初期スケール設定
        //if (optionPanel != null)
        //{
        //    optionPanel.SetActive(false);
        //    //optionPanel.transform.localScale = Vector3.zero;
        //}
        //else
        //{
        //    return;
        //}
    }

    /// <summary>
    /// チュートリアルウインドウ処理
    /// </summary>
    public void OnClickStartOptionsButton(GameObject panels)
    {
        pauseManager.OpenMenuPanel = true;
       // pauseMenu.SetActive(false);
        //SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        panels.SetActive(true);
        // オプションウィンドウをだんだん拡大
        panels.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetUpdate(true).SetLink(gameObject);
    }

    public void OnClickCloseButton(GameObject panels)
    {
        //SoundManager.instance.PlaySE(SoundManager.SE.Close);
        // backPanel.SetActive(false);
       
        // オプションウィンドウをだんだん拡大
        panels.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).SetUpdate(true).OnComplete(ResetPanel);
        pauseMenu.SetActive(true);
    }

    private void ResetPanel()
    {
        pauseManager.OpenMenuPanel = false;
        optionPanel.SetActive(false);
       
    }

}
