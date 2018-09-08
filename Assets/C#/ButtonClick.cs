using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ButtonClick : NetworkBehaviour
{

	public void onClick()
    {
        if (!isLocalPlayer) { return; }
        GameObject Player = GameObject.Find("Player");
        Debug.Log(Player.name);
    }
}
