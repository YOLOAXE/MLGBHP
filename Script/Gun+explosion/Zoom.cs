using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public bool ZoomSniper;
	public float BaseFOV = 60,ZoomFOV = 40;
	public float TimeZoom = 1; 
	public GameObject CacheSniper,DrawAlway;
	
    void Start()
    {
        gameObject.GetComponent<Camera>().fieldOfView = BaseFOV;
		CacheSniper = GameObject.Find("qiodgfjqdg_Cache");
		CacheSniper.SetActive(false);
    }
	public void ReceiveZoom(bool ZoomSniperR){
		ZoomSniper = ZoomSniperR;
		if(ZoomSniper){
			StartCoroutine(ZoomCouroutine());
		}else{
			gameObject.GetComponent<Camera>().fieldOfView = BaseFOV;
			CacheSniper.SetActive(false);
			DrawAlway.GetComponent<Camera>().farClipPlane = 20;
		}
	}
		IEnumerator ZoomCouroutine(){
			yield return new WaitForSeconds(TimeZoom);
			if(ZoomSniper){
				gameObject.GetComponent<Camera>().fieldOfView = ZoomFOV;
				CacheSniper.SetActive(true);
				DrawAlway.GetComponent<Camera>().farClipPlane = 0.1f;
			}else{
				gameObject.GetComponent<Camera>().fieldOfView = BaseFOV;
				CacheSniper.SetActive(false);
				DrawAlway.GetComponent<Camera>().farClipPlane = 20;
			}
		}
}
