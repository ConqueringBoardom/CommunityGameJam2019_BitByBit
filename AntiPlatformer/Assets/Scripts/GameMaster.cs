using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static void KillPlayer (PlayerBase player)
    {
        Destroy (player.gameObject);
        Debug.Log("Player is Destroyed");
    }
}
