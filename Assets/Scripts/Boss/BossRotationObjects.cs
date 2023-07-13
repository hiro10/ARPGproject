using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossRotationObjects : MonoBehaviour
{
    [SerializeField] List<GameObject> rotObjects;
    [SerializeField] GameObject boss;
    [Header("Prefabs")]
    public GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(OnBossRotationObj());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator  OnBossRotationObj()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(particle, boss.transform.position, Quaternion.identity);

        for (int i = 0; i < rotObjects.Count; i++)
        {
            rotObjects[i].GetComponent<RotateUnit>().OnRoitationWepons();
        }
       

    }

}