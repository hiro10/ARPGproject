using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkyBoxRot : MonoBehaviour
{
    [Range(0.001f, 0.1f)]
    public float rotateSpeed;

    [SerializeField] private Material[] sky;
    private Material skyBox;

    float rotationRepeatValue;
    [SerializeField] private GameObject[] lights;

    private void Awake()
    {
        for(int i = 0; i<lights.Length;i++)
        {
            lights[i].SetActive(false);
        }
        ChangeSkyBox();
    }

    void Update()
    {
        if(skyBox==null)
        {
            return;
        }
        rotationRepeatValue = Mathf.Repeat(skyBox.GetFloat("_Rotation") + rotateSpeed, 360f);

        skyBox.SetFloat("_Rotation", rotationRepeatValue);

        //キー押してない間はreturn
        if (Input.anyKey == false)
        {
            return;
        }


        //テスト用 Change
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    //切り替えたいタイミングでこれを書く
        //    RenderSettings.skybox = sky;
        //}
    }

    /// <summary>
    /// 時間帯によって背景を変える処理
    /// </summary>
    private void ChangeSkyBox()
    {
        // 夜
        if (DateTime.Now.Hour >= 19 || DateTime.Now.Hour <= 6)
        {
            lights[0].SetActive(true);
            RenderSettings.skybox = sky[0];
        }
        // 夕方
        else if ( DateTime.Now.Hour == 18)
        {
            lights[1].SetActive(true);
            RenderSettings.skybox = sky[1];
        }
        //早朝
        else if(DateTime.Now.Hour == 7)
        {
            lights[2].SetActive(true);
            RenderSettings.skybox = sky[2];
        }
        // 朝、昼
        else
        {
            lights[2].SetActive(true);
            RenderSettings.skybox = sky[3];
        }
        skyBox = RenderSettings.skybox;
    }
}
