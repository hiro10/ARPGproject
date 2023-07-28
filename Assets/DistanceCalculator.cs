using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// このオブジェクトからプレイヤーの距離を求める 
/// </summary>
public class DistanceCalculator : MonoBehaviour
{
    public Transform playerTransform;
    public Transform targetObject; // 距離を計算したい対象のオブジェクト
    public TextMeshProUGUI distanceText; // TextMeshProUGUIオブジェクト

    private void Update()
    {
        // ターゲットオブジェクトが設定されている場合のみ処理を行う
        if (targetObject != null)
        {
            // プレイヤー（このスクリプトがアタッチされているオブジェクト）からターゲットオブジェクトまでの距離を計算
            float distance = Vector3.Distance(playerTransform.position, targetObject.position);

            // 距離をメートルに変換して整数に変換
            int distanceInMeters = Mathf.RoundToInt(distance * 10); 

            // TextMeshProUGUIに距離を表示
            distanceText.text = distanceInMeters.ToString() + " m"; // 小数点以下2桁で表示
        }
    }
}
