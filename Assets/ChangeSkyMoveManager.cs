using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChangeSkyMoveManager : MonoBehaviour
{
    [SerializeField] private Material _mat;

   // [SerializeField] private Color _Maincol;

    private float _smoothness=0f;
    float startValue = 0f; // ŠJŽn’l
    // Start is called before the first frame update
    void Start()
    {
        // DOTween.To(() => _smoothness, x => startValue = x, 2f, 5f);
        GameManager.Instance.isMovePlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        _smoothness += 0.25f*Time.deltaTime;
        _mat.SetFloat("_CloudPawer", _smoothness);
    }

    private void OnDestroy()
    {
        GameManager.Instance.isMovePlaying = false;
        _mat.SetFloat("_CloudPawer", 0);
    }
}
