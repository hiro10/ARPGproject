using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボスの効果音管理
public class BossSoundManager : MonoBehaviour
{
    //SE
    [SerializeField] AudioSource audioSourceSE;
    [SerializeField] AudioClip[] audioClipsSE;
    /// <summary>
    /// BGMの列挙型
    /// </summary>
    public enum SE
    {
        RotOn, // ボールが破裂するとき
        RotOff,   // ボールに触れた時
    }
    void Start()
    {
        audioSourceSE.volume = PlayerPrefs.GetFloat("SE_VOLUME",1);
      
    }
    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(SE se)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[(int)se]);

    }
}
