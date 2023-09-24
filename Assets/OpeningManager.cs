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
        //�e�L�X�g�R���|�[�l���g���擾
        fieldOpText.DOFade(1f, 1f);

    }
    public void FieldOutOpName()
    {
        //�e�L�X�g�R���|�[�l���g���擾
        fieldOpText.DOFade(0f, 1f);

    }
    private void OnDestroy()
    {
        GameManager.Instance.isMovePlaying = false;
        slideUi.UiMove();
        
    }
}
