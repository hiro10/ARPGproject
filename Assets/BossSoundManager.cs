using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �{�X�̌��ʉ��Ǘ�
public class BossSoundManager : MonoBehaviour
{
    //SE
    [SerializeField] AudioSource audioSourceSE;
    [SerializeField] AudioClip[] audioClipsSE;
    /// <summary>
    /// BGM�̗񋓌^
    /// </summary>
    public enum SE
    {
        RotOn, // �{�[�����j�􂷂�Ƃ�
        RotOff,   // �{�[���ɐG�ꂽ��
    }
    void Start()
    {
        audioSourceSE.volume = PlayerPrefs.GetFloat("SE_VOLUME",1);
      
    }
    /// <summary>
    /// SE�̍Đ�
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(SE se)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[(int)se]);

    }
}
