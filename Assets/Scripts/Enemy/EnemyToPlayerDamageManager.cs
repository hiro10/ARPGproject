using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�l�~�[����v���C���[�Ƀ_���[�W��^�������̏���
public class EnemyToPlayerDamageManager : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player"&&!col.GetComponent<PlayerController>().avoid)
        {
            Debug.Log("������");
            
            col.GetComponent<PlayerData>().PlayerCurrentHp -=10;
            col.GetComponent<PlayerData>().CurrentHpSlider();
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("������");
            //col.GetComponent<PlayerController>().state=PlayerController.PLAYER_STATE.BATTLE;
        }
    }

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
