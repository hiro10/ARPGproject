using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// ���U���g��ʂł�Ui�̓����̃N���X
/// </summary>
public class ResultUiMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Ui�𓮂��������l�ɐݒ�
        gameObject.transform.localPosition = new Vector3(1000f, 0f, 0f);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ���U���g��ʂł̃X�R�A�p�l���̓���
    /// </summary>
    public void ResultScoreUYiMove()
    {
        gameObject.SetActive(true);
        gameObject.transform.DOLocalMoveX(287f,2f).SetLink(gameObject).SetEase(Ease.InOutBack);
    }
}
