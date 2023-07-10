using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBreak : MonoBehaviour
{
    [SerializeField] private bool useGravity = true;                            // 重力を有効にするかどうか
    [SerializeField] private Vector3 explodeVel = new Vector3(0, 0, 0.1f);      // 爆発の中心地
    [SerializeField] private float explodeForce = 200f;                         // 爆発の威力
    [SerializeField] private float explodeRange = 10f;                          // 爆発の範囲
    private Rigidbody[] rigidBodies;
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject uiCanvas;


    void Start()
    {
        uiCanvas.SetActive(false);
        playerCam.SetActive(false);
        rigidBodies = GetComponentsInChildren<Rigidbody>();                     // 子(破片)のRigidbodyを取得しておく
        StartCoroutine("BreakStart");                                           // 動作にディレイを掛けるためコルーチンを使用
    }

    IEnumerator BreakStart()
    {
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = false;
            rb.useGravity = useGravity;
            rb.AddExplosionForce(explodeForce / 5, transform.position + explodeVel, explodeRange);
        }
        yield return new WaitForSeconds(0.02f);                                 // 一瞬動かすことでひび割れを演出

        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = true;
        }
        yield return new WaitForSeconds(0.8f);

        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(explodeForce, transform.position + explodeVel, explodeRange);
        }
      
        playerCam.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        uiCanvas.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
   
}