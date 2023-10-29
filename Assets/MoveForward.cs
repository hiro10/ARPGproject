using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class MoveForward : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 移動速度
    [SerializeField] PlayableDirector director;
    private Vector3 originalPosition;
    private bool hasAnimationPlayed = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // オブジェクトを正面に移動させる
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (!hasAnimationPlayed)
        {
            // ここでアニメーションの再生状態を確認し、終了したら元の位置に戻す処理を行う
            director = GetComponent<PlayableDirector>();

            if (director != null && director.state == PlayState.Paused)
            {
                // アニメーションが終了した場合、元の位置に戻す
                transform.position = originalPosition;
                hasAnimationPlayed = true;
            }
        }
       
    }
}
