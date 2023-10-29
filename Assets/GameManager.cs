using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// �Q�[���̐���p�@�����ς�邩��()

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string sceneName;
    public string nowSceneName;

    // ���[�r�[������
    public bool isMovePlaying;

    // �|�[�Y����?
    public bool isPause;

    // �Q�[���I�[�o�[������?
    public bool isGameOver = false;

    // �Q�[���N���A������?
    public bool isGameClear = false;

    // ���̖��O�������Ă����Npc�Ɖ�b�������̔���
    public bool onTownName = false; 

    /// <summary>
    ///  �V���O���g����
    /// </summary>
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        isPause = false;
    }

}