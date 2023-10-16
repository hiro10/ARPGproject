using UnityEngine;

// �G�l�~�[����v���C���[�Ƀ_���[�W��^�������̏���
public class EnemyToPlayerDamageManager : MonoBehaviour
{
    // �q�b�g���̃p�[�e�B�N��
    public GameObject hitParticle;

    // �q�b�g���̃p�[�e�B�N��
    public GameObject hitParticleAwake;

   
    /// <summary>
    /// �G�l�~�[����v���C���[�ɍU��
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        // �^�O���v���C���[�ŉ����ԂłȂ��Ƃ��A�A�܂��͊o����ԂłȂ��Ƃ�
        if (col.tag == "Player" && !col.GetComponent<PlayerController>().avoid)
        {
            if(col.GetComponent<PlayerController>().attack)
            {
                return;
            }
           
           // col.gameObject.GetComponent<Animator>().SetTrigger("Damage");
            // ���������ʒu���擾
            Vector3 collisionPoint = col.ClosestPointOnBounds(transform.position);
     
            // �v���C���[���o����ԂłȂ��Ƃ��̃_���[�W����
            if (col.GetComponent<PlayerController>().IsAwakening == false)
            {
                if(col.GetComponent<PlayerController>().currentHitDamageCount==5)
                { 
                    col.GetComponent<PlayerController>().Damage();
                }
                else
                {
                    col.GetComponent<PlayerController>().currentHitDamageCount++;
                    SoundManager.instance.PlaySE(SoundManager.SE.EnemyAttack1);
                    // ���������ʒu��hit�G�t�F�N�g��\��
                    Instantiate(hitParticle, collisionPoint, Quaternion.identity);
                    // Hp�����炵
                    col.GetComponent<PlayerData>().PlayerCurrentHp -= 10;
                    // �X���C�_�[��ύX
                    col.GetComponent<PlayerData>().CurrentHpSlider();
                }
               
            }
            else if(col.GetComponent<PlayerController>().IsAwakening)
            {
                // �o�����͖��G�Ȃ̂Ń_���[�W�𔭐������Ȃ�
                SoundManager.instance.PlaySE(SoundManager.SE.PlaterAwakeGardSe);
                // ���������ʒu��hit�G�t�F�N�g��\��
                Instantiate(hitParticleAwake, collisionPoint, Quaternion.identity);
            }
            
        }
    }
    

    /// <summary>
    /// �G�l�~�[�̕���̓����蔻��̕\���A��\��
    /// </summary>
    /// <param name="on"></param>
    public void SwordCollision(bool on)
    {
        if(on==true)
        {
            gameObject.SetActive(true);
        }
        else if(on == false)
        {
            gameObject.SetActive(false);
        }
    }
    
}
