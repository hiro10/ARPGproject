using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingWepon : MonoBehaviour
{
    public float missileSpeed = 20f; // ミサイルの速度
    public float rotationSpeed = 10f; // ミサイルの回転速度
    public float homingAngle = 180f; // ロックオンの角度
    public Transform lockOnTarget; // ロックオンターゲット
    [SerializeField] PlayerLockOn player;
    public Transform launchPosition; // 発射位置
    void Update()
    {
        Homing();
    }

    void Homing()
    {

        lockOnTarget = player.target.transform;
        if (lockOnTarget != null)
        {

            // ターゲットの方向を向く
            transform.LookAt(lockOnTarget);
            // ロックオンターゲットがいる場合、その方向に向かって進む
            Vector3 targetDirection = lockOnTarget.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * missileSpeed * Time.deltaTime;
        }
        else
        {
            // ロックオンターゲットがいない場合、プレイヤーの正面180度以内で一番近い敵を狙う
            Collider[] colliders = Physics.OverlapSphere(transform.position, homingAngle, LayerMask.GetMask("Enemy"));
            if (colliders.Length > 0)
            {
                Transform nearestEnemy = null;
                float minDistance = float.MaxValue;

                foreach (var collider in colliders)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = collider.transform;
                    }
                }

                if (nearestEnemy != null)
                {
                    Vector3 targetDirection = nearestEnemy.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
                    transform.position += transform.forward * missileSpeed * Time.deltaTime;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            HitEnemy();
        }
    }

    // ターゲットに当たったときに呼び出されるメソッド
    public void HitEnemy()
    {
        lockOnTarget = null; // ターゲットをリセット
        gameObject.SetActive(false); // ミサイルを非アクティブにする
        transform.position = launchPosition.position; // 発射位置に戻す
    }
}