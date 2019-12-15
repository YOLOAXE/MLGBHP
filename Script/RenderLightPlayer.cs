using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLightPlayer : MonoBehaviour
{
    private GameObject Player = null;
    [SerializeField]
    private float SpeedRate = 1f,DistanceOP = 20f;
    [SerializeField]
    private GameObject[] LightControle = null;
    private int i = 0;

    void Start()
    {
        Player = GameObject.FindWithTag("player");
        StartCoroutine(LightControl());
    }

    IEnumerator LightControl()
    {
        while (true)
        {
            LightControle = GameObject.FindGameObjectsWithTag("LightControl");
            yield return new WaitForSeconds(SpeedRate);
            for(i = 0;i < LightControle.Length;i++)
            {
                LightControle[i].SetActive(DistanceOP < Vector3.Distance(Player.transform.position, LightControle[i].transform.position));
            }
        }
    }
}
