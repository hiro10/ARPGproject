using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class VillageSceneManager : MonoBehaviour
{
    private bool goBattle;
    [SerializeField] Renderer gate;
    [SerializeField] UiMaker mainIcon;
    [SerializeField] GameObject gateObj;
    [SerializeField] DistanceCalculator distanceCalculator;
    [SerializeField] SlideUiControl slideUi;
    [SerializeField] GameObject villageNameText;
    // ゲームオーバースライドUI
    [SerializeField] GameObject slideUiGameOver;
    // ゲームクリアスライドUI
    [SerializeField] GameObject slideUiGameClear;
    // プレイヤー
    [SerializeField] GameObject player;
    // ゲームオーバー時の背景
    [SerializeField] GameObject gameOverPanel;
    // リザルトボタン
    [SerializeField] GameObject resultButtons;
    // ゲームオーバーテキスト
    [SerializeField] TextMeshProUGUI gameOverText;
    // ゲームオーバーテキスト
    [SerializeField] TextMeshProUGUI gameClearText;
    // 空模様
    [SerializeField] List<GameObject> sky = new List<GameObject>();

    // リザルト中かの判定
    private bool resultOn;
    Image image;
    Color color;
    void Start()
    {
        if (GameManager.Instance.isGameClear == false)
        {
            sky[0].SetActive(true);

            sky[1].SetActive(false);
            SoundManager.instance.PlayBGM(SoundManager.BGM.Town);
        }
        else
        {
            sky[0].SetActive(false);

            sky[1].SetActive(true);

            SoundManager.instance.PlayBGM(SoundManager.BGM.TownClear);
        }
        slideUi.UiMove();
        goBattle = false;
        villageNameText.SetActive(false);
        slideUiGameOver.SetActive(false);
        slideUiGameClear.SetActive(false);

        image = gameOverPanel.GetComponent<Image>();
        color = image.color;
        color.a = 0f;
        gameOverPanel.SetActive(false);
        resultButtons.SetActive(false);
        gameOverText.alpha = 0f;
        gameClearText.alpha = 0f;
    }
    public void GotoBattleScene()
    {
        distanceCalculator.targetObject = gateObj.transform;
        mainIcon.targets[0] = gateObj.transform;
        goBattle = true;
        gate.material.color = Color.green;
    }

    public bool GetgoBattle()
    {
        return goBattle;
    }

    public void OpenName()
    {
        villageNameText.SetActive(true);
    }

    public async void StartGameOverResult()
    {
        // ここでResultStartメソッドを呼び出し、非同期待機する
        await GameOverResult();
    }

    public async void StartGameClearResult()
    {
        // ここでResultStartメソッドを呼び出し、非同期待機する
        await GameClearResult();
    }

    /// <summary>
    /// ゲームオーバー時のリザルト
    /// </summary>
    /// <returns></returns>
    private async UniTask GameOverResult()
    {
        // プレイヤーの入力を受け付けなくする（後で修正）
        player.GetComponent<PlayerInput>().enabled = false;

        GameManager.Instance.isGameOver = true;
        gameOverPanel.SetActive(true);
        image.DOFade(1.0f, 2f);
        await UniTask.Delay(1000); // 1秒待機
        gameOverText.DOFade(1.0f, 2f);
        await UniTask.Delay(4000); // 4秒待機
        gameOverText.DOFade(0.0f, 2f);
        await UniTask.Delay(1000); // 4秒待機
        slideUiGameOver.SetActive(true);
       
        slideUiGameOver.GetComponent<SlideUiControl>().UiMove();
        await UniTask.Delay(4000); // 4秒待機
        resultButtons.SetActive(true);
    }

    private async UniTask GameClearResult()
    {
        // プレイヤーの入力を受け付けなくする（後で修正）
        player.GetComponent<PlayerInput>().enabled = false;

        gameOverPanel.SetActive(true);
        image.DOFade(1.0f, 2f);
        await UniTask.Delay(1000); // 1秒待機
        gameClearText.DOFade(1.0f, 2f);
        await UniTask.Delay(4000); // 4秒待機
        gameClearText.DOFade(0.0f, 2f);
        await UniTask.Delay(1000); // 4秒待機
        slideUiGameClear.SetActive(true);

        slideUiGameClear.GetComponent<SlideUiControl>().UiMove();
        await UniTask.Delay(4000); // 4秒待機
        resultButtons.SetActive(true);
    }
}
