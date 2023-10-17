using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エネミー死亡後にドロップする回復薬の処理
public class HealingDropItem : MonoBehaviour
{
    public float homingSpeed = 5f;  // 回復アイテムの移動速度
    public float rotationSpeed = 180f;  // 回転速度
    public float healAmount = 20f;  // 回復量
    public float waitTime = 3f;  // 待機時間

    private Transform player;  // プレイヤーの位置
    private bool isHoming = false;
    private float waitTimer = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            return;
        }

    }

    private void Update()
    {
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
            SoundManager.instance.PlaySE(SoundManager.SE.HealingSe);
            // プレイヤーに当たったら回復し、アイテムを破壊
            PlayerData playerHealth = other.GetComponent<PlayerData>();
            if (playerHealth != null)
            {
                playerHealth.PlayerCurrentHp+=100;
                if(playerHealth.PlayerCurrentHp> playerHealth.PlayerMaxHp)
                {
                    playerHealth.PlayerCurrentHp = playerHealth.PlayerMaxHp;
                }
                playerHealth.CurrentHpSlider();
                Destroy(gameObject);
            }
        }
    }
}