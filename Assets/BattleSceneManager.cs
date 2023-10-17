using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
// 戦闘シーンの管理
public class BattleSceneManager : MonoBehaviour
{
    // 専用シーンの各状況判断用変数

    // バトルエリア内か?
    private bool inBattleArea;
    // 各バトルエリアのエネミーの生成数
    private int spownCount;
    // 各バトルエリアのエネミーの生成数
    private int deadCount;
    // エネミーの総撃破数
    private int allEnemyDeadCount;
    // 制圧エリア数
    private int clearAreaCount;
    // バトルエリア内か?
    private bool inBossBattleArea;
    // スタートしたときの時間
    private float startTime;
    // 経過時間を格納
    private int elapsedTimeInSeconds;
    // リザルト中かの判定
    private bool resultOn;

    public bool Result
    {
        get
        {
            return resultOn;
        }
        set
        {
            resultOn = value;
        }
    }
    [SerializeField] GameObject resultPanel;
    [SerializeField] GameObject slideUi;
    [SerializeField] GameObject slideUiDead;
    [SerializeField] GameObject resultScorePanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject resultButtons;
    // リザルト画面でのスコアテキスト
    [SerializeField] TextMeshProUGUI resultScoreText;
    // リザルト画面での時間テキスト
    [SerializeField] TextMeshProUGUI resultTimeText;
    //プロパティ
    // バトルエリア
    public bool InBattleArea
    {
        get
        {
            return inBattleArea;
        }
        set
        {
            inBattleArea = value;
        }
    }
    // ボスのバトルエリアか？
    public bool InBossBattleArea
    {
        get
        {
            return inBossBattleArea;
        }
        set
        {
            inBossBattleArea = value;
        }
    }

    // 各バトルエリアのエネミーの生成数
    public int SpownCount
    {
        get
        {
            return spownCount;
        }
        set
        {
            spownCount = value;
        }
    }

    // 各バトルエリアのエネミーの生成数
    public int DeadCount
    {
        get
        {
            return deadCount;
        }
        set
        {
            deadCount = value;
        }
    }

    // 各バトルエリアのエネミーの生成数
    public int AllEnemyDeadCount
    {
        get
        {
            return allEnemyDeadCount;
        }
        set
        {
            allEnemyDeadCount = value;
        }
    }
    public int ClearAreaCount
    {
        get
        {
            return clearAreaCount;
        }
        set
        {
            clearAreaCount = value;
        }
    }

    private void Start()
    {
        // ゲームが開始した時点の時間を記録
        startTime = Time.time;

        inBattleArea =false;

        spownCount = 0;

        deadCount = 0;

        allEnemyDeadCount = 0;

        clearAreaCount = 0;

        resultOn = false;

        resultPanel.SetActive(false);

        resultButtons.SetActive(false);

        slideUi.SetActive(false);

        resultScorePanel.SetActive(false);

        slideUiDead.SetActive(false);
    }

     private async void Update()
    {
        // 現在の時間と開始時間の差分を計算し、秒単位に変換して整数に変換
        elapsedTimeInSeconds = Mathf.FloorToInt(Time.time - startTime);

        if (allEnemyDeadCount >= 40 && !resultOn)
        {
            StartResult();
        }
        if(deadCount==10&& !inBossBattleArea)
        {
            clearAreaCount++;
        }
        
    }
    private async void StartResult()
    {
        resultOn = true;

        // ここでResultStartメソッドを呼び出し、非同期待機する
        await ResultStart();

        // ResultStartメソッドが完了した後の処理をここに追加できます
    }

   　public async void StartDeadResult()
    {
        // ここでResultStartメソッドを呼び出し、非同期待機する
        await DeadResultStart();
    }

    /// <summary>
    /// クリア時のリザルト
    /// </summary>
    /// <returns></returns>
    private async UniTask ResultStart()
    {
      slideUi.SetActive(true);
        resultScorePanel.SetActive(true);
        // スコア表記
        resultScoreText.text = Score().ToString();
        resultTimeText.text = elapsedTimeInSeconds.ToString() + "秒";
        // プレイヤーの入力を受け付けなくする（後で修正）
        player.GetComponent<PlayerInput>().enabled = false;
        mainPanel.SetActive(false);
        resultPanel.SetActive(true);
        slideUi.GetComponent<SlideUiControl>().UiMove();
        await UniTask.Delay(2000); // 2秒待機
        resultScorePanel.GetComponent<ResultUiMove>().ResultScoreUYiMove();
        await UniTask.Delay(2000); // 2秒待機
        resultButtons.SetActive(true);
    }

    /// <summary>
    /// 死亡時のリザルト
    /// </summary>
    /// <returns></returns>
    private async UniTask DeadResultStart()
    {
        slideUiDead.SetActive(true);
        // プレイヤーの入力を受け付けなくする（後で修正）
        player.GetComponent<PlayerInput>().enabled = false;
        mainPanel.SetActive(false);
        resultPanel.SetActive(true);
        slideUiDead.GetComponent<SlideUiControl>().UiMove();
        await UniTask.Delay(4000); // 4秒待機
        resultButtons.SetActive(true);
    }
    /// <summary>
    /// スコアを整数で返す関数
    /// </summary>
    /// <returns></returns>
    private int Score()
    {
        // TODO:撃破数で判定できるように
        int score = 4000 - elapsedTimeInSeconds;
        if (score < 0)
        {
            score = 0;
        }
        return score;
        
    }
}
