using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エネミー用オブジェクトプール管理クラス
public class EnemyObjectPool : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // スポーンする敵のプレハブ
    public int poolSize = 10;       // プールのサイズ

   [SerializeField] private List<GameObject> pooledEnemies = new List<GameObject>();

    void Start()
    {
        int enemyType;
        // プール内に敵のインスタンスを生成
        for (int i = 0; i < poolSize; i++)
        {
            enemyType = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[enemyType]);
            enemy.SetActive(false);
            pooledEnemies.Add(enemy);
        }
    }

    // 使用可能な敵を取得
    // 引数は出現位置
    public GameObject GetPooledEnemy(Vector3 pos)
    {
        foreach (GameObject enemy in pooledEnemies)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.transform.position = pos;
                enemy.SetActive(true);
                return enemy;
            }
        }
        return null;
    }


}
