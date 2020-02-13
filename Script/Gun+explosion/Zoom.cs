using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private bool ZoomSniper = false;
    [SerializeField] private float BaseFOV = 60, ZoomFOV = 40;
    [SerializeField] private float TimeZoom = 1;
    [SerializeField] private GameObject CacheSniper = null, DrawAlway = null;

    public void ChercheScope()
    {
        CacheSniper = GameObject.Find("qiodgfjqdg_Cache");
        gameObject.GetComponent<Camera>().fieldOfView = BaseFOV;
        CacheSniper.SetActive(false);
    }

    void Awake()
    {
        ChercheScope();
    }

    public void ReceiveZoom(bool ZoomSniperR)
    {
        ZoomSniper = ZoomSniperR;
        if (ZoomSniper)
        {
            StartCoroutine(ZoomCouroutine());
        }
        else
        {
            gameObject.GetComponent<Camera>().fieldOfView = BaseFOV;
            CacheSniper.SetActive(false);
            DrawAlway.GetComponent<Camera>().farClipPlane = 20;
        }
    }

    IEnumerator ZoomCouroutine()
    {
        yield return new WaitForSeconds(TimeZoom);
        if (ZoomSniper)
        {
            gameObject.GetComponent<Camera>().fieldOfView = ZoomFOV;
            CacheSniper.SetActive(true);
            DrawAlway.GetComponent<Camera>().farClipPlane = 0.1f;
        }
        else
        {
            gameObject.GetComponent<Camera>().fieldOfView = BaseFOV;
            CacheSniper.SetActive(false);
            DrawAlway.GetComponent<Camera>().farClipPlane = 20;
        }
    }
}
