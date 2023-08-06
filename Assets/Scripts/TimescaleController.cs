using UnityEngine;

public class TimescaleController : MonoBehaviour {
    public static TimescaleController Instance;
    private void Awake() { Instance = this; }

    private float _speedMultiplier;
    private bool _isSlow;
    private float _slowEnd = -1f;
    
    /// <summary>Change the speed of time for a given time</summary>
    public void TriggerSlow(float slowTime, float speedMultiplier) {
        _speedMultiplier = speedMultiplier;
        _slowEnd = Time.time + slowTime*speedMultiplier;
    }
    
    private void Update() {
        bool slowThisFrame = _slowEnd > Time.time;

        if (slowThisFrame != _isSlow) {
            Debug.Log($"Triggered {slowThisFrame}");
            Time.timeScale = slowThisFrame ? _speedMultiplier : 1f;
            _isSlow = slowThisFrame;
        }
    }
}