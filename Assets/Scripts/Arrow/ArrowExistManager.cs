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
        if (GameManager.Instance.nowSceneName == "DemoScene")
        {
            if (GameManager.Instance.isMovePlaying || sceneManager.InBattleArea)
            {// ���[�r�[���łȂ��A�o�g���G���A���Ȃ�
                arrow.SetActive(false);
            }
            else if (!GameManager.Instance.isMovePlaying || !sceneManager.InBattleArea)
            {
                arrow.SetActive(true);
            }
        }
        // �{�X��
        if (GameManager.Instance.nowSceneName == "Test")
        {
            if (GameManager.Instance.isMovePlaying || sceneManager.InBossBattleArea)
            {// ���[�r�[���łȂ��A�o�g���G���A���Ȃ�
                arrow.SetActive(false);
            }
            else if (!GameManager.Instance.isMovePlaying || !sceneManager.InBossBattleArea)
            {
                arrow.SetActive(true);
            }
        }

    }
}
