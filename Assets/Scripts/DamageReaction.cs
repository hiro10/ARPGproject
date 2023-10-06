using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
// 攻撃ヒット時のリアクション）
public class DamageReaction : MonoBehaviour
{
    //のけぞりによるダメージ演出で回転するボーン
    [SerializeField] private Transform waistBone;

    //のけぞりによるダメージ演出によるボーンの回転角度
    private Vector3 offsetAnglesWaist;

    private Sequence seq;

    [SerializeField] float damp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wepon"&&this.gameObject.tag=="Enemy")
        {
            // ヒットした攻撃判定の進行方向に向けて体を倒すため、角度を取得
            var bulletAngles = other.transform.eulerAngles;
            // X角度は無視
            bulletAngles.x = 0f;
            HitTiltWaist(Quaternion.Euler(bulletAngles) * Vector3.forward);
        }
        else if((other.gameObject.tag == "EnemysWepon" && this.gameObject.tag == "Player"))
        {
            // ヒットした攻撃判定の進行方向に向けて体を倒すため、角度を取得
            var bulletAngles = other.transform.eulerAngles;
            // X角度は無視
            bulletAngles.x = 0f;
            HitTiltWaist(Quaternion.Euler(bulletAngles) * Vector3.forward);
        }
    }

    /// <summary> のけぞり（ボーン回転）によるダメージ演出を再生 </summary>
    /// <param name="vector">ヒットした攻撃判定の進行方向</param>
    private void HitTiltWaist(Vector3 vector)
    {
        seq?.Kill();
        seq = DOTween.Sequence();
        // 攻撃判定の進行方向を自身のTransformのローカル座標系に変換
        vector = transform.InverseTransformVector(vector);
        // 攻撃判定の進行方向に向けて10度傾ける
        // FIXME: Vector3のどの要素に攻撃判定の進行方向ベクトルの各要素を使うか、正/負解釈をどうするかは、モデルのボーン構造に合わせて適宜変更する
        var tiltAngles = new Vector3(0f, -vector.x, -vector.z).normalized * damp;
        seq.Append(DOTween.To(() => Vector3.zero, angles => offsetAnglesWaist = angles, tiltAngles, 0.1f));
        seq.Append(DOTween.To(() => tiltAngles, angles => offsetAnglesWaist = angles, Vector3.zero, 0.2f));
        seq.Play();
    }

    private void LateUpdate()
    {
        // Animatorによる今フレームのボーンの角度が決まった後、のけぞりによる回転角度を与える
        waistBone.localEulerAngles += offsetAnglesWaist;
    }
}
