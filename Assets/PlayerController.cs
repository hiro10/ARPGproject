using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        
        if (animator==null)
        {
            return;
        }
        if (mov)
        {
            
            
            Move();
        }
        else
        {
            if(avoid)
            {
                rigidbody.AddForce(-transform.forward * 50f, ForceMode.Impulse);
            }
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

                }
                //通常回避
                else
                {
                    MoveOff();
                    RotaionOff();
                }
                avoid = true;
            }
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
}
