using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // カメラの操作スピード
    private Vector3 speed;

    // プレイヤーの追従設定
    // プレイヤーオブジェクト
    [SerializeField] private GameObject TargetObject;
    
    // カメラの高さオフセット
    [SerializeField] private float height = 1.5f;
    // カメラとのオフセット
    [SerializeField] private float distance = 15f;
    // 横方向のカメラ角度
    [SerializeField] private float rotAngle = 0f;
    // 縦方向のカメラ角度
    [SerializeField] private float heightAngle = 10f;
    // 見上げた時のカメラ距離
    [SerializeField] private float dis_min = 5f;
    // 通常のカメラ距離
    [SerializeField] private float dis_mdl = 10f;
    // 現在のプレイヤー位置
    private Vector3 nowPos;
    // 現在の横方向のカメラ角度
    private float nowRotAngle;
    // 現在の水平方向のカメラ角度
    private float nowHeightAngle;

    /// <summary>
    /// 減衰挙動
    /// </summary>
    // 減衰フラグ
    [SerializeField]private bool enableAtten = true;
    // 補正用パラメータ
    [SerializeField] private float attenRate = 3.0f;
    [SerializeField] private float forawardDistance = 2.0f;
    private Vector3 addForeward;
    private Vector3 prevTargetPos;
    [SerializeField] private float rotAngleAttenRate=5.0f;
    [SerializeField] private float angleAttenRate = 1f;

    // ロックオン機能
    [Header("ロックオン判定")]
    public bool rock = false;
    public GameObject rockonTarget;
    // ロックオン用センサー
    public GameObject searchCircle;

    // Start is called before the first frame update
    void Start()
    {
        nowPos = TargetObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // キー入力による水平方向の加算
        rotAngle -= speed.x * Time.deltaTime * 50f;
        // キー入力による垂直方向の加算
        heightAngle += speed.z * Time.deltaTime * 50f;
        // 垂直方向の角度制限
        heightAngle = Mathf.Clamp(heightAngle, -40f, 60f);
        // カメラ距離制限
        distance = Mathf.Clamp(distance, 5f, 40f);

        // ターゲット
        rockonTarget = searchCircle.GetComponent<RockonSensor>().nowTarget;
        
        if(enableAtten)
        {
            // ターゲット位置をプレイヤーに
            var target = TargetObject.transform.position;
            
            // ロックオン機能
            if(rock)
            {
                if(rockonTarget!=null)
                {
                    // ターゲットをロックオン対象にする
                    target = rockonTarget.transform.position;
                }
                else
                {
                    rock = false;
                }
            }
            
            var halfPoint = (TargetObject.transform.position + target) / 2;
            // 位置の微小増加量
            var deltaPos = halfPoint - prevTargetPos;
            prevTargetPos = halfPoint;
            deltaPos *= forawardDistance;

            addForeward += deltaPos * Time.deltaTime * 20f;
            addForeward = Vector3.Lerp(addForeward, Vector3.zero, Time.deltaTime * attenRate);

            nowPos = Vector3.Lerp(nowPos, halfPoint + Vector3.up * height + addForeward, Mathf.Clamp01(Time.deltaTime * attenRate));
        }
        else
        {
            // 画面の中心位置
            nowPos = TargetObject.transform.position + Vector3.up * height;
        }

        if(enableAtten)
        {
            nowRotAngle = Mathf.Lerp(nowRotAngle, rotAngle, Time.deltaTime * rotAngleAttenRate);
        }
        else
        {
            // カメラの垂直、水平角度
            nowRotAngle = rotAngle;
        }
        if (enableAtten)
        {
            nowHeightAngle = Mathf.Lerp(nowHeightAngle, heightAngle, Time.deltaTime * rotAngleAttenRate);
        }
        else
        {
            nowHeightAngle = heightAngle;
        }

        if (rock)
        {
            var dis = Vector3.Distance(TargetObject.transform.position, rockonTarget.transform.position);

            // カメラの垂直角度によるカメラ距離
            if (heightAngle > 30)
            {
                //distance = Mathf.Lerp(distance, 20f * heightAngle / 30f, Time.deltaTime);
                distance = Mathf.Lerp(distance, dis_min*dis/10*heightAngle/30f, Time.deltaTime);
            }
            else if (heightAngle <= 30 && heightAngle >= -3)
            {
                distance = Mathf.Lerp(distance, dis_mdl*dis/10f, Time.deltaTime);
            }
            else if (heightAngle < -3)
            {
                distance = Mathf.Lerp(distance, dis_min, Time.deltaTime);

            }
        }
        else
        {
            // カメラの垂直角度によるカメラ距離
            
                distance = Mathf.Lerp(distance, dis_min, Time.deltaTime);

            
        }
            //distance = Mathf.Lerp(distance, dis_min, Time.deltaTime);
            // カメラの位置座標を決定
            var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * distance;

        transform.position = nowPos + new Vector3(cx, cy, cz);
        var rot = Quaternion.LookRotation(nowPos - transform.position).normalized;
        if (enableAtten)
        {
            transform.rotation = rot;
        }
        else
        {
            transform.rotation = rot;
        }
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        speed = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
    }

    public void OnRockOn(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(rock)
            {
                rock = false;
            }
            else
            {
                rock = true;
            }
        }
    }
}
