using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class VillageSceneManager : MonoBehaviour
{
    private bool goBattle;
    [SerializeField] Renderer gate;
    [SerializeField] UiMaker mainIcon;
    [SerializeField] GameObject gateObj;
    [SerializeField] DistanceCalculator distanceCalculator;
    [SerializeField] SlideUiControl slideUi;
    [SerializeField] GameObject villageNameText;
    // �Q�[���I�[�o�[�X���C�hUI
    [SerializeField] GameObject slideUiGameOver;
    // �Q�[���N���A�X���C�hUI
    [SerializeField] GameObject slideUiGameClear;
    // �v���C���[
    [SerializeField] GameObject player;
    // �Q�[���I�[�o�[���̔w�i
    [SerializeField] GameObject gameOverPanel;
    // ���U���g�{�^��
    [SerializeField] GameObject resultButtons;
    // �Q�[���I�[�o�[�e�L�X�g
    [SerializeField] TextMeshProUGUI gameOverText;
    // �Q�[���I�[�o�[�e�L�X�g
    [SerializeField] TextMeshProUGUI gameClearText;
    // ��͗l
    [SerializeField] List<GameObject> sky = new List<GameObject>();
    // ������G�t�F�N�g
    [SerializeField] GameObject kamifubukiEffect;

    // ���U���g�����̔���
    private bool resultOn;
    Image image;
    Color color;
    void Start()
    {
        if (GameManager.Instance.isGameClear == false)
        {
            sky[0].SetActive(true);

            sky[1].SetActive(false);

            kamifubukiEffect.SetActive(false);
            SoundManager.instance.PlayBGM(SoundManager.BGM.Town);
        }
        else
        {
            sky[0].SetActive(false);

            sky[1].SetActive(true);

            kamifubukiEffect.SetActive(true);

            SoundManager.instance.PlayBGM(SoundManager.BGM.TownClear);
        }
        // ���̖��O�������Ă����NPC�ɘb�����������ŕ\���̗L����ς���
        if (GameManager.Instance.onTownName)
        {
            villageNameText.SetActive(true);
        }
        else
        {
            villageNameText.SetActive(false);
        }
        slideUi.UiMove();
        goBattle = false;
        
        slideUiGameOver.SetActive(false);
        slideUiGameClear.SetActive(false);

        image = gameOverPanel.GetComponent<Image>();
        color = image.color;
        color.a = 0f;
        gameOverPanel.SetActive(false);
        resultButtons.SetActive(false);
        gameOverText.alpha = 0f;
        gameClearText.alpha = 0f;
    }
    public void GotoBattleScene()
    {
        distanceCalculator.targetObject = gateObj.transform;
        mainIcon.targets[0] = gateObj.transform;
        goBattle = true;
        gate.material.color = Color.green;
    }

    public bool GetgoBattle()
    {
        return goBattle;
    }

    public void OpenName()
    {
        GameManager.Instance.onTownName = true;
        villageNameText.SetActive(true);
    }

    public async void StartGameOverResult()
    {
        // ������ResultStart���\�b�h���Ăяo���A�񓯊��ҋ@����
        await GameOverResult();
    }

    public async void StartGameClearResult()
    {
        // ������ResultStart���\�b�h���Ăяo���A�񓯊��ҋ@����
        await GameClearResult();
    }

    /// <summary>
    /// �Q�[���I�[�o�[���̃��U���g
    /// </summary>
    /// <returns></returns>
    private async UniTask GameOverResult()
    {
        // �v���C���[�̓��͂��󂯕t���Ȃ�����i��ŏC���j
        player.GetComponent<PlayerInput>().enabled = false;

        GameManager.Instance.isGameOver = true;
        gameOverPanel.SetActive(true);
        image.DOFade(1.0f, 2f);
        await UniTask.Delay(1000); // 1�b�ҋ@
        gameOverText.DOFade(1.0f, 2f);
        await UniTask.Delay(4000); // 4�b�ҋ@
        gameOverText.DOFade(0.0f, 2f);
        await UniTask.Delay(1000); // 4�b�ҋ@
        slideUiGameOver.SetActive(true);
       
        slideUiGameOver.GetComponent<SlideUiControl>().UiMove();
        await UniTask.Delay(4000); // 4�b�ҋ@
        resultButtons.SetActive(true);
    }

    private async UniTask GameClearResult()
    {
        // �v���C���[�̓��͂��󂯕t���Ȃ�����i��ŏC���j
        player.GetComponent<PlayerInput>().enabled = false;

        gameOverPanel.SetActive(true);
        image.DOFade(1.0f, 2f);
        await UniTask.Delay(1000); // 1�b�ҋ@
        gameClearText.DOFade(1.0f, 2f);
        await UniTask.Delay(4000); // 4�b�ҋ@
        gameClearText.DOFade(0.0f, 2f);
        await UniTask.Delay(1000); // 4�b�ҋ@
        slideUiGameClear.SetActive(true);

        slideUiGameClear.GetComponent<SlideUiControl>().UiMove();
        await UniTask.Delay(4000); // 4�b�ҋ@
        resultButtons.SetActive(true);
    }
}
