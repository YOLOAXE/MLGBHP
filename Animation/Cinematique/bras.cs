using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bras : MonoBehaviour
{
    public GameObject Arms;
	public float VitesseRotation,VitesseDeplacement;
	
    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(Mathf.LerpAngle(gameObject.transform.eulerAngles.x,Arms.transform.eulerAngles.x,Time.deltaTime * VitesseRotation),Mathf.LerpAngle(gameObject.transform.eulerAngles.y,Arms.transform.eulerAngles.y,Time.deltaTime * VitesseRotation),Mathf.LerpAngle(gameObject.transform.eulerAngles.z,Arms.transform.eulerAngles.z,Time.deltaTime * VitesseRotation));
		gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x,Arms.transform.position.x,Time.deltaTime * VitesseDeplacement),Mathf.Lerp(gameObject.transform.position.y,Arms.transform.position.y,Time.deltaTime * VitesseDeplacement),Mathf.Lerp(gameObject.transform.position.z,Arms.transform.position.z,Time.deltaTime * VitesseDeplacement));
    }
}
