using Collidable;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text eggText;

    private void Start()
    {
        instance = this;
        CarController.onHitCollidable.AddListener(UpdateEggText);
    }

    public void UpdateEggText(AbstractCollidableObject collidableObject)
    {
        eggText.text = PlayerData.eggCount.ToString();
    }
    
    public void SetSpeedText(float speed)
    {
        speedText.text = Mathf.RoundToInt(speed).ToString();
    }
}
