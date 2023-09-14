using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardScaller : MonoBehaviour
{
    // �v���C���[�̈ʒu
    [SerializeField] Transform player;

    // �v���C���[�Ƃ̋���
    // �ŏ�
    private float minDistance = 5f;
    // �ő�
    private float maxDistance = 20f;

    // �T�C�Y
    // �ŏ�
    private float minSize = 0.1f;
    // �ő�
    private float maxSize = 0.2f;

    // ���ۂ̃T�C�Y
    private float distance;

    private float scallFactor;

    Renderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<Renderer>();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        //�v���C���[�ƃI�u�W�F�N�g�i�r���r���{�[�h�j�̋����̌v�Z
        distance = Vector3.Distance(transform.position, player.transform.position);

        // �����ɉ����đ傫����ύX
        scallFactor = Mathf.Clamp((distance - minDistance) / (maxDistance - minDistance), 0f, 1f);

        float size = Mathf.Lerp(minSize, maxSize, scallFactor);
        transform.localScale = new Vector3(size, size, size);

        if(maxDistance<distance)
        {
            // SetActive���y��
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }


}
