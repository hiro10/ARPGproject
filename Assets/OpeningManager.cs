using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;
using TMPro;
public class OpeningManager : MonoBehaviour
{
    [SerializeField] PlayableDirector _playableDirector;
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject skipButton;
    [SerializeField] GameObject tentativePlayer;
    [SerializeField] TextMeshProUGUI fieldOpText;
    // Start is called before the first frame update
    void Start()
    {
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
        return _playableDirector.time >= _playableDirector.duration;
    }
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
        //テキストコンポーネントを取得
        fieldOpText.DOFade(1f, 1f);

    }
    public void FieldOutOpName()
    {
        //テキストコンポーネントを取得
        fieldOpText.DOFade(0f, 1f);

    }
}
