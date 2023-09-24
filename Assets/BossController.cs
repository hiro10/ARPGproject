using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] BattleSceneManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager.InBossBattleArea = false;
        GameManager.Instance.isMovePlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
