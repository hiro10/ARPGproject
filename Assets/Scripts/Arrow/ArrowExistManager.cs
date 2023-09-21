using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ړI�n���������̕\����\������
public class ArrowExistManager : MonoBehaviour
{
    // ���I�u�W�F�N�g
    GameObject arrow;
    // �o�g���V�[���̃}�l�[�W���[
    [SerializeField] BattleSceneManager sceneManager;

    private void Start()
    {
        arrow = transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isMovePlaying|| sceneManager.InBattleArea)
        {
            arrow.SetActive(false);
        }
        else if (!GameManager.Instance.isMovePlaying|| !sceneManager.InBattleArea)
        {
            arrow.SetActive(true);
        }
        
    }
}
