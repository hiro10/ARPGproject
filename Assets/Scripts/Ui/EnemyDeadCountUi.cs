using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyDeadCountUi : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyDeadCount;
    [SerializeField] TextMeshProUGUI enemyCreateCount;
    [SerializeField] BattleSceneManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        enemyDeadCount.text = sceneManager.AllEnemyDeadCount.ToString();
        enemyCreateCount.text = sceneManager.SpownCount.ToString();
    }
}
