using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 戦闘エリアに入ったときに戦闘エリアを展開する処理
public class InverseCollider : MonoBehaviour
{
    // サイズ
    [SerializeField] private float colliderSize;
    
    // エリアが展開しているかどうか
    private bool isActivated = false;
    
    // 生成するシリンダー格納用
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
        // Cylinderの生成
        colliderObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        colliderObject.transform.position = transform.position;
        colliderObject.transform.SetParent(transform);
        colliderObject.transform.localScale = new Vector3(colliderSize, colliderSize, colliderSize);
        colliderObject.tag = "Ground";
       // シネマシーン用、レイヤーで判定のため
       colliderObject.gameObject.layer = 8;

        // Colliderオブジェクトの描画は不要なのでRendererを消す
        Destroy(colliderObject.GetComponent<MeshRenderer>());

        // 元々存在するColliderを削除
        Collider[] colliders = colliderObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i]);
        }

        // メッシュの面を逆にしてからMeshColliderを設定
        var mesh = colliderObject.GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        colliderObject.AddComponent<MeshCollider>();
    }

   
}