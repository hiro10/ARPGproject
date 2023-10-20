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
    // �v���C���[
    [SerializeField] GameObject player;
    // �Q�[���I�[�o�[���̔w�i
    [SerializeField] GameObject gameOverPanel;
    // ���U���g�{�^��
    [SerializeField] GameObject resultButtons;
    // �Q�[���I�[�o�[�e�L�X�g
    [SerializeField] TextMeshProUGUI gameOverText;
    // ���U���g�����̔���
    private bool resultOn;
    Image image;
    Color color;
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Town);
        slideUi.UiMove();
        goBattle = false;
        villageNameText.SetActive(false);
        slideUiGameOver.SetActive(false);
       
        image = gameOverPanel.GetComponent<Image>();
        color = image.color;
        color.a = 0f;
        gameOverPanel.SetActive(false);
        resultButtons.SetActive(false);
        gameOverText.alpha = 0f;
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
        villageNameText.SetActive(true);
    }

    public async void StartGameOverResult()
    {
        // ������ResultStart���\�b�h���Ăяo���A�񓯊��ҋ@����
        await GameOverResult();
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
}
