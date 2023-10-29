using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの足音を鳴らすクラス
/// </summary>
public class FootSePlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] float pitchRange = 0.1f;
    protected AudioSource source;
    [SerializeField] PlayerController player;
    private void Awake()
    {
        source = GetComponents<AudioSource>()[0];
    }

    /// <summary>
    /// プレイヤーの足音を鳴らす
    /// アニメーションクリップとして使用
    /// TODO:プレイヤー処理変更時に更新
    /// </summary>
    public void PlayWalkFootstepSE()
    {
        if ((player.move.magnitude > 0.1f&& player.move.magnitude <= 0.5f) && player.isGrounded)
        {
            source.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
            source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
      
    }

    public void PlayRunFootstepSE()
    {
        if (player.move.magnitude > 0.5f && player.isGrounded)
        {
            source.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
            source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }
    public void PlayJumpFootstepSE()
    {
        if (player.isGrounded)
        {
            source.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
            source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }
}