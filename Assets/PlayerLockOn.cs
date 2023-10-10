using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO:要修正
public class PlayerLockOn : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Transform originTrn;
    [SerializeField] float lockonRange = 20;
    [SerializeField] string targetLayerName;
    [SerializeField] GameObject lockonCursor;
    [SerializeField] GameObject player;
    public GameObject target;


    bool lockonInput = false;
    bool isLockon = false;

    Camera mainCamera;
    Transform cameraTrn;
    


    void Start()
    {
        mainCamera = Camera.main;
        cameraTrn = mainCamera.transform;
    }


    void Update()
    {
       
        // 一定距離外に出たらロックオンを外す
        if (target != null)
        {
            // すでにロックオン済みなら解除する
            if (Vector3.Distance(target.transform.position, originTrn.position) >= lockonRange|| target.activeSelf == false)
            {
                isLockon = false;
                target = null;
                playerCamera.InactiveLockonCamera();
                lockonCursor.SetActive(false);
                lockonInput = false;
                return;
            }
        }
        // ロックオンボタンを押した際の処理
        if (lockonInput)
        {
            // すでにロックオン済みなら解除する
            if (isLockon)
            {
                isLockon = false;
                target = null;
                playerCamera.InactiveLockonCamera();
                lockonCursor.SetActive(false);
                lockonInput = false;
                return;
            }

            // ロックオン対象の検索、いるならロックオン、いないならカメラ角度をリセット
            target = GetLockonTarget();
            if (target/*&&target.gameObject.GetComponent<EnemyController>().CurrentHp()>=0*/)
            {
                isLockon = true;
                playerCamera.ActiveLockonCamera(target);
                lockonCursor.SetActive(true);
            }
            else
            {
                playerCamera.ResetFreeLookCamera();
            }
            lockonInput = false;
        }

        // ロックオンカーソル
        if (isLockon)
        {
            lockonCursor.transform.position = mainCamera.WorldToScreenPoint(new Vector3(target.transform.position.x, target.transform.position.y + 1f, target.transform.position.z));
        }
    }
    // ロックオン入力を受け取る関数
    public void OnLockOn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("ロックオン");
            if (lockonInput == false)
                lockonInput = true;
            else
                lockonInput = false;

        }

    }

    /// <summary>
    /// ロックオン対象の計算処理を行い取得する
    /// 計算は3つの工程に分かれる
    /// </summary>
    /// <returns></returns>
    GameObject GetLockonTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // "enemy"タグの全てのオブジェクトを取得

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            Vector3 direction = enemy.transform.position - transform.position; // カメラから敵への方向ベクトル
            float angle = Vector3.Angle(transform.forward, direction); // カメラから敵への方向とカメラの正面との角度

            if (angle <= 45.0f)
            {
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position); // 敵とプレイヤーの距離

                if (distanceToPlayer < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distanceToPlayer;
                }
            }
        }

        
        return closestEnemy;

    }
}