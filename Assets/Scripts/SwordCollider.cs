using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

// ���̓����蔻�菈
public class SwordCollider : MonoBehaviour
{
    // ����p�̃��C���[
    public LayerMask layerMask;
    // �q�b�g���̃p�[�e�B�N��
    public GameObject hitParticle;
    [SerializeField] ConboUiManager conboUi;
    EnemyController enemy;
    

    [SerializeField] DamageIndicator damageUi;
    //���Z�b�g�p�̃^�C���J�E���g
    [SerializeField] LoadBoss boss;

    [SerializeField] PlayerData player;

    // �v���C���[�̍U�����G�l�~�[�ɂ����������̏���
    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }
        if(other.gameObject.tag==("Enemy"))
        {
            enemy = other.gameObject.GetComponent<EnemyController>();
            //�q�b�g���̍Đ�
            SoundManager.instance.PlaySE(SoundManager.SE.AttackHitSe);
            // �o���Q�[�W�𑝂₷
            if (player.PlayerCurrentAwake < 100)
            {
                player.PlayerCurrentAwake += 10f;
                if(player.PlayerCurrentAwake > 100)
                {
                    player.PlayerCurrentAwake = 100;
                }
            }
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,layerMask))
            {
               
                //Hit Particle
                conboUi.ComboAnim();
                Instantiate(hitParticle, hit.point, Quaternion.identity);
            }
            // �G�l�~�[�̃_���[�Wor���S���A�N�V�����̃g���K�[
           
            enemy.SetCurrentHp(4);
            int currentHp = enemy.CurrentHp();
            int maxHp = enemy.MaxHp();
            enemy.slider.value = (float)currentHp / (float)maxHp; 
           // damageUi.ShowDamageIndicator(other.gameObject.transform.position, 999);
            if (enemy.CurrentHp() <= 0&&enemy.state!=EnemyController.State.Die)
            {
                enemy.EnemyDie();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.DrawRay(ray);
    }
}
