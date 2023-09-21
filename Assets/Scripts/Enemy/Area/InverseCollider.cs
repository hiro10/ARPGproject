using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// �퓬�G���A�ɓ������Ƃ��ɐ퓬�G���A��W�J���鏈��
public class InverseCollider : MonoBehaviour
{
    // �T�C�Y
    [SerializeField] private float colliderSize;
    
    // �G���A���W�J���Ă��邩�ǂ���
    private bool isActivated = false;
    
    // ��������V�����_�[�i�[�p
    private GameObject colliderObject;

    [SerializeField] GameObject Area;

    [SerializeField] GameObject beforeEnemySpownPos;

    [SerializeField] BattleSceneManager sceneManager;
    private void Start()
    {
        Area.SetActive(false);
    }
    private void OnTriggerEnter(Collider c)
    {

        if (!isActivated && c.gameObject.CompareTag("Player"))
        {
            Destroy(beforeEnemySpownPos);
            isActivated = true;
            sceneManager.InBattleArea = true;
            Area.SetActive(true);
            CreateInverseCollider();
        }
    }

    private void CreateInverseCollider()
    {
        // Cylinder�̐���
        colliderObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        colliderObject.transform.position = transform.position;
        colliderObject.transform.SetParent(transform);
        colliderObject.transform.localScale = new Vector3(colliderSize, colliderSize, colliderSize);
        colliderObject.tag = "Ground";
       // �V�l�}�V�[���p�A���C���[�Ŕ���̂���
       colliderObject.gameObject.layer = 8;

        // Collider�I�u�W�F�N�g�̕`��͕s�v�Ȃ̂�Renderer������
        Destroy(colliderObject.GetComponent<MeshRenderer>());

        // ���X���݂���Collider���폜
        Collider[] colliders = colliderObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i]);
        }

        // ���b�V���̖ʂ��t�ɂ��Ă���MeshCollider��ݒ�
        var mesh = colliderObject.GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        colliderObject.AddComponent<MeshCollider>();
    }

   
}