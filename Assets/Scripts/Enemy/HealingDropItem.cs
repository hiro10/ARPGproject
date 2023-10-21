using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �G�l�~�[���S��Ƀh���b�v����񕜖�̏���
public class HealingDropItem : MonoBehaviour
{
    // �񕜃A�C�e���̈ړ����x
    public float homingSpeed = 5f;
    // ��]���x
    public float rotationSpeed = 180f;
    // �ҋ@����
    public float waitTime = 3f;
    // �v���C���[�̈ʒu
    private Transform player;
    // �z�[�~���O���Ă��邩
    private bool isHoming = false;
    // �҂�����
    private float waitTimer = 0;


    private void Start()
    {
        // �v���C���[�̎擾
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            return;
        }

    }

    private void Update()
    {
        // ��~���Ă��鏈��
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
            other.GetComponent<PlayerController>().StartEffect();
            // SE�Đ�
            SoundManager.instance.PlaySE(SoundManager.SE.HealingSe);
            // �v���C���[�ɓ���������񕜂��A�A�C�e����j��
            PlayerData player = other.GetComponent<PlayerData>();
            if (player != null)
            {
                // �v���C���[����
                player.PlayerCurrentHp+=100;
                // Hp���ő�l���傫���Ȃ��
                if(player.PlayerCurrentHp> player.PlayerMaxHp)
                {
                    player.PlayerCurrentHp = player.PlayerMaxHp;
                }
                // �X���C�_�[�ւ̔��f
                player.CurrentHpSlider();
                Destroy(gameObject);
            }
        }
    }
}