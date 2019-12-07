using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnModelRandomPos : MonoBehaviour
{
    public GameObject Pos1,Pos2,SpawnObject;
	public Transform[] ObjecthasSpawn;
	public bool ModeYOnly;
	public int IntervalMin,IntervalMax;
	private int Nb;
	
    void Start()
    {
        Nb = Random.Range(IntervalMin,IntervalMax);
		if(Nb <= 0 || ObjecthasSpawn.Length == 0){Destroy(gameObject);}
        for(int i = 0;i < Nb;i++)
		{
			SpawnObject.transform.position = new Vector3(Random.Range(Pos1.transform.position.x,Pos2.transform.position.x),Random.Range(Pos1.transform.position.y,Pos2.transform.position.y),Random.Range(Pos1.transform.position.z,Pos2.transform.position.z));
			if(!ModeYOnly)
			{
				Instantiate(ObjecthasSpawn[Random.Range(0,ObjecthasSpawn.Length)],SpawnObject.transform.position,Quaternion.Euler(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360)));
			}
			else
			{
				Instantiate(ObjecthasSpawn[Random.Range(0,ObjecthasSpawn.Length)],SpawnObject.transform.position,Quaternion.Euler(0f,Random.Range(0,360),0f));
			}
		}
		Destroy(gameObject);
    }	
}
