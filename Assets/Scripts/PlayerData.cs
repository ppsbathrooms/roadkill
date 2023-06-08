using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public void Awake() { instance = this; }

    [Header("Player Vars")] 
    public float Eggs;
}
