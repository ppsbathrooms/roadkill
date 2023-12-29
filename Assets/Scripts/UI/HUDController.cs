using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HotbarData
{
    public string itemName;
    public Sprite sprite;
}

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

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

    public void UpdateEggText()
    {
        eggText.text = PlayerData.EggCount.ToString();
    }

    public void UpdateSpeedText(float speed)
    {
        speedText.text = Mathf.RoundToInt(speed).ToString();
    }

   
}
