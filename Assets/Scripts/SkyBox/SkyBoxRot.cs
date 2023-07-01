using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxRot : MonoBehaviour
{
    [Range(0.001f, 0.1f)]
    public float rotateSpeed;

    public Material sky;

    float rotationRepeatValue;

    void Update()
    {

        rotationRepeatValue = Mathf.Repeat(sky.GetFloat("_Rotation") + rotateSpeed, 360f);

        sky.SetFloat("_Rotation", rotationRepeatValue);

        //キー押してない間はreturn
        if (Input.anyKey == false)
        {
            return;
        }


        //テスト用 Change
        if (Input.GetKeyDown(KeyCode.C))
        {
            //切り替えたいタイミングでこれを書く
            RenderSettings.skybox = sky;
        }
    }
}
