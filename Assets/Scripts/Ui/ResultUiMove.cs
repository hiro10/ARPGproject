using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// リザルト画面でのUiの動きのクラス
/// </summary>
public class ResultUiMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Uiを動かす初期値に設定
        gameObject.transform.localPosition = new Vector3(1000f, 0f, 0f);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// リザルト画面でのスコアパネルの動き
    /// </summary>
    public void ResultScoreUYiMove()
    {
        gameObject.SetActive(true);
        gameObject.transform.DOLocalMoveX(287f,2f).SetLink(gameObject).SetEase(Ease.InOutBack);
    }
}
