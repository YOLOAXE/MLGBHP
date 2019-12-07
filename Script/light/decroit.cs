using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decroit : MonoBehaviour
{
    public float Range;
	public float RangeTDecroit,MultipleP;
	
	void Start()
	{
		RangeTDecroit = GetComponent<Light>().range;
		RangeTDecroit = 1;
		if(MultipleP > 1){MultipleP = 1;}
	}
    void Update()
    { 
		if(RangeTDecroit > 0)
		{
			RangeTDecroit -= Time.deltaTime * MultipleP;
			GetComponent<Light>().range = RangeTDecroit * Range;
		}
    }
}
