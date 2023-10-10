using UnityEngine;

// �G�l�~�[����v���C���[�Ƀ_���[�W��^�������̏���
public class EnemyToPlayerDamageManager : MonoBehaviour
{
    // �q�b�g���̃p�[�e�B�N��
    public GameObject hitParticle;
    /// <summary>
    /// �G�l�~�[����v���C���[�ɍU��
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        // �^�O���v���C���[�ŉ����ԂłȂ��Ƃ��A�A�܂��͊o����ԂłȂ��Ƃ�
        if (col.tag == "Player" && !col.GetComponent<PlayerController>().avoid)
        {
            Instantiate(hitParticle, col.transform.position, Quaternion.identity);
            if (col.GetComponent<PlayerController>().IsAwakening == false)
            {
                // Hp�����炵
                col.GetComponent<PlayerData>().PlayerCurrentHp -= 10;
                // �X���C�_�[��ύX
                col.GetComponent<PlayerData>().CurrentHpSlider();
            }
        }
    }
   
    /// <summary>
    /// �G�l�~�[�̕���̓����蔻��̕\���A��\��
    /// </summary>
    /// <param name="on"></param>
    public void SwordCollision(bool on)
    {
        if(on)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
}
