using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲーム終了時の処理
public class CloseGame : MonoBehaviour
{
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
