using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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

    private void Start()
    {
        inBattleArea=false;

        spownCount = 0;

        deadCount = 0;

        allEnemyDeadCount = 0;
    }
}
