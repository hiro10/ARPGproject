using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;

public class CreditWindowManager : MonoBehaviour
{
    // クレジット画面用(DoTween)
    [SerializeField] private GameObject creditPanel;
    // Start is called before the first frame update
    void Start()
    {
        // クレジットパネルのnullチェックと初期スケール設定
      //  if (creditPanel != null)
        {
            //creditPanel.SetActive(false);
            creditPanel.transform.localScale = Vector3.zero;
        }
      //  else
        {
        //    return;
        }
    }

    /// <summary>
    /// チュートリアルウインドウ処理
    /// </summary>
    public void OnClickStartOptionsButton(GameObject panels)
    {
       // backPanel.SetActive(true);
        //SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        panels.SetActive(true);
        // オプションウィンドウをだんだん拡大
        panels.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OnClickCloseButton(GameObject panels)
    {
        //SoundManager.instance.PlaySE(SoundManager.SE.Close);
        //backPanel.SetActive(false);
        // オプションウィンドウをだんだん拡大
        panels.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(ResetPanel);

    }

    private void ResetPanel()
    {
     
        creditPanel.SetActive(false);
    }
}
