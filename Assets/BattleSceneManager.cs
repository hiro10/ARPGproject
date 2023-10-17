using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
// �퓬�V�[���̊Ǘ�
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
    // �X�^�[�g�����Ƃ��̎���
    private float startTime;
    // �o�ߎ��Ԃ��i�[
    private int elapsedTimeInSeconds;
    // ���U���g�����̔���
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
    // ���U���g��ʂł̃X�R�A�e�L�X�g
    [SerializeField] TextMeshProUGUI resultScoreText;
    // ���U���g��ʂł̎��ԃe�L�X�g
    [SerializeField] TextMeshProUGUI resultTimeText;
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
        // �Q�[�����J�n�������_�̎��Ԃ��L�^
        startTime = Time.time;

        inBattleArea =false;

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

     private async void Update()
    {
        // ���݂̎��ԂƊJ�n���Ԃ̍������v�Z���A�b�P�ʂɕϊ����Đ����ɕϊ�
        elapsedTimeInSeconds = Mathf.FloorToInt(Time.time - startTime);

        if (allEnemyDeadCount >= 40 && !resultOn)
        {
            StartResult();
        }
        if(deadCount==10&& !inBossBattleArea)
        {
            clearAreaCount++;
        }
        
    }
    private async void StartResult()
    {
        resultOn = true;

        // ������ResultStart���\�b�h���Ăяo���A�񓯊��ҋ@����
        await ResultStart();

        // ResultStart���\�b�h������������̏����������ɒǉ��ł��܂�
    }

   �@public async void StartDeadResult()
    {
        // ������ResultStart���\�b�h���Ăяo���A�񓯊��ҋ@����
        await DeadResultStart();
    }

    /// <summary>
    /// �N���A���̃��U���g
    /// </summary>
    /// <returns></returns>
    private async UniTask ResultStart()
    {
      slideUi.SetActive(true);
        resultScorePanel.SetActive(true);
        // �X�R�A�\�L
        resultScoreText.text = Score().ToString();
        resultTimeText.text = elapsedTimeInSeconds.ToString() + "�b";
        // �v���C���[�̓��͂��󂯕t���Ȃ�����i��ŏC���j
        player.GetComponent<PlayerInput>().enabled = false;
        mainPanel.SetActive(false);
        resultPanel.SetActive(true);
        slideUi.GetComponent<SlideUiControl>().UiMove();
        await UniTask.Delay(2000); // 2�b�ҋ@
        resultScorePanel.GetComponent<ResultUiMove>().ResultScoreUYiMove();
        await UniTask.Delay(2000); // 2�b�ҋ@
        resultButtons.SetActive(true);
    }

    /// <summary>
    /// ���S���̃��U���g
    /// </summary>
    /// <returns></returns>
    private async UniTask DeadResultStart()
    {
        slideUiDead.SetActive(true);
        // �v���C���[�̓��͂��󂯕t���Ȃ�����i��ŏC���j
        player.GetComponent<PlayerInput>().enabled = false;
        mainPanel.SetActive(false);
        resultPanel.SetActive(true);
        slideUiDead.GetComponent<SlideUiControl>().UiMove();
        await UniTask.Delay(4000); // 4�b�ҋ@
        resultButtons.SetActive(true);
    }
    /// <summary>
    /// �X�R�A�𐮐��ŕԂ��֐�
    /// </summary>
    /// <returns></returns>
    private int Score()
    {
        // TODO:���j���Ŕ���ł���悤��
        int score = 4000 - elapsedTimeInSeconds;
        if (score < 0)
        {
            score = 0;
        }
        return score;
        
    }
}
