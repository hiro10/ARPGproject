using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    // プレイヤーの位置
    private Transform player;
    private NavMeshAgent navMeshAgent;

    // プレイヤーを追跡する角度
    private float trackingAngle = 180f; 

    // プレイヤーとの最大距離
    private float maxDistance = 10f;

    [SerializeField] GameObject findPlayerEffect;
    // 現在のエネミーの状態
    public State state;

    

    Animator animator;

    float distanceToPlayer;

    private int maxHp;
    // 現在のHp
    private int currentHp;
    //Sliderを入れる
    public Slider slider;

    // スクリプタブルオブジェクトでステータス管理
    public EnemyStateSo enemyState;
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
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        maxHp = enemyState.maxHealth;
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = maxHp;
        // 待機状態
        Idle();
       
    }

    void Update()
    {
        Debug.Log("currentHp" + currentHp);
        if (player != null)
        {
            if (currentHp <= 0)
            {
                Die();
            }


            // プレイヤーとの距離を計算
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
           // Debug.Log("エネミーとプレイヤーの距離" + distanceToPlayer);

            // プレイヤーとの距離が一定以下なら攻撃モードに移行
            if (distanceToPlayer <= 2f)
            {
                Attack();
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
                    Run();
                }
                else if (distanceToPlayer > maxDistance)
                {
                    Idle();
                    // 生成した範囲に戻る
                    // 何か別の行動をここで実行
                    // 例: 待機するか、ランダムな方向に移動する
                }

            }
        }
        
    }

    private void ResetEnemy()
    {
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = maxHp;
        state = State.Idle;
        navMeshAgent.isStopped = false;
        findPlayerEffect.SetActive(false);
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

    // 待機状態
    private void Idle()
    {
        // 状態を待機
        state = State.Idle;
        // 待機アニメーション
        animator.SetTrigger("Idle");
        // オーラを消す
        findPlayerEffect.SetActive(false);
        // ナビメッシュを止める
        navMeshAgent.isStopped = true;
    }

    // プレイヤーを追いかける処理
    private void Run()
    {
        state = State.Run;
        animator.SetTrigger("Run");
        findPlayerEffect.SetActive(true);
        navMeshAgent.isStopped = false;
        
        // プレイヤーの方向に向かって追跡
        navMeshAgent.SetDestination(player.position);
    }

    // 攻撃状態
    private void Attack()
    {
        state = State.Attack;
        animator.SetTrigger("Attack");
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        findPlayerEffect.SetActive(true);
        transform.forward = directionToPlayer;
        navMeshAgent.isStopped = true;
        
    }
    // 死亡状態
    private void Die()
    {
        state = State.Die;
        animator.SetTrigger("Die");
    }
   
    public int CurrentHp()
    {
        return currentHp;
    }

    public void SetCurrentHp(int damage)
    {
        currentHp -= damage;
    }

    public int MaxHp()
    {
        return maxHp;
    }
}