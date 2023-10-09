using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// �J�b�g�C������̃N���X
/// </summary>
public class CutInManager : MonoBehaviour
{
    // �J�b�g�C���i���C���摜�j
    public GameObject cutInObject;
    // �J�b�g�C���摜�i�w�i�j
    public GameObject cutInBack;
   
    private void Start()
    {
        // �J�b�g�C�����\���ɂ���
        cutInObject.SetActive(false);
        cutInBack.SetActive(false);
    }

    /// <summary>
    /// �J�b�g�C���̓����̊֐�
    /// </summary>
    /// <returns></returns>
    private async UniTask PlayCutInAsync()
    {
        // �J�b�g�C�����E�ォ�璆���Ɉړ�����A�j���[�V����
        cutInBack.SetActive(true);
        cutInObject.SetActive(true);
        cutInObject.transform.position = new Vector3(Screen.width, Screen.height, 0);

        // �E�ォ�璆���Ɉړ�
        await cutInObject.transform.DOLocalMove(new Vector3(0f,0f,0f), 0.5f).SetUpdate(true).AsyncWaitForCompletion();

        // 0.5�b�ҋ@
        await UniTask.Delay(500);

        // �������獶���Ɉړ����Ĕ�\����
        await cutInObject.transform.DOLocalMove(new Vector3(-Screen.width, -Screen.height, 0), 0.5f).SetUpdate(true).AsyncWaitForCompletion();
        cutInObject.SetActive(false);
        cutInBack.SetActive(false);
    }

    // �J�b�g�C�����Đ�
    public async void StoryEventTriggered()
    {

        await PlayCutInAsync(); // �J�b�g�C�����Đ�

    }
}
