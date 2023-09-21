using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 目的地を示す矢印の表示非表示処理
public class ArrowExistManager : MonoBehaviour
{
    // 矢印オブジェクト
    GameObject arrow;
    // バトルシーンのマネージャー
    [SerializeField] BattleSceneManager sceneManager;

    private void Start()
    {
        arrow = transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isMovePlaying|| sceneManager.InBattleArea)
        {
            arrow.SetActive(false);
        }
        else if (!GameManager.Instance.isMovePlaying|| !sceneManager.InBattleArea)
        {
            arrow.SetActive(true);
        }
        
    }
}
