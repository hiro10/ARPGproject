using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// エネミー死亡後にドロップする回復薬の処理
public class HealingDropItem : MonoBehaviour
{
    // 回復アイテムの移動速度
    public float homingSpeed = 5f;
    // 回転速度
    public float rotationSpeed = 180f;
    // 待機時間
    public float waitTime = 3f;
    // プレイヤーの位置
    private Transform player;
    // ホーミングしているか
    private bool isHoming = false;
    // 待ち時間
    private float waitTimer = 0;


    private void Start()
    {
        // プレイヤーの取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            return;
        }

    }

    private void Update()
    {
        // 停止している処理
        if (waitTimer < waitTime)
        {
            waitTimer += Time.deltaTime;
        }
        else
        {
            if (!isHoming)
            {
                Vector3 targetPosition = player.position;
                // プレイヤーの位置のy座標を1高くする
                targetPosition.y += 1f;

                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.position += direction * homingSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().StartEffect();
            // SE再生
            SoundManager.instance.PlaySE(SoundManager.SE.HealingSe);
            // プレイヤーに当たったら回復し、アイテムを破壊
            PlayerData player = other.GetComponent<PlayerData>();
            if (player != null)
            {
                // プレイヤーを回復
                player.PlayerCurrentHp+=100;
                // Hpが最大値より大きくなれば
                if(player.PlayerCurrentHp> player.PlayerMaxHp)
                {
                    player.PlayerCurrentHp = player.PlayerMaxHp;
                }
                // スライダーへの反映
                player.CurrentHpSlider();
                Destroy(gameObject);
            }
        }
    }
}