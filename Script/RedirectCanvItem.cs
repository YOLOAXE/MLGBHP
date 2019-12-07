using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectCanvItem : MonoBehaviour
{
	public Vector3 TargetPointHaut;
	public float RotationV;
    void Start()
    {
		
    }

    void Update()
    {
		Vector3 targetPoint = TargetPointHaut - transform.position;
		float step = RotationV * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPoint, step, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDir);
    }
}
