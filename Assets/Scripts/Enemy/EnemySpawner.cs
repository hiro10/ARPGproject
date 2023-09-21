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
    [SerializeField] private EnemyObjectPool objectPool;
    // スポーン領域のコライダー
    private Collider spawnArea;
    GameObject enemy;

    // エネミーの最大数
    const int ENEMY_SPOWE_MAX = 10;

    // プレイヤーがエリア内にいるかの判定
    public bool inPlayer;

    [SerializeField] BattleSceneManager sceneManager;
    
    void Start()
    {
        inPlayer = false;
        sceneManager.SpownCount = 0;
        spawnArea = GetComponent<Collider>();
        // エネミーのオブジェクトプールの取得
        objectPool = transform.root.gameObject.GetComponent<EnemyObjectPool>();
    }

    private void Update()
    {
        if(sceneManager.DeadCount>=10&&inPlayer)
        {
            sceneManager.InBattleArea = false;
            sceneManager.DeadCount = 0;
            sceneManager.SpownCount = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {// 判定内にプレイヤーがいれば敵をオブジェクトプールから持ってくる

        if (other.gameObject.tag == ("Player"))
        {
            inPlayer = true;
            
            // エネミーの生成位置をランダムに(ｙ座標は固定)
            Vector3 randomPosition = Vector3.zero;
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnInterval)
            {
                // エネミーの生成位置
                randomPosition = new Vector3
                        (
                            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                            1f,
                            Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
                        );
                if (sceneManager.SpownCount < 10)
                {
                    enemy = objectPool.GetPooledEnemy(randomPosition);
                    enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    sceneManager.SpownCount++;
                }
                //if (enemy != null)
                //{
                //    // スポーン位置を設定
                //    enemy.transform.position = randomPosition;
                //    enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f); 
                //    enemy.SetActive(true);
                //    sceneManager.SpownCount++;
                //}

                timeSinceLastSpawn = 0f;
            }
        }
    }
   
}
