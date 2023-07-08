using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RotateUnit : MonoBehaviour
{
    //　旋回するターゲット
    [SerializeField]
    private Transform target;
    //　現在の角度
    [SerializeField]private float angle;
    //　回転するスピード
    float rotateSpeed=1000f;
    //　ターゲットからの距離
    [SerializeField]
    private Vector3 distanceFromTarget = new Vector3(0f, 1f, 1f);
    private void Start()
    {
        Debug.Log("ファントムソードオン");
        this.transform.localScale = Vector3.zero;
      
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            DOTween.To(() => rotateSpeed, (n) => rotateSpeed = n, 270f, 1.5f).SetEase(Ease.Linear);
            //rotateSpeed = Mathf.Lerp(700f, 270f, 10f);
            this.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
        }
        //　ユニットの位置 = ターゲットの位置 ＋ ターゲットから見たユニットの角度 ×　ターゲットからの距離
        transform.position = target.position + Quaternion.Euler(0f, angle, 0f) * distanceFromTarget;
        //　ユニット自身の角度 = ターゲットから見たユニットの方向の角度を計算しそれをユニットの角度に設定する
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, transform.position.y, target.position.z), Vector3.up);
        //　ユニットの角度を変更
        angle += rotateSpeed * Time.deltaTime;
        //　角度を0〜360度の間で繰り返す
        angle = Mathf.Repeat(angle, 360f);
    }
}
