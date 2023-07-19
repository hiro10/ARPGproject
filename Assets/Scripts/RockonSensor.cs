using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RockonSensor : MonoBehaviour
{
    // ロックオンターゲット
    public GameObject nowTarget;

    // センサー内の敵格納
    [SerializeField] private List<GameObject> enemyList;

    private CameraController cameraController;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();

        nowTarget = null;
        enemyList = new List<GameObject>();
    }

    private void Update()
    {
        // リストがからなら
        if (enemyList.Count == 0)
        {
            nowTarget = null;
            return;
        }
        else if (enemyList.Count != 0 && nowTarget == null)
        {
            SetNowTarget();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && !enemyList.Contains(other.gameObject))
        {
            // リストになければ追加
            enemyList.Add(other.gameObject);
            if (nowTarget = null)
            {
                nowTarget = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && enemyList.Contains(other.gameObject))
        {
            if (other.gameObject == nowTarget)
            {
                nowTarget = null;
            }
            enemyList.Remove(other.gameObject);
        }
    }

    public GameObject GetNowTarget()
    {
        return nowTarget;
    }

    public void SetNowTarget()
    {
        foreach (var enemy in enemyList)
        {
            if (nowTarget==null)
            {
                nowTarget = enemy;
            }
        }
    }

    public void RockOnSwitch(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(enemyList.IndexOf(nowTarget)!=enemyList.Count-1)
            {
                nowTarget = enemyList[enemyList.IndexOf(nowTarget) + 1];
            }
            else 
            {
                nowTarget = enemyList[0];
            }
           
        }
    }
}