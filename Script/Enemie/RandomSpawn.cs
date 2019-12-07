using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
	public Transform MobObjectSpawn;
	public int NBSpawn;
	public float DistanceAutoriser;
	private float HitDistanceUp,HitDistanceRight,HitDistanceLeft,HitDistanceForward,HitDistanceBackward;
	public GameObject Point1,Point2,PointTp;
	public Transform[] ObjectSpawn;

    void Start(){
		ObjectSpawn = new Transform[NBSpawn];
    }

    void Update(){
		for(int i = 0;ObjectSpawn.Length > i;i++){
			if(ObjectSpawn[i] == null){
				PointTp.transform.position = new Vector3(Random.Range(Point1.transform.position.x,Point2.transform.position.x),Random.Range(Point1.transform.position.y,Point2.transform.position.y),Random.Range(Point1.transform.position.z,Point2.transform.position.z));
				RaycastHit hitUp;
				RaycastHit hitLeft;
				RaycastHit hitRight;
				RaycastHit hitForward;
				RaycastHit hitBackward;
				if (Physics.Raycast(PointTp.transform.position, PointTp.transform.TransformDirection(Vector3.up), out hitUp)){HitDistanceUp = hitUp.distance;}
				if (Physics.Raycast(PointTp.transform.position, PointTp.transform.TransformDirection(Vector3.back), out hitBackward)){HitDistanceBackward = hitBackward.distance;}
				if (Physics.Raycast(PointTp.transform.position, PointTp.transform.TransformDirection(Vector3.left), out hitLeft)){HitDistanceLeft = hitLeft.distance;}
				if (Physics.Raycast(PointTp.transform.position, PointTp.transform.TransformDirection(Vector3.right), out hitRight)){HitDistanceRight = hitRight.distance;}
				if (Physics.Raycast(PointTp.transform.position, PointTp.transform.TransformDirection(Vector3.forward), out hitForward)){HitDistanceForward = hitForward.distance;}
				if(HitDistanceUp >= DistanceAutoriser &&  HitDistanceLeft >= DistanceAutoriser && HitDistanceRight >= DistanceAutoriser && HitDistanceForward >= DistanceAutoriser && HitDistanceBackward >= DistanceAutoriser){
					ObjectSpawn[i] = Instantiate(MobObjectSpawn,PointTp.transform.position,Quaternion.identity);
				}	
			}
			
		}
    }
}

