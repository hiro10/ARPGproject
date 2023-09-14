using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPCが一定範囲内ならplayerの方向を向く処理
/// </summary>
public class NpcController : MonoBehaviour
{
    // プレイヤーの位置
    public Transform player;
    // 回転速度
    public float rotationSpeed = 5f;
    // 回転範囲
    public float detectionRange = 10f;
    // Npcの初期回転位置
    private Quaternion initialRotation;

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // オブジェクトの初期回転を保存
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // プレイヤーとNPCの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // プレイヤーが範囲内にいる場合
        if (distanceToPlayer <= detectionRange)
        {
            // プレイヤーの方を向く
            Vector3 directionToPlayer = player.position - transform.position;
            
            // Y軸成分をゼロに設定することでX軸回転をさせない
            directionToPlayer.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            
            // 補完しながらplayerの方を向く
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // 元の向きに戻る
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
