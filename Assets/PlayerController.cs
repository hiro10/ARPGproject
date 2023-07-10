using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public Vector3 move;
    public Vector3 moveForward;
    [Header("移動速度")]
    [SerializeField] private float moveSpeed;
    [Header("回転割合")]
    [SerializeField] private float turnTimeRate;

    // アクションフラグ（回避中か）
    [SerializeField]private bool avoid ;
    // 移動処理フラグ
    [SerializeField] private bool mov = true;
    // 回転処理フラグ
    [SerializeField] private bool rot = true;

    Animator animator;

    [SerializeField] private PlayableDirector[] timeline;

    //攻撃用アクションフラグ
    public bool attack;

    // コンボ判定用フラグ
    private int comboFlgl;

    // コンボ回数
    private int coumboCount=0;

    [SerializeField] private PlayableDirector[] attackTimeline;

    [SerializeField] GameObject wepon;

    // スティック角度
    private float degree;

    private bool isGrounded;

    [SerializeField] WarpConntroller warpConntroller;
    [SerializeField] PlayerLockOn playerLockOn;

    public PLAYER_STATE state;
    public enum PLAYER_STATE
    {
        TOWN,    // 村にいるとき
        BATTLE,  // 戦闘時
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Awake()
    {

        isGrounded = true;
        AttackOff();
        // cameraController = Camera.main.GetComponent<CameraController>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
        wepon.SetActive(false);
    }

    private void Start()
    {

        //if (GameManager.Instance.nowSceneName == "DemoScene")
        //{
        //    state = PLAYER_STATE.BATTLE;
        //}
        //else
        //{
        //    state = PLAYER_STATE.TOWN;
        //}
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //　ジャンプ力をアニメーションパラメータに設定（要修正）
        animator.SetFloat("FoolSpeed", rigidbody.velocity.y);
        animator.SetBool("isGround", isGrounded);
        Debug.Log("isGrounded" + isGrounded);
        SticeAngle();

        //接地判定
        isGrounded = CheckGrounded();
        if(isGrounded)
        {
            mov=true;
        }
      //  Debug.Log("isGrounded" + isGrounded);
        if (mov)
        {
            Move();
        }
        else
        {
            if (avoid == true )
            {
                rigidbody.AddForce(-transform.forward * 1000f, ForceMode.Acceleration);
            }
        }

    }

    private void FixedUpdate()
    {
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

    private void SetLocalGravity()
    {
        rigidbody.AddForce(new Vector3(0f,-15f,0f), ForceMode.Acceleration);
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
      
        if (avoid)
        {
            if (move.magnitude > 0)
            {
                rigidbody.AddForce(moveForward * 5f, ForceMode.VelocityChange);
            }
        }


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

    public void OnAvoid(InputAction.CallbackContext context)
    {
        Debug.Log("押された");
        if(context.started)
        {
            if(!avoid)
            {
                // 移動回避
                if(move.magnitude>0)
                {
                    timeline[0].Play();
                   // MoveOff();
                    RotaionOff();
                   
                }

                //通常回避
                else
                {
                    timeline[1].Play();
                    MoveOff();
                    RotaionOff();
                }
                avoid = true;
            }
        }
    }

    void SticeAngle()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
       // if (state == PLAYER_STATE.BATTLE)
        {
            if (context.started)
            {
                if (!attack && !avoid && warpConntroller.isWarp == false)
                {
                    attack = true;

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

        //animator.SetFloat("jumpPower",0);
        //放つ光線の初期位置と姿勢
        var ray = new Ray(transform.position + Vector3.up * 0.01f, Vector3.down);
        //光線の距離(今回カプセルオブジェクトに設定するのでHeight/2 + 0.1以上を設定)
        var distance = 0.5f;
        //Raycastがhitするかどうかで判定レイヤーを指定することも可能
        return Physics.Raycast(ray, distance);

        
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
