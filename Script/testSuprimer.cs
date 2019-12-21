using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSuprimer : MonoBehaviour
{
    [SerializeField]
    private Vector3 OriginePos = new Vector3(0, 0, 0);
    private Vector3 calculePos = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 Pos = new Vector3(0, 0, 0);
    [SerializeField]
    private GameObject Arme = null;

    void Start()
    {
        OriginePos = Arme.transform.localPosition;
        calculePos = OriginePos;
        Pos = calculePos;
    }

    void Update()
    {
        calculePos = Vector3.Lerp(calculePos, Pos,Time.deltaTime * 8);
    }

    void LateUpdate()
    {
        Arme.transform.localPosition = calculePos;
    }
}
