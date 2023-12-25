using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public void Awake() { instance = this; }

    [Header("Refs")]
    public Transform scene;

    [Space]
    [Header("Effects")]
    public GameObject deathEffect;
    public GameObject featherEffect;
    public GameObject eggEffect;
    public GameObject muzzleFlash;
    public GameObject smoke;
    public GameObject grassHit;
    public GameObject bloodHit;
    public GameObject tireMark;

    [Space]
    [Header("Containers")]
    public Transform effectsContainer;
    public Transform chickenContainer;
    public Transform coopContainer;
    public Transform tireMarkContainer;
}
