using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//スクロールUIの処理
public class SlideUiControl : MonoBehaviour
{
    // Uiの状態
    public int scrollUIState = 0;
    public bool loop = false;

    // Uiの移動する位置座標
    // UIの移動開始位置
    [Header("開始位置")]
    public Vector3 startPos;
    // UIの停止位置
    [Header("停止位置")]
    public Vector3 inPos;
    // UIの移動終了位置
    [Header("移動終了位置")]
    public Vector3 endPos;

    Image image;

    // 透過値
    [Header("背景透過値")]
    [SerializeField] float alpha = 0.75f;

    private void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
       
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 開始
        if(scrollUIState==0)
        {
            // 移動管理
            if(transform.localPosition!= startPos)
            {
                transform.localPosition = startPos;
            }
            // 透過管理(画像があるなら)
            if(image)
            {
                // 背景を透明に
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
        // スライドイン
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
            
            // 背景を不透明に
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
        // スライドアウト
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

                // 透過管理(画像があるなら)
                if (image)
                {
                    // 背景を透明に
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
    /// スクロール状態の初期化
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
    /// UIの移動の時間管理
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
