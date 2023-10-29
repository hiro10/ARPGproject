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

    // �V�[���}�l�[�W���[
    [SerializeField] BattleSceneManager sceneManager;

    // �o�����̃p�[�e�B�N��
    public GameObject exsistParticle;

    // �I�t�Z�b�g�̑傫��
    private float offset = 0.5f;      

    /// <summary>
    /// �J�n����
    /// �e��Ԕ���p�ϐ��̏������A�擾
    /// </summary>
    void Start()
    {
        inPlayer = false;
        sceneManager.SpownCount = 0;
        spawnArea = GetComponent<Collider>();
        // �G�l�~�[�̃I�u�W�F�N�g�v�[���̎擾
        objectPool = transform.root.gameObject.GetComponent<EnemyObjectPool>();
    }

    /// <summary>
    /// �X�V����
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
    {// ������Ƀv���C���[������ΓG���I�u�W�F�N�g�v�[�����玝���Ă���

        if (other.gameObject.tag == ("Player"))
        {
            inPlayer = true;
            
            // �G�l�~�[�̐����ʒu�������_����(�����W�͌Œ�)
            Vector3 randomPosition = Vector3.zero;
            timeSinceLastSpawn += Time.deltaTime;

            // �������Ԃ𒴂�����
            if (timeSinceLastSpawn >= spawnInterval)
            {
                // �G�l�~�[�̐����ʒu
                randomPosition = new Vector3
                        (
                            Random.Range(spawnArea.bounds.min.x+ offset, spawnArea.bounds.max.x- offset),
                            1f,
                            Random.Range(spawnArea.bounds.min.z+ offset, spawnArea.bounds.max.z- offset)
                        );
                // �����l���Ȃ琶��
                if (sceneManager.SpownCount < ENEMY_SPOWE_MAX)
                {
                    Instantiate(exsistParticle, randomPosition, Quaternion.identity);
                    enemy = objectPool.GetPooledEnemy(randomPosition);
                    enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    sceneManager.SpownCount++;
                }
                // �X�|�[�����Ԃ����Z�b�g
                timeSinceLastSpawn = 0f;
            }
        }
    }
}
