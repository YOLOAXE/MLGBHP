using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
 public class Odrs
 {
     public GameObject[] Object;
 }
public class RoomManager : MonoBehaviour
{
	public GameObject[] cadreSpawnObject,Sol;
	public  Odrs[] objectDoubleRandomSuprimer;
	public Transform[] content; 
	public Transform spawnEnterRoom,spawnExitRoom;
	[HideInInspector]
	public Transform spawnExitBoss;
	public Transform spawnMob;
	public int SpawnState;
	
    void Start()
    {
		StartCoroutine(Late());
    }
	float RandomAngledroit()
	{
		switch(Random.Range(0,4))
		{
		case 3:
			return 0f;
			break;
		case 2:
			return 90f;
			break;
		case 1:
			return 180f;
			break;
		case 0:
			return 270f;
			break;
		default:
			return 0f;
			break;
		}

	}
	IEnumerator Late()
	{
		yield return new WaitForSeconds(2f);
		for(int i = 0;i < Sol.Length;i++)
		{
			Sol[i].transform.eulerAngles = new Vector3(0,RandomAngledroit(),0);
		}
		for(int x=0;x < objectDoubleRandomSuprimer.Length;x++)
		{
			int radomNum = Random.Range(0,objectDoubleRandomSuprimer[x].Object.Length);
			for(int i=0;i < objectDoubleRandomSuprimer[x].Object.Length;i++)
			{
				if(radomNum == i)
				{
					GameObject asignObject = objectDoubleRandomSuprimer[x].Object[i];
					for(int a=0;a < objectDoubleRandomSuprimer[x].Object.Length;a++)
					{
						if(asignObject != objectDoubleRandomSuprimer[x].Object[a])
						{
							Destroy(objectDoubleRandomSuprimer[x].Object[a]);	
						}
					}
					break;
				}
			}
		}
		if(SpawnState == 0)
		{
			Instantiate(content[Random.Range(0,content.Length)],transform.position,Quaternion.identity);
			Instantiate(spawnMob,transform.position,Quaternion.identity);
		}
		else if(SpawnState == 1) //Entrer
		{
			Instantiate(spawnEnterRoom,transform.position,Quaternion.identity);
		}
		else if(SpawnState == 2) //Sortie 
		{
			Instantiate(spawnExitRoom,transform.position,Quaternion.identity);
		}
		else if(SpawnState == 3) //Boss
		{
			Instantiate(spawnExitBoss,transform.position,Quaternion.identity);
		}
	}
}
