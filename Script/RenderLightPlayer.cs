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

    public void Generate()
    {
        Player = GameObject.FindWithTag("player");
        LightControle = GameObject.FindGameObjectsWithTag("LightControl");
        StartCoroutine(LightControl());

    }

    IEnumerator LightControl()
    {
        while (true)
        {
            for(i = 0;i < LightControle.Length;i++)
            {
                if (LightControle[i] != null)
                {
                    LightControle[i].SetActive(DistanceOP > Vector3.Distance(Player.transform.position, LightControle[i].transform.position));
                }
            }
            yield return new WaitForSeconds(SpeedRate);
        }
    }
}
