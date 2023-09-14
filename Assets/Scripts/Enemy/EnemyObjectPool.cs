using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�l�~�[�p�I�u�W�F�N�g�v�[���Ǘ��N���X
public class EnemyObjectPool : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // �X�|�[������G�̃v���n�u
    public int poolSize = 10;       // �v�[���̃T�C�Y

   [SerializeField] private List<GameObject> pooledEnemies = new List<GameObject>();

    void Start()
    {
        int enemyType;
        // �v�[�����ɓG�̃C���X�^���X�𐶐�
        for (int i = 0; i < poolSize; i++)
        {
            enemyType = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[enemyType]);
            enemy.SetActive(false);
            pooledEnemies.Add(enemy);
        }
    }

    // �g�p�\�ȓG���擾
    // �����͏o���ʒu
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
