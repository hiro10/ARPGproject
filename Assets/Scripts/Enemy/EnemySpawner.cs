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

    // シーンマネージャー
    [SerializeField] BattleSceneManager sceneManager;

    // 出現時のパーティクル
    public GameObject exsistParticle;

    /// <summary>
    /// 開始処理
    /// 各状態判定用変数の初期化、取得
    /// </summary>
    void Start()
    {
        inPlayer = false;
        sceneManager.SpownCount = 0;
        spawnArea = GetComponent<Collider>();
        // エネミーのオブジェクトプールの取得
        objectPool = transform.root.gameObject.GetComponent<EnemyObjectPool>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if(sceneManager.DeadCount>= ENEMY_SPOWE_MAX && inPlayer)
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

            // 生成時間を超えたら
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
                    Instantiate(exsistParticle, randomPosition, Quaternion.identity);
                    enemy = objectPool.GetPooledEnemy(randomPosition);
                    enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    sceneManager.SpownCount++;
                }
                
                timeSinceLastSpawn = 0f;
            }
        }
    }
   
}
