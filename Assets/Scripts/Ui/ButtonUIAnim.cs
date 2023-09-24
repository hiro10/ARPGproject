using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonUIAnim : UIBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 BaseScale;

    protected override void Start()
    {

        BaseScale = transform.localScale;
    }


    public void OnPointerDown
        (PointerEventData eventData)
    {
        transform.DOScale(BaseScale * 0.8f, 0.25f).SetUpdate(true)
       .Play();
    }

    public void OnPointerUp
        (PointerEventData eventData)
    {
        transform.DOScale(BaseScale, 0.25f).SetUpdate(true)
       .Play();
    }

}
