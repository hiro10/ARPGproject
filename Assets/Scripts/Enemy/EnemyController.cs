using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//エネミーの動き
public class EnemyController : MonoBehaviour
{
    // プレイヤーの位置
    private Transform player;
    // ナビメッシュ
    private NavMeshAgent navMeshAgent;
    // プレイヤーを追跡する角度
    private float trackingAngle = 180f; 
    // プレイヤーとの最大距離
    private float maxDistance = 10f;
    // プレイヤー発見時に赤くなるオブジェクト
    [SerializeField] GameObject findPlayerEffect;
    // 現在のエネミーの状態
    public State state;
    // アニメーター
    Animator animator;
    // プレイヤーとの距離
    float distanceToPlayer;
    // 最大Hp　　
    private int maxHp;
    // 現在のHp
    private int currentHp;
    // Hp用Sliderを入れる
    public Slider slider;
    // 吹っ飛ばすオブジェクトのRigidbody
    private Rigidbody rb;
    // スクリプタブルオブジェクトでステータス管理
    public EnemyStateSo enemyState;
    // シーンマネージャー
    BattleSceneManager sceneManager;
    //
    public Collider enemyCollider;

    // プレイヤーからロックオンされているか？
    public bool lockOn;

    //エネミーからプレイヤーへの攻撃
    [SerializeField] EnemyToPlayerDamageManager damageManager;

    // 攻撃のクールダウン時間（秒）
    private float attackCooldown = 2.0f; 
    // 毎秒更新の攻撃時間のスパン
    public float attackTimer = 0.0f;

    public enum State
    {
        // 待機巡回状態
        Idle,
        // プレイヤーを追う
        Run,
        // 攻撃
        Attack,
        // 死亡
        Die,
    }

    /// <summary>
    /// 開始処理
    /// 各状態判定用変数の初期化、取得
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        sceneManager = GameObject.FindWithTag("BattleSceneManager").GetComponent<BattleSceneManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        maxHp = enemyState.maxHealth;
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = maxHp;
        // 待機状態
        Idle(false);
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<Collider>();
        lockOn = false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (player != null)
        {
            // Hpが0以下なら死亡
            if (currentHp <= 0)
            {
              
                Die();
                //nemyDie();
            }

            else
            {
                // プレイヤーとの距離を計算
                distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // プレイヤーとの距離が一定以下なら攻撃モードに移行
                if (distanceToPlayer <= 2f)
                {
                    attackTimer += Time.deltaTime;
                    // 攻撃モーション用の待機モーションに切り替え
                    animator.SetBool("BattleIdle", true);
                    // 滑り防止でナビメッシュを止める
                    navMeshAgent.isStopped = true;
                    // 攻撃待機時間（隙）
                    if (attackTimer >= attackCooldown)
                    {
                        animator.SetBool("BattleIdle", false);
                        Attack();
                        attackTimer = 0.0f; // タイマーをリセット
                    }
                   
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
                        if (!lockOn)
                        {
                            Run();
                        }
                        else
                        {
                            Idle(true);
                        }
                    }
                    else if (distanceToPlayer > maxDistance)
                    {
                        Idle(false);
                        // 生成した範囲に戻る
                        // 何か別の行動をここで実行
                        // 例: 待機するか、ランダムな方向に移動する
                    }

                }
            }
        }
        
    }

    /// <summary>
    /// 再初期化処理
    /// </summary>
    private void ResetEnemy()
    {
        rb.isKinematic = true;
        navMeshAgent.enabled = true;
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = maxHp;
        // 状態を待機に
        state = State.Idle;
        // エフェクトを非表示に
        findPlayerEffect.SetActive(false);
        enemyCollider.enabled = true;
        lockOn = false;
    }


    // エネミー死亡
    public void EnemyDie()
    {
        navMeshAgent.enabled = false;
        state = State.Die;
        sceneManager.DeadCount++;
        sceneManager.AllEnemyDeadCount++;
    }

    // 死亡時、アニメーションクリップにて非表示にする
    public void EnemyFalse()
    {
        ResetEnemy();
        this.gameObject.SetActive(false);
        
    }

    // 待機状態
    private void Idle(bool isBattle)
    {
        // 状態を待機
        state = State.Idle;
        if (state == State.Idle)
        {
            // 待機アニメーション
            animator.SetTrigger("Idle");
            if (!isBattle)
            {
                // オーラを消す
                findPlayerEffect.SetActive(false);
            }
            // ナビメッシュを止める
            navMeshAgent.isStopped = true;
        }
    }

    // プレイヤーを追いかける処理
    private void Run()
    {
        animator.SetBool("BattleIdle", false);
        state = State.Run;
        if (state == State.Run)
        {
            animator.SetTrigger("Run");
            findPlayerEffect.SetActive(true);
            navMeshAgent.isStopped = false;

            // プレイヤーの方向に向かって追跡
            navMeshAgent.SetDestination(player.position);
        }
    }

    // 攻撃状態の処理
    // 
    private void Attack()
    {
        // 攻撃状態に変更
        state = State.Attack;
        if (state == State.Attack)
        {   
            animator.SetTrigger("Attack");
            // 剣の当たり判定を有効に
            damageManager.SwordCollision(true);
            // エネミーからプレイやーへの単位ベクトルを求める
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            
            findPlayerEffect.SetActive(true);
            transform.forward = directionToPlayer;
        }
    }

    public void AttackEnd()
    {
        damageManager.SwordCollision(false);
    }
    // 死亡状態
    private void Die()
    {
        state = State.Die;
        if (state == State.Die)
        {
            navMeshAgent.enabled = false;
            findPlayerEffect.SetActive(false);
            rb.isKinematic = false;
            // 吹っ飛ばす方向
            Vector3 forceDirection = -transform.forward;
            // 吹っ飛ばす処理
            rb.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
            rb.AddForce(forceDirection, ForceMode.Impulse);

            enemyCollider.enabled = false;
            animator.SetTrigger("Die");
           
        }
    }

    // ランダムなアクションの開始
    private void StartRandomAction()
    {
        // ランダムに0から1の間の値を取得し、攻撃か待機を選択
        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= 0.2f) // 0.2（20%）以下なら攻撃
        {
           
            animator.SetTrigger("Attack"); // 攻撃アニメーションを再生
        }
        else
        {
            // 待機を選択
            Idle(true);
            // 待機時間を設定し、待機アニメーションが終了したら再度ランダムなアクションを開始
            float waitTime = Random.Range(1f, 3f);
            Invoke("StartRandomAction", waitTime);
        }
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