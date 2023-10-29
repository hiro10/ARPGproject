using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;
using TMPro;
/// <summary>
/// 戦闘シーンのopmoveの管理クラス
/// </summary>
public class OpeningManager : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject skipButton;
    [SerializeField] GameObject tentativePlayer;
    [SerializeField] TextMeshProUGUI fieldOpText;
    [SerializeField] SlideUiControl slideUi;
   
    void Start()
    {
        GameManager.Instance.isMovePlaying = true;
        playerCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDone()==false)
        {
            playerCam.SetActive(true);
          
        }
    }

    public bool IsDone()
    {
        return playableDirector.time >= playableDirector.duration;
    }

    //ムービーのスキップボタンの処理
    public void OnClickOpeningMoveSkip()
    {
        Destroy(skipButton, 0.1f);
        Destroy(fieldOpText);
        Destroy(tentativePlayer);
        gameObject.SetActive(false);
        Destroy(gameObject, 0.1f);

    }
    public void FieldInOpName()
    {
        //テキストコンポーネントを取得(場所の名前を表示)
        fieldOpText.DOFade(1f, 1f);

    }

    public void FieldOutOpName()
    {
        // テキストをフェードアウトさせる
        fieldOpText.DOFade(0f, 1f);

    }
    private void OnDestroy()
    {
        GameManager.Instance.isMovePlaying = false;
        slideUi.UiMove();

    }
}
