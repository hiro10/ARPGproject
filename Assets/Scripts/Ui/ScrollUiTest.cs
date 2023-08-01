using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUiTest : MonoBehaviour
{
    [SerializeField] SlideUiControl uiControl;
    [SerializeField] SlideUiControl uiControl3;
    // Start is called before the first frame update
    void Start()
    {
        uiControl.UiMove();
        uiControl3.UiMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
