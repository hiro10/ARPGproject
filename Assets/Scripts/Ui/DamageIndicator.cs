using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ダメージを受けた時にキャンバスにダメージ数値を出すクラス
public class DamageIndicator : MonoBehaviour
{
    // ダメージテキストプレハブ
    public TextMeshProUGUI damageTextPrefab;
    // キャンバス
    public Transform canvasTransform;
    // 表示時間
    private float displayDuration = 0.5f;
  
    /// <summary>
    /// ダメージをキャンバスに表示する関数
    /// </summary>
    /// <param name="position">表示位置</param>
    /// <param name="damage">ダメージ数値</param>
    public void ShowDamageIndicator(Vector3 position, int damage)
    {
        // エネミーの攻撃が当たった場所にダメージ数値を表示
        TextMeshProUGUI damageText = Instantiate(damageTextPrefab, canvasTransform);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        damageText.transform.position = screenPosition;

        // ダメージ数値を設定
        damageText.text = damage.ToString();

        // dotweenでアニメーションを入れたい
        // 数秒後にダメージ数値を消去
        Destroy(damageText.gameObject, displayDuration);
    }
}
