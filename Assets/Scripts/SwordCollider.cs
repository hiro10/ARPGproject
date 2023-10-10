using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

// 剣の当たり判定処
public class SwordCollider : MonoBehaviour
{
    // 判定用のレイヤー
    public LayerMask layerMask;
    // ヒット時のパーティクル
    public GameObject hitParticle;
    [SerializeField] ConboUiManager conboUi;
    EnemyController enemy;
    

    [SerializeField] DamageIndicator damageUi;
    //リセット用のタイムカウント
    [SerializeField] LoadBoss boss;

    [SerializeField] PlayerData player;

    // プレイヤーの攻撃がエネミーにあたった時の処理
    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }
        if(other.gameObject.tag==("Enemy"))
        {
            enemy = other.gameObject.GetComponent<EnemyController>();
            //ヒット音の再生
            SoundManager.instance.PlaySE(SoundManager.SE.AttackHitSe);
            // 覚醒ゲージを増やす
            if (player.PlayerCurrentAwake < 100)
            {
                player.PlayerCurrentAwake += 10f;
                if(player.PlayerCurrentAwake > 100)
                {
                    player.PlayerCurrentAwake = 100;
                }
            }
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,layerMask))
            {
               
                //Hit Particle
                conboUi.ComboAnim();
                Instantiate(hitParticle, hit.point, Quaternion.identity);
            }
            // エネミーのダメージor死亡リアクションのトリガー
           
            enemy.SetCurrentHp(4);
            int currentHp = enemy.CurrentHp();
            int maxHp = enemy.MaxHp();
            enemy.slider.value = (float)currentHp / (float)maxHp; 
           // damageUi.ShowDamageIndicator(other.gameObject.transform.position, 999);
            if (enemy.CurrentHp() <= 0&&enemy.state!=EnemyController.State.Die)
            {
                enemy.EnemyDie();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.DrawRay(ray);
    }
}
