using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    // 点滅スピード
    [SerializeField] private float speed = 1.0f;

    // 点滅させたいUI格納
    [SerializeField] private TextMeshProUGUI tapText;

    // 点滅頻度
    private float time;

    // 点滅解除後に表示させるUI
    [SerializeField] GameObject StartMenu;

    // メニューボタン格納用
    [SerializeField] Button[] MenmuButton = new Button[4];

    // オプション画面用(DoTween)
    [SerializeField] private GameObject optionPanel;

    // オプション画面用(DoTween)
    [SerializeField] private GameObject creditPanel;

    // 背景パネル用
    [SerializeField] private GameObject backPanel;


    // Start is called before the first frame update
    void Start()
    {
        backPanel.SetActive(false);
        if (StartMenu != null)
        {
            StartMenu.SetActive(false);
        }
        // オプションパネルのnullチェックと初期スケール設定
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
            optionPanel.transform.localScale = Vector3.zero;
        }
        else
        {
            return;
        }
        // クレジットパネルのnullチェックと初期スケール設定
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
            creditPanel.transform.localScale = Vector3.zero;
        }
        else
        {
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        tapText.color = GetAlphaColor(tapText.color);

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(TapText());
        }
    }

    public void OnTitleButoon()
    {
        SceneManager.LoadSceneAsync("LoadScene");
    }

    /// <summary>
    /// Alpha値を更新してColorを返す点滅処理
    /// </summary>
    /// <param name="color"></param>
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }

    /// <summary>
    /// 「タップ」を押した時のコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator TapText()
    {
        // 点滅速度の上昇
        speed = 4f;
        // 1s待つ
        yield return new WaitForSeconds(1);
        // タップオブジェクトを非表示に
        tapText.gameObject.SetActive(false);
        // スタートメニューを表示
        StartMenu.SetActive(true);
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed)
        {
            StartCoroutine(TapText());
        }
    }


    /// <summary>
    /// チュートリアルウインドウ処理
    /// </summary>
    public void OnClickStartOptionsButton(GameObject panels)
    {
        backPanel.SetActive(true);
        //SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        panels.SetActive(true);
        // オプションウィンドウをだんだん拡大
        panels.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OnClickCloseButton(GameObject panels)
    {
        //SoundManager.instance.PlaySE(SoundManager.SE.Close);
        backPanel.SetActive(false);
        // オプションウィンドウをだんだん拡大
        panels.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(ResetPanel);

    }

    private void ResetPanel()
    {
        backPanel.SetActive(false);
        creditPanel.SetActive(false);
        optionPanel.SetActive(false);
    }
}
