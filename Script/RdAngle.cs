using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RdAngle : MonoBehaviour
{

    void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,RandomAngle(),transform.eulerAngles.z);
    }
	int RandomAngle()
	{
		int rd = Random.Range(0,3);
		if(rd == 0){return 0;}
		else if(rd == 1){return 90;}
		else if(rd == 2){return 270;}
		else{return 0;} 
	}
}
