using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAttackController : MonoBehaviour
{
    public Transform enemy; // エネミーのTransformコンポーネント
    public float attackDistance = 5f; // 攻撃範囲
    public float moveDistance = 1f; // 移動距離
    public float attackDuration = 1f; // 攻撃の移動にかかる時間

    public PlayerLockOn playerLock;
    public PlayerController playerController;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void StartAttack()
    {
        if (playerLock.target)
        {
            enemy = playerLock.target.transform;
            // エネミーとの距離を計算
            float distance = Vector3.Distance(transform.position, enemy.position);

            // 攻撃ボタンが押され、エネミーが攻撃範囲内にいる場合
            if ( distance <= attackDistance)
            {
                playerLock.target.GetComponent<EnemyController>().lockOn=true;
                // プレイヤーをエネミーの方向に向ける
                Vector3 targetLookDirection = enemy.position - transform.position;
                targetLookDirection.y = 0f; // y軸方向の回転を無効化
                transform.rotation = Quaternion.LookRotation(targetLookDirection);
                if (distance >= moveDistance)
                {

                    animator.SetFloat("Speed", -1f);

                    // プレイヤーから見たエネミーの位置の少し手前まで移動する
                    Vector3 targetPosition = enemy.position - transform.forward*1.5f;
                    targetPosition.y = transform.position.y; // y軸位置を維持する
                    transform.DOMove(targetPosition, 0.5f);
                }
                
            }
        }
    }
}
