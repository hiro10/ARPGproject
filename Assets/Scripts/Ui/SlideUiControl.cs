using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//�X�N���[��UI�̏���
public class SlideUiControl : MonoBehaviour
{
    // Ui�̏��
    public int scrollUIState = 0;
    public bool loop = false;

    // Ui�̈ړ�����ʒu���W
    // UI�̈ړ��J�n�ʒu
    [Header("�J�n�ʒu")]
    public Vector3 startPos;
    // UI�̒�~�ʒu
    [Header("��~�ʒu")]
    public Vector3 inPos;
    // UI�̈ړ��I���ʒu
    [Header("�ړ��I���ʒu")]
    public Vector3 endPos;

    Image image;

    // ���ߒl
    [Header("�w�i���ߒl")]
    [SerializeField] float alpha = 0.75f;

    private void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
       
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        // �J�n
        if(scrollUIState==0)
        {
            // �ړ��Ǘ�
            if(transform.localPosition!= startPos)
            {
                transform.localPosition = startPos;
            }
            // ���ߊǗ�(�摜������Ȃ�)
            if(image)
            {
                // �w�i�𓧖���
                if(image.color.a>0.05f)
                {
                    image.color = new Color(0, 0, 0, 0.002f);
                }
                else
                {
                    image.color = new Color(0, 0, 0, 0);
                }
            }
        }
        // �X���C�h�C��
        else if(scrollUIState==1)
        {
            if(transform.localPosition.x>inPos.x-1.0f&&
                transform.localPosition.y > inPos.y - 1.0f&&
                transform.localPosition.z > inPos.z - 1.0f)
            {
                transform.localPosition = inPos;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, inPos, 4.0f * Time.unscaledDeltaTime);
            }
            
            // �w�i��s������
            if(image)
            {
                if(image.color.a<alpha)
                {
                    image.color +=new Color(0, 0, 0, 0.01f);
                }
                else
                {
                    image.color = new Color(0, 0, 0, alpha);
                }
            }
        }
        // �X���C�h�A�E�g
        else if(scrollUIState==2)
        {
            if (transform.localPosition != endPos)
            {
                if (transform.localPosition.x > endPos.x - 1.0f &&
                    transform.localPosition.y > endPos.y - 1.0f &&
                    transform.localPosition.z > endPos.z - 1.0f)
                {
                    transform.localPosition = endPos;
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, 2.0f * Time.unscaledDeltaTime);
                }

                // ���ߊǗ�(�摜������Ȃ�)
                if (image)
                {
                    // �w�i�𓧖���
                    if (image.color.a > 0.05f)
                    {
                        image.color -= new Color(0, 0, 0, 0.01f);
                    }
                    else
                    {
                        image.color = new Color(0, 0, 0, 0);
                    }
                }
            }
            else
            {
                if(loop)
                {
                    ScrollReset();
                }
            }
        }
    }

    /// <summary>
    /// �X�N���[����Ԃ̏�����
    /// </summary>
    private void ScrollReset()
    {
        scrollUIState = 0;
    }

    public void UiMove()
    {
        StartCoroutine(StateUiMove());
    }

    /// <summary>
    /// UI�̈ړ��̎��ԊǗ�
    /// </summary>
    /// <returns></returns>
    IEnumerator StateUiMove()
    {
        yield return new WaitForSeconds(1f);
        scrollUIState = 1;
        yield return new WaitForSeconds(3f);
        scrollUIState = 2;
        yield return new WaitForSeconds(1f);
        //StartCoroutine(StateUiMove());
    }
}
