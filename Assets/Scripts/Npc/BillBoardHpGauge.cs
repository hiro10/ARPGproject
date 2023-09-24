using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardHpGauge : MonoBehaviour
{
	void Update()
	{
		Vector3 p = Camera.main.transform.position;
		transform.LookAt(p);
	}
}
