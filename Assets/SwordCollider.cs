using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject hitParticle;
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

    // çUåÇÇ™ìñÇΩÇ¡ÇΩÇ©Çï‘Ç∑ä÷êî
}
