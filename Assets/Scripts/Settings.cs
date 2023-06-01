using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public void Awake() { instance = this; }

    [Header("Refs")] 
    public GameObject deathEffect;

    [Header("Containers")] 
    public Transform effectsContainer;
}