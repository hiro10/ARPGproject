using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // プレイヤーの位置
    private Transform player;
    private NavMeshAgent navMeshAgent;

    // プレイヤーを追跡する角度
    private float trackingAngle = 180f; 

    // プレイヤーとの最大距離
    private float maxDistance = 10f;
 

    // 現在のエネミーの状態
    public State state;

    public int Hp = 1;
　 public enum State
    {
        // 待機巡回状態
        Idle,
        // プレイヤーを追う
        Run,
        // 攻撃
        Attack,
        // 死亡
        Die
    }

    void Start()
    {
        // 待機状態
        state = State.Idle;
        player = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null&&Hp>0)
        {

            // プレイヤーとの距離を計算
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Debug.Log("エネミーとプレイヤーの距離" + distanceToPlayer);
            // プレイヤーとの距離が一定以下なら攻撃モードに移行
            if (distanceToPlayer <= 2f)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.forward = directionToPlayer;
                navMeshAgent.isStopped = true;
                state = State.Attack;
                return;
            }
            else
            {
                // プレイヤーの位置を取得
                Vector3 playerDirection = player.position - transform.position;
                float angle = Vector3.Angle(transform.forward, playerDirection);
                // プレイヤーが攻撃範囲内にいるか確認

                // プレイヤーが正面90度以内にいるか確認
                if (angle <= trackingAngle / 2f && distanceToPlayer <= maxDistance)
                {
                    navMeshAgent.isStopped = false;
                    state = State.Run;
                    // プレイヤーの方向に向かって追跡
                    navMeshAgent.SetDestination(player.position);
                }
                else if (distanceToPlayer > maxDistance)
                {
                    navMeshAgent.isStopped = true;
                    state = State.Idle;

                    // 生成した範囲に戻る
                    // 何か別の行動をここで実行
                    // 例: 待機するか、ランダムな方向に移動する
                }

            }
        }
    }

    private void ResetEnemy()
    {
        state = State.Idle;
        navMeshAgent.isStopped = false;
        Hp = 1;
    }


    // エネミー死亡
    public void EnemyDie()
    {
         
        state = State.Die;
        
    }

    // 死亡時、アニメーションクリップにて非表示にする
    public void EnemyFalse()
    {
        ResetEnemy();
        this.gameObject.SetActive(false);
        
    }
   
}