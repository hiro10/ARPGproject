using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// �ړI�n�ɃA�C�R����\��
/// </summary>
public class UiMaker : MonoBehaviour
{
    // �I�u�W�F�N�g���f���J����
    [SerializeField] private Camera mainCamera;

    // UI��\��������Ώ�
    [SerializeField] public List<Transform> targets = new List<Transform>();

    // �\������UI
    [SerializeField] private GameObject targetUIPrefab;

    // �I�t�Z�b�g
    [SerializeField] Vector3 worldOffset;

    public RectTransform parent;

    // �C���X�^���X������UI���i�[���郊�X�g
    private List<Transform> instantiatedUIs = new List<Transform>();

    
    // TextMeshProUGUI�I�u�W�F�N�g
    public List<TextMeshProUGUI> distanceText=new List<TextMeshProUGUI>();
    [SerializeField] Transform playerTransform;


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
        //parent = targetUIPrefab.transform.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        // �^�[�Q�b�g��������UI���C���X�^���X�����ă��X�g�ɒǉ�
        for (int i = 0; i < targets.Count; i++)
        {
            GameObject uiInstance = Instantiate(targetUIPrefab, parent);
            distanceText.Add( uiInstance.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>());
            instantiatedUIs.Add(uiInstance.transform);
        }
    }


    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        CheckTargets();
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                // �^�[�Q�b�g�� null �ɂȂ����ꍇ�A��������X�g�����菜��
                targets.RemoveAt(i);
                i--; // ���X�g�̗v�f���폜�����̂ŁA�C���f�b�N�X���f�N�������g����
                continue;
            }
            OnUpdatePosition(i);
        }

    }

    private void CheckTargets()
    {
        // �^�[�Q�b�g�����������ꍇ��UI���폜
        if (targets.Count < instantiatedUIs.Count)
        {
            for (int i = targets.Count; i < instantiatedUIs.Count; i++)
            {
                Destroy(instantiatedUIs[i].gameObject);
            }
            instantiatedUIs.RemoveRange(targets.Count, instantiatedUIs.Count - targets.Count);
        }
    }


    private void OnUpdatePosition(int index)
    {
        UpdateDistanceText();

        var cameraTransform = mainCamera.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;

        // �I�u�W�F�N�g�̈ʒu
        var targetworldPos = targets[index].position + worldOffset;
        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetworldPos - cameraTransform.position;

        // ���ς��g���ăJ�����O�����ǂ���
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�������O���Ȃ�UI�\���A���Ȃ��\��
        instantiatedUIs[index].gameObject.SetActive(isFront);
        if (!isFront)
        {
            return;
        }

        // �I�u�W�F�N�g�̃��[���h���W����X�N���[�����W�̕ϊ�
        var targetScreenPos = mainCamera.WorldToScreenPoint(targetworldPos);

        // �X�N���[�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (parent, targetScreenPos, null, out var uiLocalPos);

        instantiatedUIs[index].localPosition = uiLocalPos;
        
    }

    private void UpdateDistanceText()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            // �^�[�Q�b�g�I�u�W�F�N�g���ݒ肳��Ă���ꍇ�̂ݏ������s��
            if (targets[i] != null)
            {

                // �v���C���[�i���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�j����^�[�Q�b�g�I�u�W�F�N�g�܂ł̋������v�Z
                float distance = Vector3.Distance(playerTransform.position, targets[i].position);

                // ���������[�g���ɕϊ����Đ����ɕϊ�
                int distanceInMeters = Mathf.RoundToInt(distance);

                // TextMeshProUGUI�ɋ�����\��
                distanceText[i].text = distanceInMeters.ToString() + " m"; // �����_�ȉ�2���ŕ\��
            }
        }
    }
}
