using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeckController : MonoBehaviour
{
    public Transform enemy; // エネミーのTransformコンポーネント
    public Transform neckBone; // 首のTransformコンポーネント
    public float detectionRadius = 10f; // 検知範囲半径
    public float maxAngle = 45f; // プレイヤーから見た最大角度
    [SerializeField] PlayerLockOn playerLock;
    private Quaternion originalRotation; // 初期回転の保存

    private void Start()
    {
        originalRotation = neckBone.localRotation;
    }

    private void FixedUpdate()
    {
        if (playerLock.target!=null)
        {
            enemy = playerLock.target.transform;
            // プレイヤーとエネミーの距離を計算
            float distance = Vector3.Distance(transform.position, enemy.position);

            if (distance <= detectionRadius)
            {
                // プレイヤーからエネミーへの方向ベクトルを計算
                Vector3 direction = enemy.position - transform.position;
                direction.y = 0f; // y軸方向の回転を無効化

                // プレイヤーから見たエネミーの角度を計算
                float angle = Vector3.Angle(transform.forward, direction);

                if (angle <= maxAngle)
                {
                    // 首の回転をエネミーの方向に補完的に変更
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    neckBone.rotation = Quaternion.Slerp(originalRotation, targetRotation, 0.5f);
                }
                else
                {
                    // 初期回転に戻す
                    neckBone.localRotation = originalRotation;
                }
            }
            else
            {
                // 初期回転に戻す
                neckBone.localRotation = originalRotation;
            }
        }
        else
        {
            // 初期回転に戻す
            neckBone.localRotation = originalRotation;
        }
    }
    //public Transform enemy; // エネミーのTransformコンポーネント
    //public float detectionRadius = 10f; // 検知範囲半径
    ////public float maxAngle = 360f; // プレイヤーから見た最大角度
    //[SerializeField] PlayerLockOn playerLock;
    //private void FixedUpdate()
    //{
    //    if (playerLock.target)
    //    {
    //        enemy = playerLock.target.transform;
    //        // プレイヤーとエネミーの距離を計算
    //        float distance = Vector3.Distance(transform.position, enemy.position);

    //        if (distance <= detectionRadius)
    //        {
    //            // プレイヤーからエネミーへの方向ベクトルを計算
    //            Vector3 direction = enemy.position - transform.position;
    //           // direction.y = 0f; // y軸方向の回転を無効化

    //            // プレイヤーから見たエネミーの角度を計算
    //            //float angle = Vector3.Angle(transform.forward, direction);

    //           // if (angle <= maxAngle)
    //            {
    //                // プレイヤーの方向をエネミーの方向に向ける
    //                transform.LookAt(enemy);
    //            }
    //        }
    //    }
    //}

    //private void TargetIn()
    //{
    //    enemy = playerLock.target.transform;
    //}
}
