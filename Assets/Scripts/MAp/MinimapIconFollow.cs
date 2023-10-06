using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconFollow : MonoBehaviour
{
    [SerializeField] Transform player; // �v���C���[�I�u�W�F�N�g��Transform�R���|�[�l���g���A�^�b�`

    void Update()
    {
        // �v���C���[��y���ʒu�𖳎����Ax����z����Ǐ]
        Vector3 newPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = newPosition;
    }
}
