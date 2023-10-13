using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI�p�A�E�g���C���̎g�p�A���g�p�̐؂�ւ� 
public class PlayerAwakeOutLineManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject awakeSliderOutLine;
    void Start()
    {
        awakeSliderOutLine.GetComponent<UIOutline>().enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerData>().PlayerCurrentAwake >= 100&&!player.GetComponent<PlayerController>().IsAwakening)
        {
            awakeSliderOutLine.GetComponent<UIOutline>().enabled = true;
        }
        else if(player.GetComponent<PlayerController>().IsAwakening)
        {
            awakeSliderOutLine.GetComponent<UIOutline>().enabled = true;
        }
        else
        {
            awakeSliderOutLine.GetComponent<UIOutline>().enabled = false;
        }
    }
}
