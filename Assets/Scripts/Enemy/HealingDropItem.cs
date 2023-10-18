using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�l�~�[���S��Ƀh���b�v����񕜖�̏���
public class HealingDropItem : MonoBehaviour
{
    public float homingSpeed = 5f;  // �񕜃A�C�e���̈ړ����x
    public float rotationSpeed = 180f;  // ��]���x
    public float healAmount = 20f;  // �񕜗�
    public float waitTime = 3f;  // �ҋ@����

    private Transform player;  // �v���C���[�̈ʒu
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
                // �v���C���[�̈ʒu��y���W��1��������
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
            // �v���C���[�ɓ���������񕜂��A�A�C�e����j��
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