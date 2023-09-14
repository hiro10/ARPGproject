using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SwordCollider : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject hitParticle;
    [SerializeField] BattleSceneManager battleSceneManager;
    EnemyController enemy;

    [SerializeField] DamageIndicator damageUi;
    //リセット用のタイムカウント

    private void Start()
    {
       
    }

    // プレイヤーの攻撃がエネミーにあたった時の処理
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag==("Enemy"))
        {
            enemy = other.gameObject.GetComponent<EnemyController>();
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,layerMask))
            {
               
                //Hit Particle
                battleSceneManager.ComboAnim();
                Instantiate(hitParticle, hit.point, Quaternion.identity);
            }
            // エネミーのダメージor死亡リアクションのトリガー
            enemy.Hp -= 1;
            damageUi.ShowDamageIndicator(other.gameObject.transform.position, 999);
            enemy.EnemyDie();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.DrawRay(ray);
    }
}
