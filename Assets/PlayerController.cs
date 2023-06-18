using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Vector3 move;
    private Vector3 moveForward;
    [Header("移動速度")]
    [SerializeField] private float moveSpeed;
    [Header("回転割合")]
    [SerializeField] private float turnTimeRate;

    private CameraController cameraController;

    // アクションフラグ（回避中か）
    [SerializeField]private bool avoid = false;
    // 移動処理フラグ
    [SerializeField] private bool mov = true;
    // 回転処理フラグ
    [SerializeField] private bool rot = true;

    Animator animator;

    [SerializeField] private PlayableDirector[] timeline;

    //攻撃用アクションフラグ
    public bool attack=false;

    // コンボ判定用フラグ
    private int comboFlgl;

    // コンボ回数
    private int coumboCount=0;

    [SerializeField] private PlayableDirector[] attackTimeline;

    [SerializeField] GameObject wepon;

    // スティック角度
    private float degree;

    private bool isGrounded;


    /// <summary>
    /// 開始処理
    /// </summary>
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
        wepon.SetActive(false);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if(isGrounded&&mov==false)
        {
            MoveOn();
        }

        SticeAngle();
        isGrounded = CheckGrounded();
        Debug.Log("コンボフラグは" + comboFlgl);

        

            Debug.Log(degree);
        if (animator==null)
        {
            return;
        }
        if (mov)
        {

            Move();
        }
     
        if (avoid&&move.magnitude==0)
        {
            rigidbody.AddForce(-transform.forward * 4.5f, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (rot)
        {
            if (cameraController.rock)
            {
                // ロックオン中はテーゲットの正面に
                var dir = cameraController.rockonTarget.transform.position - this.gameObject.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnTimeRate);
            }
            else
            {
                // 回転
                Rotation();
            }
        }
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

        if(avoid)
        {
            if(move.magnitude>0)
            {
                rigidbody.AddForce(moveForward * 50f, ForceMode.Impulse);
            }
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
                    MoveOff();
                    RotaionOff();
                }
                else if (move.magnitude > 0)
                {
                    timeline[0].Play();
                    MoveOff();
                    RotaionOff();
                }
                else if (move.magnitude > 0)
                {
                    timeline[0].Play();
                    MoveOff();
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
        
        if (context.started)
        {
            if (!attack && !avoid)
            {
                attack = true;
               
                switch(coumboCount)
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

    void Jump()
    {
        MoveOff();
        rigidbody.AddForce(transform.up * 8, ForceMode.VelocityChange);

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                animator.SetTrigger("isJump");
            }
        }
    }
    bool CheckGrounded()
    {
        //放つ光線の初期位置と姿勢
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
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

}
