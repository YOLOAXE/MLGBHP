using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    private float DistanceInteractionPNJ = 3.5f;
    [SerializeField]
    private float DistanceInteractionCoffre = 3.5f;
    private bool OnDialog = true;

    void Update()
    {
        if (Input.GetButtonDown("Interagire") && !Input.GetButton("Inventaire"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "PNJ" || hit.transform.tag == "Soldat")
                {
                    if (hit.distance <= DistanceInteractionPNJ)
                    {
                        if (hit.transform.GetComponent<DialogueTrigger>() != null)
                        {
                            hit.transform.SendMessage("ReceiveDialogState", OnDialog);
                        }
                    }
                }
                if (hit.transform.tag == "Coffre")
                {
                    if (hit.distance <= DistanceInteractionCoffre)
                    {
                        hit.transform.SendMessage("Open");
                    }
                }
            }
        }
    }
}
