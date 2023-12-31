using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NpcVoiceImage : MonoBehaviour
{
    // プレイヤーの位置
    [SerializeField] Transform player;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private float textAppearDuration = 0.15f;
    [SerializeField] private float textDisappearDuration = 0.3f;
    [SerializeField] private float textJumpHeight = 30f;
    [SerializeField] GameObject sword;
    // アニメ
    [SerializeField] Animator animator;

    // 表示する最大距離
    private float maxDistance = 20f;
    // 実際の距離
    private float distance;

    private void Start()
    {
        //textMeshProUGUI.text = string.Empty;
        textMeshProUGUI.DOFade(0, 0);

        if(GameManager.Instance.isGameClear)
        {
            animator.SetBool("GameClear",true);
            textMeshProUGUI.text = "♪〜";
            sword.SetActive(false);
        }
    }

    private void Update()
    {
        //プレイヤーとオブジェクトの距離の計算
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (maxDistance < distance)
        {
            // SetActiveより軽い
            textMeshProUGUI.enabled = false;
        }
        else
        {
            textMeshProUGUI.enabled = true;
        }
    }


    public void TakeDamage()
    {
        textMeshProUGUI.DOFade(0, 0);
        

        var tmpAnimator = new DOTweenTMPAnimator(textMeshProUGUI);

        for (var i = 0; i < tmpAnimator.textInfo.characterCount; i++)
        {
            tmpAnimator.DOScaleChar(i, 0.7f, 0);
            var charOffset = tmpAnimator.GetCharOffset(i);

            var sequence = DOTween.Sequence();

            // 登場
            sequence.Append(tmpAnimator.DOOffsetChar(i, charOffset + new Vector3(0f, textJumpHeight, 0f), textAppearDuration)
                    .SetEase(Ease.OutFlash, 2))
                .Join(tmpAnimator.DOFadeChar(i, 1f, textAppearDuration / 2f))
                .Join(tmpAnimator.DOScaleChar(i, 1f, textAppearDuration)
                    .SetEase(Ease.OutBack))
                .SetDelay(0.05f * i);

            // タイミングを合わせて0.5秒待つ
            sequence.AppendInterval(0.05f * (tmpAnimator.textInfo.characterCount - i))
                .AppendInterval(0.5f);

            // 消滅
            sequence.Append(tmpAnimator.DOFadeChar(i, 0, textDisappearDuration));
            sequence.Join(tmpAnimator.DOOffsetChar(i, charOffset + new Vector3(0, textJumpHeight, 0), textDisappearDuration)
                .SetEase(Ease.Linear));
        }
    }
}