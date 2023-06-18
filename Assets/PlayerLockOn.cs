using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLockOn : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Transform originTrn;
    [SerializeField] float lockonRange = 20;
    [SerializeField] string targetLayerName;
    [SerializeField] GameObject lockonCursor;

    float lockonFactor = 0.3f;
    float lockonThreshold = 0.5f;
    bool lockonInput = false;
    bool isLockon = false;

    Camera mainCamera;
    Transform cameraTrn;
    GameObject targetObj;


    void Start()
    {
        mainCamera = Camera.main;
        cameraTrn = mainCamera.transform;
    }


    void Update()
    {
        if (lockonInput)
        {
            // すでにロックオン済みなら解除する
            if (isLockon)
            {
                isLockon = false;
                playerCamera.InactiveLockonCamera();
                lockonCursor.SetActive(false);
                lockonInput = false;
                return;
            }

            // ロックオン対象の検索、いるならロックオン、いないならカメラ角度をリセット
            targetObj = GetLockonTarget();
            if (targetObj)
            {
                isLockon = true;
                playerCamera.ActiveLockonCamera(targetObj);
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
            lockonCursor.transform.position = mainCamera.WorldToScreenPoint(targetObj.transform.position);
        }
    }



    // ロックオン入力を受け取る関数
    public void OnLockon(InputValue value)
    {
        lockonInput = value.isPressed;
    }


    /// <summary>
    /// ロックオン対象の計算処理を行い取得する
    /// 計算は3つの工程に分かれる
    /// </summary>
    /// <returns></returns>
    GameObject GetLockonTarget()
    {
        // 1. SphereCastAllを使ってPlayer周辺のEnemyを取得しListに格納
        RaycastHit[] hits = Physics.SphereCastAll(originTrn.position, lockonRange, Vector3.up, 0, LayerMask.GetMask(targetLayerName));
        if (hits?.Length == 0)
        {
            // 範囲内にターゲットなし
            return null;
        }


        // 2. 1のリスト全てにrayを飛ばし射線が通るものだけをList化
        List<GameObject> hitObjects = new List<GameObject>();
        RaycastHit hit;
        for (var i = 0; i < hits.Length; i++)
        {
            var direction = hits[i].collider.gameObject.transform.position - originTrn.position;
            if (Physics.Raycast(originTrn.position, direction, out hit, lockonRange))
            {
                if (hit.collider.gameObject == hits[i].collider.gameObject)
                {
                    hitObjects.Add(hit.collider.gameObject);
                }
            }
        }
        if (hitObjects?.Count == 0)
        {
            // 射線が通ったターゲットなし
            return null;
        }


        // 3. 2のリスト全てのベクトルとカメラのベクトルを比較し、画面中央に一番近いものを探す
        // 正直何やってるかよくわかんない...
        float degreep = Mathf.Atan2(cameraTrn.forward.x, cameraTrn.forward.z);
        float degreemum = Mathf.PI * 2;
        GameObject target = null;

        foreach (var enemy in hitObjects)
        {
            Vector3 pos = cameraTrn.position - enemy.transform.position;
            Vector3 pos2 = enemy.transform.position - cameraTrn.position;
            pos2.y = 0.0f;
            pos2.Normalize();

            float degree = Mathf.Atan2(pos2.x, pos2.z);
            if (Mathf.PI <= (degreep - degree))
            {
                degree = degreep - degree - Mathf.PI * 2;
            }
            else if (-Mathf.PI >= (degreep - degree))
            {
                degree = degreep - degree + Mathf.PI * 2;
            }
            else
            {
                degree = degreep - degree;
            }

            degree = degree + degree * (pos.magnitude / 500) * lockonFactor;
            if (Mathf.Abs(degreemum) >= Mathf.Abs(degree))
            {
                degreemum = degree;
                target = enemy;
            }
        }

        //// 求めた一番小さい値が一定値より小さい場合、ターゲッティングをオンにします
        if (Mathf.Abs(degreemum) <= lockonThreshold)
        {
            return target;
        }

        return null;
    }
}
