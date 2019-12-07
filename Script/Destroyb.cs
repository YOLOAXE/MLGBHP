using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolGameObject
{
	public GameObject[] ObjectRD;
}
public class Destroyb : MonoBehaviour
{
    public BoolGameObject[] Objectb;
	
    void Start()
    {
       for(int i = 0;i < Objectb.Length;i++)
	   {
			Destroy(Objectb[i].ObjectRD[Random.Range(0,Objectb[i].ObjectRD.Length)]);
	   }
    }

}
