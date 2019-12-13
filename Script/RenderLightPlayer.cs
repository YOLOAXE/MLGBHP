using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLightPlayer : MonoBehaviour
{
    private GameObject Player = null;
    [SerializeField]
    private float SpeedRate = 0.8f;
    public List<GameObject> LightControle = new List<GameObject>();

    void Start()
    {
        Player = GameObject.FindWithTag("player");
        StartCoroutine(LightControl());
       /* LightControl.Add(new GameObject(GameObject.FindWithTags));*/
    }

    IEnumerator LightControl()
    {
        while (true)
        {

            yield return new WaitForSeconds(SpeedRate);
        }
    }
}
