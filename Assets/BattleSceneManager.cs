using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;
// 戦闘シーン（主にエネミー）の管理
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
        inBattleArea=false;

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

    private void Update()
    {
        if (allEnemyDeadCount >= 40 && !resultOn)
        {
            resultOn = true;
            StartCoroutine(ResultStart());
        }
        if(deadCount==10&& !inBossBattleArea)
        {
            clearAreaCount++;
        }
        
    }

     IEnumerator ResultStart()
    {
        slideUi.SetActive(true);
        resultScorePanel.SetActive(true);
        // プレイヤーの入力を受け付けなくする（後で修正）
        player.GetComponent<PlayerInput>().enabled=false;
        mainPanel.SetActive(false);
        resultPanel.SetActive(true);
        slideUi.GetComponent<SlideUiControl>().UiMove();
        yield return new WaitForSeconds(2);
        resultScorePanel.GetComponent<SlideUiControl>().UiMove();
        yield return new WaitForSeconds(2);
        resultButtons.SetActive(true);
    }

    public IEnumerator DeadResultStart()
    {
        slideUiDead.SetActive(true);
        // プレイヤーの入力を受け付けなくする（後で修正）
        player.GetComponent<PlayerInput>().enabled = false;
        mainPanel.SetActive(false);
        resultPanel.SetActive(true);
        slideUiDead.GetComponent<SlideUiControl>().UiMove();
        yield return new WaitForSeconds(4);
        resultButtons.SetActive(true);
    }

}
