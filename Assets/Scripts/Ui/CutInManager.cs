using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// カットイン制御のクラス
/// </summary>
public class CutInManager : MonoBehaviour
{
    // カットイン（メイン画像）
    public GameObject cutInObject;
    // カットイン画像（背景）
    public GameObject cutInBack;
   
    private void Start()
    {
        // カットインを非表示にする
        cutInObject.SetActive(false);
        cutInBack.SetActive(false);
    }

    /// <summary>
    /// カットインの動きの関数
    /// </summary>
    /// <returns></returns>
    private async UniTask PlayCutInAsync()
    {
        // カットインを右上から中央に移動するアニメーション
        cutInBack.SetActive(true);
        cutInObject.SetActive(true);
        cutInObject.transform.position = new Vector3(Screen.width, Screen.height, 0);

        // 右上から中央に移動
        await cutInObject.transform.DOLocalMove(new Vector3(0f,0f,0f), 0.5f).SetUpdate(true).AsyncWaitForCompletion();

        // 0.5秒待機
        await UniTask.Delay(500);

        // 中央から左下に移動して非表示に
        await cutInObject.transform.DOLocalMove(new Vector3(-Screen.width, -Screen.height, 0), 0.5f).SetUpdate(true).AsyncWaitForCompletion();
        cutInObject.SetActive(false);
        cutInBack.SetActive(false);
    }

    // カットインを再生
    public async void StoryEventTriggered()
    {

        await PlayCutInAsync(); // カットインを再生

    }
}
