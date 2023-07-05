using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SwordCollider : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject hitParticle;
    [SerializeField] BattleSceneManager battleSceneManager;
    //リセット用のタイムカウント

    private void Start()
    {
       // battleSceneManager = GetComponent<BattleSceneManager>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,layerMask))
            {
                //Hit Particle
                battleSceneManager.ComboAnim();
                Instantiate(hitParticle, hit.point, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.DrawRay(ray);
    }
}
