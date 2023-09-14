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
    void Start()
    {
        slideUi.UiMove();
        goBattle = false;
    }
    public void GotoBattleScene()
    {
        distanceCalculator.targetObject = gateObj.transform;
        mainIcon.target = gateObj.transform;
        goBattle = true;
        gate.material.color = Color.green;
    }

    public bool GetgoBattle()
    {
        return goBattle;
    }
}
