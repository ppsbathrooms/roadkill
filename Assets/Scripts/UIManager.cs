using System;
using Collidable;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake() { Instance = this; }

    [Header("Refs")]
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text eggText;

    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private GameObject _endGameButton;
    [SerializeField] private GameObject _gameOverScreen;

    public GameObject StartGameButton => _startGameButton;
    public GameObject EndGameButton => _endGameButton;
    public GameObject GameOverScreen => _gameOverScreen;

    private void Start()
    {
        CarController.OnHitCollidable.AddListener(UpdateEggText);
    }

    public void UpdateEggText(AbstractCollidableObject collidableObject)
    {
        eggText.text = PlayerData.eggCount.ToString();
    }
    
    public void UpdateSpeedText(float speed)
    {
        speedText.text = Mathf.RoundToInt(speed).ToString();
    }
}
