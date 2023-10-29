using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChangeSkyMoveManager : MonoBehaviour
{
    // 変化させる雲シェーダーのマテリアル
    [SerializeField] private Material cloudMat;

    // 濃さの値
    private float smoothness=0f;
  
    // Update is called once per frame
    void Update()
    {
        // 雲の多さを減らす
        smoothness += 0.4f*Time.deltaTime;
        cloudMat.SetFloat("_CloudPawer", smoothness);
    }

    private void OnDestroy()
    {
        GameManager.Instance.isMovePlaying = false;
        cloudMat.SetFloat("_CloudPawer", 0);
    }

    /// <summary>
    /// タイムラインにイベント登録する関数
    /// </summary>
    public void StartEndMove()
    {
        // ムーボーが始まったタイミングでゲームマネージャーの値を更新
        GameManager.Instance.isMovePlaying = true;
    }
}
