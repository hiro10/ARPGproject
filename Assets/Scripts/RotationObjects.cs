using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotationObjects : MonoBehaviour
{
    [SerializeField] List<GameObject> rotObjects;
    [SerializeField]bool rotationObj;
    [SerializeField] GameObject player;
    [Header("Prefabs")]
    public GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
        rotationObj = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnROtationObj(InputAction.CallbackContext context)
    {
        Debug.Log("‰Ÿ‚³‚ê‚½");
        if (context.started)
        {
            Instantiate(particle, player.transform.position, Quaternion.identity);
            if (rotationObj == false)
            {
                rotationObj = true;
                for (int i = 0; i < rotObjects.Count; i++)
                {
                    rotObjects[i].GetComponent<RotateUnit>().OnRoitationWepons();
                }


            }
            else if (rotationObj == true)
            {

                rotationObj = false;
                for (int i = 0; i < rotObjects.Count; i++)
                {
                    rotObjects[i].GetComponent<RotateUnit>().OffRoitationWepons();
                }

            }
        }
    }
}