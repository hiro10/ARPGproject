using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class MoveForward : MonoBehaviour
{
    public float moveSpeed = 5.0f; // �ړ����x
    [SerializeField] PlayableDirector director;
    private Vector3 originalPosition;
    private bool hasAnimationPlayed = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // �I�u�W�F�N�g�𐳖ʂɈړ�������
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (!hasAnimationPlayed)
        {
            // �����ŃA�j���[�V�����̍Đ���Ԃ��m�F���A�I�������猳�̈ʒu�ɖ߂��������s��
            director = GetComponent<PlayableDirector>();

            if (director != null && director.state == PlayState.Paused)
            {
                // �A�j���[�V�������I�������ꍇ�A���̈ʒu�ɖ߂�
                transform.position = originalPosition;
                hasAnimationPlayed = true;
            }
        }
       
    }
}
