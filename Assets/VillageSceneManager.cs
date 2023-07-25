using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VillageSceneManager : MonoBehaviour
{
    private bool goBattle;
    [SerializeField] Renderer gate;
    void Start()
    {
        goBattle = false;
    }
    public void GotoBattleScene()
    {
        goBattle = true;
        gate.material.color = Color.green;
    }

    public bool GetgoBattle()
    {
        return goBattle;
    }
}
