using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SwordCollider : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject hitParticle;
    [SerializeField] BattleSceneManager battleSceneManager;
    EnemyController enemy;

    [SerializeField] DamageIndicator damageUi;
    //���Z�b�g�p�̃^�C���J�E���g

    private void Start()
    {
       
    }

    // �v���C���[�̍U�����G�l�~�[�ɂ����������̏���
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag==("Enemy"))
        {
            enemy = other.gameObject.GetComponent<EnemyController>();
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,layerMask))
            {
               
                //Hit Particle
                battleSceneManager.ComboAnim();
                Instantiate(hitParticle, hit.point, Quaternion.identity);
            }
            // �G�l�~�[�̃_���[�Wor���S���A�N�V�����̃g���K�[
            enemy.Hp -= 1;
            damageUi.ShowDamageIndicator(other.gameObject.transform.position, 999);
            enemy.EnemyDie();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.DrawRay(ray);
    }
}
