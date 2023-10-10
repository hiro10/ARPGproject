using UnityEngine;

// エネミーからプレイヤーにダメージを与えた時の処理
public class EnemyToPlayerDamageManager : MonoBehaviour
{
    // ヒット時のパーティクル
    public GameObject hitParticle;
    /// <summary>
    /// エネミーからプレイヤーに攻撃
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        // タグがプレイヤーで回避状態でないとき、、または覚醒状態でないとき
        if (col.tag == "Player" && !col.GetComponent<PlayerController>().avoid)
        {
            // 当たった位置を取得
            Vector3 collisionPoint = col.ClosestPointOnBounds(transform.position);
            // 当たった位置にhitエフェクトを表示
            Instantiate(hitParticle, collisionPoint, Quaternion.identity);
           
            if (col.GetComponent<PlayerController>().IsAwakening == false)
            {
                // Hpを減らし
                col.GetComponent<PlayerData>().PlayerCurrentHp -= 10;
                // スライダーを変更
                col.GetComponent<PlayerData>().CurrentHpSlider();
            }
        }
    }
   
    /// <summary>
    /// エネミーの武器の当たり判定の表示、非表示
    /// </summary>
    /// <param name="on"></param>
    public void SwordCollision(bool on)
    {
        if(on)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
}
