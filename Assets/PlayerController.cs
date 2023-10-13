using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;
using ARPG.Dialogue;

// プレイヤーの挙動全般
public class PlayerController : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public Vector3 move;
    public Vector3 moveForward;
    [Header("移動速度")]
    [SerializeField] private float moveSpeed;
    [Header("回転割合")]
    [SerializeField] private float turnTimeRate;
    public LayerMask layerMask;
    // アクションフラグ（回避中か）
    public bool avoid ;
    // 移動処理フラグ
    [SerializeField] private bool mov = true;
    // 回転処理フラグ
    [SerializeField] private bool rot = true;
    //アニメーター
    Animator animator;

    [SerializeField] private PlayableDirector[] timeline;

    //攻撃用アクションフラグ
    public bool attack;

    // コンボ判定用フラグ
    private int comboFlgl;

    // コンボ回数
    private int coumboCount = 0;

    [SerializeField] private PlayableDirector[] attackTimeline;
    [SerializeField] GameObject wepon;
    [SerializeField] WarpConntroller warpConntroller;
    [SerializeField] PlayerLockOn playerLockOn;
    [SerializeField] PlayerAttackController playerAttack;

    // 地面に接しているか
    public bool isGrounded;
    // 現在のステート
    public PLAYER_STATE state;

    private float lockOnSpeed = 10f;
    
    // 回避時のスピード
    private float avoidSpeed = 5f;
    // 現在地
    private Vector3 nowPosition;

    [SerializeField] PlayerData playerData;
    [SerializeField] BattleSceneManager sceneManager;
    [SerializeField] PlayerConversant playerConversant;
    [SerializeField] RotationObjects rotationObjects;
    // プレイヤーが死んでいるか
    bool playerDead;

    // 覚醒中かどうか
    private bool isAwakening;
    public bool IsAwakening
    {
        get
        {
            return isAwakening;
        }
        set
        {
            isAwakening = value;
        }
    }

    public enum PLAYER_STATE
    {
        TOWN,    // 村にいるとき
        BATTLE,  // 戦闘時
        RESULT,
        ENDING
    }
    /// <summary>
    /// 開始処理
    /// </summary>
    private void Awake()
    {
        isAwakening = false;
        playerDead = false;
        isGrounded = true;
        AttackOff();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
        wepon.SetActive(false);
    }

    private void Start()
    {
        ChangeState();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if (playerData.PlayerCurrentHp <= 0 && !playerDead && state != PLAYER_STATE.RESULT)
        {// Hpが0
            PlayerDead();
        }
        //　ジャンプ力をアニメーションパラメータに設定（要修正）
        animator.SetFloat("FoolSpeed", rigidbody.velocity.y);
        animator.SetBool("isGround", isGrounded);

        //接地判定
        isGrounded = CheckGrounded();
        if (isGrounded)
        {
            animator.SetFloat("FoolSpeed", 0f);
        }
        if (attack == true || avoid == true || playerConversant.isTaking)
        {
            // 攻撃中はy軸の力を発生させない
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
            RotaionOff();
            MoveOff();
        }
        else
        {
            MoveOn();
            RotaionOn();
        }
        //  移動処理
        if (mov)
        {
            Move();
        }

        if (playerConversant.isTaking)
        {
            animator.SetFloat("Speed", 0f);
        }
        if(isAwakening&&playerData.PlayerCurrentAwake<=0)
        {
            rotationObjects.OffRotationObj();
        }
    }
   
    private void FixedUpdate()
    {
        // 町の時はロックオンできなくする
        if (state != PLAYER_STATE.TOWN)
        {
            if (playerLockOn.target != null)
            {
                animator.SetBool("LockOn", true);
            }
            else
            {
                animator.SetBool("LockOn", false);
            }
        }
        if (rot)
        { 
            // 回転
            Rotation();
        }

        if (warpConntroller.isWarp == false)
        {
            SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。FixedUpdateが好ましい。
        }
       
    }
    private void LateUpdate()
    {
        // プレイヤーの回転を保存
        Quaternion savedRotation = transform.rotation;

        if (state == PLAYER_STATE.BATTLE)
        {
            if (playerLockOn.target && move.magnitude <= 0 && isGrounded && !avoid)
            {
                // ターゲットの方向を向くための回転を計算
                Quaternion targetRotation = Quaternion.LookRotation(playerLockOn.target.transform.position - transform.position);

                // 補間処理を行って滑らかに回転させる
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lockOnSpeed * Time.fixedDeltaTime);

                // x軸回転を元に戻す
                Vector3 eulerRotation = transform.rotation.eulerAngles;
                eulerRotation.x = savedRotation.eulerAngles.x;
                transform.rotation = Quaternion.Euler(eulerRotation);
            }
            // 回避処理
            Avoid();
            DecAwakeGage();
        }
    }

    void DecAwakeGage()
    {
        if (isAwakening&&!GameManager.Instance.isPause)
        {
            playerData.PlayerCurrentAwake -= 10f*Time.deltaTime;
        }
    }
    /// <summary>
    /// シーンの状態判定
    /// </summary>
    void ChangeState()
    {
        if (GameManager.Instance.nowSceneName == "DemoScene" || GameManager.Instance.nowSceneName == "Test")
        {
            state = PLAYER_STATE.BATTLE;
        }
        else
        {
            state = PLAYER_STATE.TOWN;
        }
    }
    /// <summary>
    /// ベースの重力
    /// </summary>
    private void SetLocalGravity()
    {
        rigidbody.AddForce(new Vector3(0f,-30f,0f), ForceMode.Acceleration);
    }
    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        // メインカメラの前方方向のベクトル
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        // 方向キーの入力値とカメラの向きから移動方向を決定
        moveForward = cameraForward * move.z + Camera.main.transform.right * move.x;
        moveForward = moveForward.normalized;
        var speedw = Mathf.Abs(rigidbody.velocity.z);
        // 移動速度をアニメーターに反映
        animator.SetFloat("Speed", move.magnitude, 0.1f, Time.deltaTime);

        if (move.magnitude>0)
        {
            rigidbody.velocity = moveForward * moveSpeed * move.magnitude + new Vector3(0, rigidbody.velocity.y, 0);
        }
        // 何も入力していない
        else
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        }    
    }
    /// <summary>
    /// 回転処理
    /// </summary>
    private void Rotation()
    {
        // メインカメラの前方方向のベクトル
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        // 方向キーの入力値とカメラの向きから移動方向を決定
        moveForward = cameraForward * move.z + Camera.main.transform.right * move.x;
        moveForward = moveForward.normalized;
        if (move.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnTimeRate);
        }
        // 何も入力していない
        else
        {
            Quaternion targetRotation = transform.rotation;
            transform.rotation = targetRotation;
        }
    }
    /// <summary>
    /// InputSystem反映用
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        move = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
    }
    /// <summary>
    /// 回避の入力処理
    /// </summary>
    /// <param name="context"></param>
    public void OnAvoid(InputAction.CallbackContext context)
    {
        Debug.Log("押された");
        if(context.started&&state == PLAYER_STATE.BATTLE)
        {
            if(!avoid&&isGrounded&&!warpConntroller.isWarp )
            {
                avoid = true;
                // 移動回避
                if (move.magnitude > 0)
                {  
                    MoveOff();
                    RotaionOff();
                    animator.SetBool("Avoid",true);
                }

                //通常回避
                else
                {
                    MoveOff();
                    RotaionOff();
                    animator.SetBool("Avoid", true);                   
                }
                
            }
        }
    }
    
    /// <summary>
    /// 回避アニメーション終了時（アニメーションクリップ用）
    /// </summary>
    public void AvoidEnd()
    {
        avoid = false;
        rigidbody.velocity = Vector3.zero;
        animator.SetBool("Avoid", false);   
    }

    /// <summary>
    /// 回避処理
    /// TODP:要修正
    /// 都度斜辺を求めて正規化する
    /// </summary>
    private void Avoid()
    {
        if (avoid == true)
        {
            
            Vector3 playerForwardUp= (transform.forward + transform.up).normalized;
            Vector3 playerForwardDown = Vector3.zero;//= (transform.forward - transform.up).normalized;
            // プレイヤーから前方にレイを飛ばす
            Ray rayFront = new Ray(transform.position, transform.forward);
            float rayFrontDistance = 1f; // レイの長さ
            // レイキャストの結果を格納する変数
            RaycastHit hitFront;

            // レイをプレイヤーの足元から下向きに飛ばす
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            float distanceToGround=0f;
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Ground")
            {
                 distanceToGround = hit.distance;
            }
           
            // レイキャストの実行
            if (Physics.Raycast(rayFront, out hitFront, rayFrontDistance))
            {
                // レイが"ground"タグに当たった場合の処理
                if (hitFront.collider.CompareTag("Ground"))
                {
                    rigidbody.velocity = playerForwardUp * avoidSpeed + new Vector3(0, rigidbody.velocity.y, 0);
                }
            }
            else if (!isGrounded)
            {
                playerForwardDown = (transform.forward - new Vector3(0, distanceToGround, 0)).normalized;
                rigidbody.velocity = playerForwardDown * avoidSpeed + new Vector3(0, rigidbody.velocity.y, 0);
            }
            else
            {
                rigidbody.velocity = transform.forward * avoidSpeed + new Vector3(0, rigidbody.velocity.y, 0);
                
            }
        }
    }

    /// <summary>
    /// 攻撃の入力処理
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // 戦闘状態
        if (state == PLAYER_STATE.BATTLE)
        {
            if (context.started)
            {
                if (!attack && !avoid && warpConntroller.isWarp == false)
                {
                    if (isGrounded)
                    {
                        attack = true;
                        playerAttack.StartAttack();
                        StartCoroutine(ComboStart());
                    }
                }
            }
        }
    }
    IEnumerator ComboStart()
    {
        if (playerLockOn.target)
        {
            
            yield return new WaitForSeconds(0.3f);
            switch (coumboCount)
            {
                case 0:
                    attackTimeline[0].Play();
                    wepon.SetActive(true);
                    Debug.Log("攻撃ボタンが押された");
                    break;
            }
        }
        else
        {
            switch (coumboCount)
            {
                case 0:
                    attackTimeline[0].Play();
                    wepon.SetActive(true);
                    Debug.Log("攻撃ボタンが押された");
                    break;
            }
        }
     
    }
    public void OnCombo(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(attack&&comboFlgl==1)
            {
                comboFlgl = 2;
            }
        }
    }
    public void ComboEnable()
    {
        if(comboFlgl==0)
        {
            comboFlgl = 1;
        }
    }
    public void ComboCheak()
    {
        if(comboFlgl==2)
        {
            switch(coumboCount)
            {
                case 0:
                    attackTimeline[0].Stop();
                    attackTimeline[1].Play();
                    coumboCount = 1;
                    break;
                case 1:
                    attackTimeline[1].Stop();
                    attackTimeline[2].Play();
                    coumboCount = 2;
                    break;
                case 2:
                    attackTimeline[2].Stop();
                    attackTimeline[3].Play();
                    coumboCount = 3;
                    break;
                
            }
            comboFlgl = 0;
        }
    }
    public void AttackStop()
    {
        if(comboFlgl!=4)
        {
            comboFlgl=0;
        }
        attack = false;
        wepon.SetActive(false);
        coumboCount = 0;
    }

    /// <summary>
    ///  jumpの処理（アニメーションクリップにて実行）
    /// </summary>
    void Jump()
    {
        MoveOff();
        rigidbody.velocity = Vector3.up * 10f;
    }

    /// <summary>
    /// プレイヤーの死亡時の処理
    /// </summary>
    public void PlayerDead()
    {

        playerDead = true;
        state = PLAYER_STATE.RESULT;
        animator.SetTrigger("Dead");
        StartCoroutine(sceneManager.DeadResultStart());

    }

    /// <summary>
    /// InputSystem用　ジャンプボタン処理
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                Jump();
                //animator.SetTrigger("isJump");
            }
        }
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <returns>接地 true それ以外falseを返す</returns>
    bool CheckGrounded()
    {
        //放つ光線の初期位置と姿勢
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        //光線の距離(今回カプセルオブジェクトに設定するのでHeight/2 + 0.1以上を設定)
        var distance = 0.5f;
        //Raycastがhitするかどうかで判定レイヤーを指定することも可能
        return Physics.Raycast(ray, distance,layerMask);    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stage")
        {
            transform.position = nowPosition;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Stage")
        {
            nowPosition= transform.position;
        }
    }

    // タイムライン呼び出し用
    public void MoveOn()
    {
        mov = true;
    }
    public void MoveOff()
    {
        mov = false;
    }
    public void RotaionOn()
    {
        rot = true;
    }
    public void RotaionOff()
    {
        rot = false;
    }
    public void ActionFlugReset()
    {
        avoid = false;
    }

    public void AttackOn()
    {
        attack = true;
    }

    public void AttackOff()
    {
        attack = false;
    }
}
