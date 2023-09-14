using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMaker : MonoBehaviour
{
    // �I�u�W�F�N�g���f���J����
    [SerializeField] private Camera mainCamera;

    // UI��\��������Ώ�
    public Transform target;

    // �\������UI
    [SerializeField] private Transform targetUI;

    // �I�t�Z�b�g
    [SerializeField] Vector3 worldOffset;

    private RectTransform parent;

    /// <summary>
    /// �J�n����
    /// </summary>
    private void Awake()
    {
        // null�`�F�b�N
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        // �e��Transform���擾
        parent = targetUI.parent.GetComponent<RectTransform>();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        OnUpdatePosition();
    }

    private void OnUpdatePosition()
    {
        var cameraTransform = mainCamera.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;
        // �I�u�W�F�N�g�̈ʒu
        var targetworldPos = target.position + worldOffset;
        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetworldPos - cameraTransform.position;

        // ���ς��g���ăJ�����O�����ǂ���
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�������O���Ȃ�UI�\���A���Ȃ��\��
        targetUI.gameObject.SetActive(isFront);
        if(!isFront)
        {
            return;
        }

        // �I�u�W�F�N�g�̃��[���h���W����X�N���[�����W�̕ϊ�
        var targetScreenPos = mainCamera.WorldToScreenPoint(targetworldPos);

        // �X�N���[�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (parent, targetScreenPos, null, out var uiLocalPos);

        targetUI.localPosition = uiLocalPos;
    }

}
