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
    [SerializeField] private EnemyObjectPool objectPool;
    // �X�|�[���̈�̃R���C�_�[
    private Collider spawnArea;
    GameObject enemy;

    // �G�l�~�[�̍ő吔
    const int ENEMY_SPOWE_MAX = 10;

    // �v���C���[���G���A���ɂ��邩�̔���
    public bool inPlayer;

    [SerializeField] BattleSceneManager sceneManager;
    
    void Start()
    {
        inPlayer = false;
        sceneManager.SpownCount = 0;
        spawnArea = GetComponent<Collider>();
        // �G�l�~�[�̃I�u�W�F�N�g�v�[���̎擾
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
    {// ������Ƀv���C���[������ΓG���I�u�W�F�N�g�v�[�����玝���Ă���

        if (other.gameObject.tag == ("Player"))
        {
            inPlayer = true;
            
            // �G�l�~�[�̐����ʒu�������_����(�����W�͌Œ�)
            Vector3 randomPosition = Vector3.zero;
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnInterval)
            {
                // �G�l�~�[�̐����ʒu
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
                //    // �X�|�[���ʒu��ݒ�
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
