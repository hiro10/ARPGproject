using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトプールから敵を生成するクラス
public class EnemySpawner : MonoBehaviour
{
    // 生成までのインターバル
    private float spawnInterval = 2f;
    // 生成時間
    private float timeSinceLastSpawn = 0f;
    // オブジェクトプール
    private EnemyObjectPool objectPool;
    // スポーン領域のコライダー
    private Collider spawnArea;
    GameObject enemy;
    void Start()
    {
        spawnArea = GetComponent<Collider>();
        // エネミーのオブジェクトプールの取得
        objectPool = transform.root.gameObject.GetComponent<EnemyObjectPool>();
    }

    private void OnTriggerStay(Collider other)
    {// 判定内にプレイヤーがいれば敵をオブジェクトプールから持ってくる
        if (other.gameObject.tag == ("Player"))
        {
            // エネミーの生成位置をランダムに(ｙ座標は固定)
            Vector3 randomPosition = Vector3.zero;
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnInterval)
            {
                randomPosition = new Vector3
                        (
                            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                            1f,
                            Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
                        );

                enemy = objectPool.GetPooledEnemy(randomPosition);

                if (enemy != null)
                {
                    
                    
                    Debug.Log("randomPosition" + randomPosition);
                    // スポーン位置を設定
                    enemy.transform.position = randomPosition;
                    enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f); 
                    enemy.SetActive(true);
                }

                timeSinceLastSpawn = 0f;
            }
        }
    }
   
}
