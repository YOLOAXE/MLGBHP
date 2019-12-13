using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICursorDetect : MonoBehaviour
{

    [SerializeField]
    private int ID = 0;
    private GameObject Player = null;
    [SerializeField]
    private bool _ID = false, _IDM = false, _IDS1 = false, _IDS2 = false;

    private void Start()
    {
        Player = GameObject.FindWithTag("player");
    }
    public void PointerEnter()
    {
        if (_ID)
        {
            Player.SendMessage("EnterUI_ID", ID);
        }
        else if (_IDM)
        {
            Player.SendMessage("EnterUI_IDM", ID);
        }
        else if (_IDS1)
        {
            Player.SendMessage("EnterUI_IDS1", ID);
        }
        else if (_IDS2)
        {
            Player.SendMessage("EnterUI_IDS2", ID);
        }
    }
}
