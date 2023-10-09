using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

// �_���[�W���󂯂����ɃL�����o�X�Ƀ_���[�W���l���o���N���X
public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private float textAppearDuration = 0.15f;
    [SerializeField] private float textDisappearDuration = 0.3f;
    [SerializeField] private float textJumpHeight = 30f;

    private void Start()
    {
        textMeshProUGUI.text = string.Empty;
        textMeshProUGUI.DOFade(0, 0);
    }

    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Wepon")
        {
            float damage = Random.Range(10, 20);
            TakeDamage((int)damage);
        }
    }

    private void TakeDamage(int damage)
    {
        textMeshProUGUI.DOFade(0, 0);
        textMeshProUGUI.text = damage.ToString();

        var tmpAnimator = new DOTweenTMPAnimator(textMeshProUGUI);

        for (var i = 0; i < tmpAnimator.textInfo.characterCount; i++)
        {
            tmpAnimator.DOScaleChar(i, 0.7f, 0);
            var charOffset = tmpAnimator.GetCharOffset(i);

            var sequence = DOTween.Sequence();

            // �o��
            sequence.Append(tmpAnimator.DOOffsetChar(i, charOffset + new Vector3(0f, textJumpHeight, 0f), textAppearDuration)
                    .SetEase(Ease.OutFlash, 2))
                .Join(tmpAnimator.DOFadeChar(i, 1f, textAppearDuration / 2f))
                .Join(tmpAnimator.DOScaleChar(i, 1f, textAppearDuration)
                    .SetEase(Ease.OutBack))
                .SetDelay(0.05f * i);

            // �^�C�~���O�����킹��0.5�b�҂�
            sequence.AppendInterval(0.05f * (tmpAnimator.textInfo.characterCount - i))
                .AppendInterval(0.5f);

            // ����
            sequence.Append(tmpAnimator.DOFadeChar(i, 0, textDisappearDuration));
            sequence.Join(tmpAnimator.DOOffsetChar(i, charOffset + new Vector3(0, textJumpHeight, 0), textDisappearDuration)
                .SetEase(Ease.Linear));
        }
    }
}
