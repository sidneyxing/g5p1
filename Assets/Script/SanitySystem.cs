using UnityEngine;
using UnityEngine.UI; // 必須引用 UI 命名空間
using TMPro;

public class SanitySystem : MonoBehaviour
{
    // 讓其他物件（如怪物、機關）能輕鬆找到這個腳本
    public static SanitySystem Instance;

    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float currentSanity;
    currentSanity = 50;

    [Header("UI References")]
    public Image sanityBarFill;  // 拖入黃條 Image
    public TextMeshProUGUI sanityText; // 拖入文字（選配）

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentSanity = maxSanity;
        UpdateUI();
    }

    // 當外部事件結束或觸發時，呼叫這個方法
    // 例如：SanitySystem.Instance.ChangeSanity(10); 扣10點
    public void ChangeSanity(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity); // 確保不超出 0~100
        UpdateUI();
    }

    void UpdateUI()
    {
        if (sanityBarFill != null)
        {
            // 計算比例 (0.0 到 1.0)
            sanityBarFill.fillAmount = currentSanity / maxSanity;
            
            // 如果理智太低變紅色 (選用)
            sanityBarFill.color = (currentSanity < 30) ? Color.red : Color.yellow;
        }

        if (sanityText != null)
        {
            sanityText.text = $"SAN: {Mathf.CeilToInt(currentSanity)}";
        }
    }
}