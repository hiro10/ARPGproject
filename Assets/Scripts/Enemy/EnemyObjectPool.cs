using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�l�~�[�p�I�u�W�F�N�g�v�[���Ǘ��N���X
public class EnemyObjectPool : MonoBehaviour
{
    // �X�|�[������G�̃v���n�u
    public GameObject[] enemyPrefabs;
    // �v�[���̃T�C�Y
    private int poolSize = 10;       

   [SerializeField] private List<GameObject> pooledEnemies = new List<GameObject>();
    [SerializeField] BattleSceneManager sceneManager;
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
        // ���������P�O�ȓ�
        if (sceneManager.SpownCount < 10)
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
        }
            return null;
        
    }


}
