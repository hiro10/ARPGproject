using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �I�u�W�F�N�g�v�[������G�𐶐�����N���X
public class EnemySpawner : MonoBehaviour
{
    // �����܂ł̃C���^�[�o��
    private float spawnInterval = 2f;
    // ��������
    private float timeSinceLastSpawn = 0f;
    // �I�u�W�F�N�g�v�[��
    private EnemyObjectPool objectPool;
    // �X�|�[���̈�̃R���C�_�[
    private Collider spawnArea;
    GameObject enemy;
    void Start()
    {
        spawnArea = GetComponent<Collider>();
        // �G�l�~�[�̃I�u�W�F�N�g�v�[���̎擾
        objectPool = transform.root.gameObject.GetComponent<EnemyObjectPool>();
    }

    private void OnTriggerStay(Collider other)
    {// ������Ƀv���C���[������ΓG���I�u�W�F�N�g�v�[�����玝���Ă���
        if (other.gameObject.tag == ("Player"))
        {
            // �G�l�~�[�̐����ʒu�������_����(�����W�͌Œ�)
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
                    // �X�|�[���ʒu��ݒ�
                    enemy.transform.position = randomPosition;
                    enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f); 
                    enemy.SetActive(true);
                }

                timeSinceLastSpawn = 0f;
            }
        }
    }
   
}
