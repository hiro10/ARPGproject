using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VillageSceneManager : MonoBehaviour
{
    private bool goBattle;
    [SerializeField] Renderer gate;
    [SerializeField] UiMaker mainIcon;
    [SerializeField] GameObject gateObj;
    [SerializeField] DistanceCalculator distanceCalculator;
    [SerializeField] SlideUiControl slideUi;
    [SerializeField] GameObject villageNameText;
    
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Town);
        slideUi.UiMove();
        goBattle = false;
        villageNameText.SetActive(false);
    }
    public void GotoBattleScene()
    {
        distanceCalculator.targetObject = gateObj.transform;
        mainIcon.targets[0] = gateObj.transform;
        goBattle = true;
        gate.material.color = Color.green;
    }

    public bool GetgoBattle()
    {
        return goBattle;
    }

    public void OpenName()
    {
        villageNameText.SetActive(true);
    }
}
