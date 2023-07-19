using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RockonSensor : MonoBehaviour
{
    // ���b�N�I���^�[�Q�b�g
    public GameObject nowTarget;

    // �Z���T�[���̓G�i�[
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
        // ���X�g������Ȃ�
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
            // ���X�g�ɂȂ���Βǉ�
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