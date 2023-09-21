using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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

    private void Start()
    {
        inBattleArea=false;

        spownCount = 0;

        deadCount = 0;

        allEnemyDeadCount = 0;
    }
}
