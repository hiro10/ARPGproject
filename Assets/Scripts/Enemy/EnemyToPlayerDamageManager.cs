using UnityEngine;

// エネミーからプレイヤーにダメージを与えた時の処理
public class EnemyToPlayerDamageManager : MonoBehaviour
{
    // ヒット時のパーティクル
    public GameObject hitParticle;

    // ヒット時のパーティクル
    public GameObject hitParticleAwake;

   
    /// <summary>
    /// エネミーからプレイヤーに攻撃
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        // タグがプレイヤーで回避状態でないとき、、または覚醒状態でないとき
        if (col.tag == "Player" && !col.GetComponent<PlayerController>().avoid)
        {
            if(col.GetComponent<PlayerController>().attack)
            {
                return;
            }
           
           // col.gameObject.GetComponent<Animator>().SetTrigger("Damage");
            // 当たった位置を取得
            Vector3 collisionPoint = col.ClosestPointOnBounds(transform.position);
     
            // プレイヤーが覚醒状態でないときのダメージ処理
            if (col.GetComponent<PlayerController>().IsAwakening == false)
            {
                if(col.GetComponent<PlayerController>().currentHitDamageCount==5)
                { 
                    col.GetComponent<PlayerController>().Damage();
                }
                else
                {
                    col.GetComponent<PlayerController>().currentHitDamageCount++;
                    SoundManager.instance.PlaySE(SoundManager.SE.EnemyAttack1);
                    // 当たった位置にhitエフェクトを表示
                    Instantiate(hitParticle, collisionPoint, Quaternion.identity);
                    // Hpを減らし
                    col.GetComponent<PlayerData>().PlayerCurrentHp -= 10;
                    // スライダーを変更
                    col.GetComponent<PlayerData>().CurrentHpSlider();
                }
               
            }
            else if(col.GetComponent<PlayerController>().IsAwakening)
            {
                // 覚醒時は無敵なのでダメージを発生させない
                SoundManager.instance.PlaySE(SoundManager.SE.PlaterAwakeGardSe);
                // 当たった位置にhitエフェクトを表示
                Instantiate(hitParticleAwake, collisionPoint, Quaternion.identity);
            }
            
        }
    }
    

    /// <summary>
    /// エネミーの武器の当たり判定の表示、非表示
    /// </summary>
    /// <param name="on"></param>
    public void SwordCollision(bool on)
    {
        if(on==true)
        {
            gameObject.SetActive(true);
        }
        else if(on == false)
        {
            gameObject.SetActive(false);
        }
    }
    
}
