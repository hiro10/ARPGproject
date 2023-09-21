using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMark : MonoBehaviour
{
    //=============================================================================
    //     変数
    //=============================================================================
    // プレイヤーの情報
    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private Transform player = null;

    // カメラ情報
    [SerializeField, Tooltip("プレイヤーを映すカメラ")]
    private Transform Camera = null;

    // ターゲット
    [SerializeField, Tooltip("追いかけるターゲット")]
    public List<Transform> targets; // ターゲットのリスト

    private Transform closestTarget; // 最も距離が近いターゲット
    [SerializeField] GameObject arrowPearent;
    //=============================================================================
    //     プロパティ
    //=============================================================================
    public Transform SetPlayer { get { return player; } set { player = value; } }
    public Transform SetCamera { get { return Camera; } set { Camera = value; } }
   // public Transform SetTarget { get { return Target; } set { Target = value; } }

    //=============================================================================
    //     アップデート
    //=============================================================================
    void Update()
    {
        TurnAroundDirectionTarget();
    }

    //=============================================================================
    //     矢印を回転させる
    //=============================================================================
    private void TurnAroundDirectionTarget()
    {
        float closestDistance = float.MaxValue; // 最も距離が近いターゲットまでの距離を初期化

        targets.RemoveAll(obj => obj.GetComponent<EnemySpawner>().inPlayer == true);

        if(targets.Count<=0)
        {
            Destroy(arrowPearent);
        }

        foreach (Transform target in targets)
        {
            // ターゲットとプレイヤーの位置を取得
            Vector3 targetPosition = target.position;
            Vector3 playerPosition = player.position;

            // ターゲットとプレイヤーの距離を計算
            float distance = Vector3.Distance(targetPosition, playerPosition);

            // 最も距離が近いターゲットを見つけた場合、そのターゲットを記録
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }


        // プレイヤーからターゲットまでのベクトルを計算
        Vector3 Direction = (closestTarget.position - player.transform.position).normalized;

        // 求めた方向への回転量を求める
        Quaternion RotationalVolume = Quaternion.LookRotation(Direction, Vector3.up);

        // カメラ情報を元に回転量の補正
        Quaternion CorrectionVolume = Quaternion.FromToRotation(Camera.transform.forward, Vector3.forward);

        Vector3 vec = (RotationalVolume * CorrectionVolume).eulerAngles;

        transform.rotation = Quaternion.Euler(0, vec.y, 0);
    }
}