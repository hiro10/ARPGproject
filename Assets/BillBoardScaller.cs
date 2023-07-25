using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardScaller : MonoBehaviour
{
    // プレイヤーの位置
    [SerializeField] Transform player;

    // プレイヤーとの距離
    // 最小
    private float minDistance = 5f;
    // 最大
    private float maxDistance = 20f;

    // サイズ
    // 最小
    private float minSize = 0.1f;
    // 最大
    private float maxSize = 0.2f;

    // 実際のサイズ
    private float distance;

    private float scallFactor;

    Renderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<Renderer>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //プレイヤーとオブジェクト（ビルビルボード）の距離の計算
        distance = Vector3.Distance(transform.position, player.transform.position);

        // 距離に応じて大きさを変更
        scallFactor = Mathf.Clamp((distance - minDistance) / (maxDistance - minDistance), 0f, 1f);

        float size = Mathf.Lerp(minSize, maxSize, scallFactor);
        transform.localScale = new Vector3(size, size, size);

        if(maxDistance<distance)
        {
            // SetActiveより軽い
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }


}
