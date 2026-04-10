using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI swordText;
    public TextMeshProUGUI CountDownText;
    public TextMeshProUGUI WaveText;
    public UnityEvent OnWaveStart;
    private bool waveStarted = false;

    void Update()
    {
        foodText.text = "Food: " + Mathf.FloorToInt(ResourceManager.Instance.FoodAmount);
        goldText.text = "Gold: " + Mathf.FloorToInt(ResourceManager.Instance.GoldAmount);
        woodText.text = "Wood: " + Mathf.FloorToInt(ResourceManager.Instance.WoodAmount);
        swordText.text = "Sword: " + Mathf.FloorToInt(ResourceManager.Instance.SwordAmount);
        if(ResourceManager.Instance.SwordAmount > 0 && !waveStarted)
        {
            CountDownText.gameObject.SetActive(false);
            OnWaveStart?.Invoke();
            WaveText.text = "Wave Started!";
            waveStarted = true;
            return;
        }
        if(waveStarted) return;
        float timeLeft=GameManager.Instance.WaveStartDelay - Time.timeSinceLevelLoad;
        if (timeLeft > 0)
        {
            CountDownText.gameObject.SetActive(true);
        }
        else
        {
            OnWaveStart?.Invoke();
            WaveText.text = "Wave Started!";
            waveStarted = true;
            CountDownText.gameObject.SetActive(false);
        }
        CountDownText.text = Mathf.CeilToInt(timeLeft) + "s";
    }
}
