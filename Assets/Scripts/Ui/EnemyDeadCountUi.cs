using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyDeadCountUi : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyDeadCount;
    [SerializeField] TextMeshProUGUI enemyCreateCount;
    [SerializeField] TextMeshProUGUI clearAreaCount;
    [SerializeField] BattleSceneManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // 撃破ステータスの更新
    void Update()
    {
        enemyDeadCount.text = sceneManager.AllEnemyDeadCount.ToString();
        enemyCreateCount.text = sceneManager.SpownCount.ToString();
        clearAreaCount.text = sceneManager.ClearAreaCount.ToString();
    }
}
