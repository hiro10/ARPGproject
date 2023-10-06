using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;
// �퓬�V�[���i��ɃG�l�~�[�j�̊Ǘ�
public class BattleSceneManager : MonoBehaviour
{
    // ��p�V�[���̊e�󋵔��f�p�ϐ�

    // �o�g���G���A����?
    private bool inBattleArea;
    // �e�o�g���G���A�̃G�l�~�[�̐�����
    private int spownCount;
    // �e�o�g���G���A�̃G�l�~�[�̐�����
    private int deadCount;
    // �G�l�~�[�̑����j��
    private int allEnemyDeadCount;
    // �����G���A��
    private int clearAreaCount;
    // �o�g���G���A����?
    private bool inBossBattleArea;
    

    private bool resultOn;
    public bool Result
    {
        get
        {
            return resultOn;
        }
        set
        {
            resultOn = value;
        }
    }
    [SerializeField] GameObject resultPanel;
    [SerializeField] GameObject slideUi;
    [SerializeField] GameObject slideUiDead;
    [SerializeField] GameObject resultScorePanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject resultButtons;
    //�v���p�e�B
    // �o�g���G���A
    public bool InBattleArea
    {
        get
        {
            return inBattleArea;
        }
        set
        {
            inBattleArea = value;
        }
    }
    // �{�X�̃o�g���G���A���H
    public bool InBossBattleArea
    {
        get
        {
            return inBossBattleArea;
        }
        set
        {
            inBossBattleArea = value;
        }
    }

    // �e�o�g���G���A�̃G�l�~�[�̐�����
    public int SpownCount
    {
        get
        {
            return spownCount;
        }
        set
        {
            spownCount = value;
        }
    }

    // �e�o�g���G���A�̃G�l�~�[�̐�����
    public int DeadCount
    {
        get
        {
            return deadCount;
        }
        set
        {
            deadCount = value;
        }
    }

    // �e�o�g���G���A�̃G�l�~�[�̐�����
    public int AllEnemyDeadCount
    {
        get
        {
            return allEnemyDeadCount;
        }
        set
        {
            allEnemyDeadCount = value;
        }
    }
    public int ClearAreaCount
    {
        get
        {
            return clearAreaCount;
        }
        set
        {
            clearAreaCount = value;
        }
    }

    private void Start()
    {
        inBattleArea=false;

        spownCount = 0;

        deadCount = 0;

        allEnemyDeadCount = 0;

        clearAreaCount = 0;

        resultOn = false;

        resultPanel.SetActive(false);

        resultButtons.SetActive(false);

        slideUi.SetActive(false);

        resultScorePanel.SetActive(false);

        slideUiDead.SetActive(false);
    }

    private void Update()
    {
        if (allEnemyDeadCount >= 40 && !resultOn)
        {
            resultOn = true;
            StartCoroutine(ResultStart());
        }
        if(deadCount==10&& !inBossBattleArea)
        {
            clearAreaCount++;
        }
        
    }

     IEnumerator ResultStart()
    {
        slideUi.SetActive(true);
        resultScorePanel.SetActive(true);
        // �v���C���[�̓��͂��󂯕t���Ȃ�����i��ŏC���j
        player.GetComponent<PlayerInput>().enabled=false;
        mainPanel.SetActive(false);
        resultPanel.SetActive(true);
        slideUi.GetComponent<SlideUiControl>().UiMove();
        yield return new WaitForSeconds(2);
        resultScorePanel.GetComponent<SlideUiControl>().UiMove();
        yield return new WaitForSeconds(2);
        resultButtons.SetActive(true);
    }

    public IEnumerator DeadResultStart()
    {
        slideUiDead.SetActive(true);
        // �v���C���[�̓��͂��󂯕t���Ȃ�����i��ŏC���j
        player.GetComponent<PlayerInput>().enabled = false;
        mainPanel.SetActive(false);
        resultPanel.SetActive(true);
        slideUiDead.GetComponent<SlideUiControl>().UiMove();
        yield return new WaitForSeconds(4);
        resultButtons.SetActive(true);
    }

}
