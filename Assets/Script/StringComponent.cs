using UnityEngine;

public class StringComponent : MonoBehaviour
{
    public enum ComponentType { String, Blood }
    [Header("類型設定")]
    public ComponentType type;

    private ViolinInteraction mainSystem;
    private bool isProcessed = false;

    void Start()
    {
        mainSystem = GetComponentInParent<ViolinInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isProcessed) return;

        // 邏輯：琴弦只對剪刀有反應
        if (type == ComponentType.String && other.CompareTag("scissor"))
        {
            isProcessed = true;
            mainSystem.OnStringCut(this.gameObject);
            this.gameObject.SetActive(false); // 琴弦消失
        }
        // 邏輯：血跡只對抹布有反應
        else if (type == ComponentType.Blood && other.CompareTag("cloth"))
        {
            isProcessed = true;
            mainSystem.OnStringWiped(this.gameObject);
            this.gameObject.SetActive(false); // 血跡被擦乾淨（消失）
        }
    }
}