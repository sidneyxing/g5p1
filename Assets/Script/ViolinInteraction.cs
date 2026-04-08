using UnityEngine;

public class ViolinInteraction : MonoBehaviour
{
    [Header("基礎組件")]
    public AudioSource violinSource;
    public GameObject normalViolin;

    [Header("音效與門")]
    public AudioClip cutSFX;
    public AudioClip sisterThanksVO;
    public DoorManager targetDoor;

    [Header("狀態追蹤")]
    public int cutCount = 0;
    public int wipeCount = 0;
    private bool interactionTypeLocked = false; // 鎖定互動類型
    private string currentMode = ""; // "Cut" or "Wipe"

    // 當任何一根弦被剪斷時呼叫
    public void OnStringCut(GameObject stringObj)
    {
        if (currentMode == "Wipe") return; // 如果已經開始擦拭，則禁止剪斷
        
        currentMode = "Cut";
        cutCount++;
        
        // 播放剪斷音效與震動
        if(violinSource && cutSFX) violinSource.PlayOneShot(cutSFX);
        HapticManager.Instance.VibrateHeavy(OVRInput.Controller.Active);

        if (cutCount >= 4)
        {
            TriggerFailure();
        }
    }

    // 當血跡被擦掉時呼叫
    public void OnStringWiped(GameObject bloodObj)
    {
        if (currentMode == "Cut") {
            Debug.LogWarning("因為已經剪斷弦，無法進行擦拭！");
            return;
        }

        currentMode = "Wipe";
        wipeCount++; 
        Debug.Log($"擦拭進度: {wipeCount}/1");

        HapticManager.Instance.VibrateLight(OVRInput.Controller.Active);

        if (wipeCount >= 1) 
        {
            Debug.Log("擦拭完成，觸發成功開門");
            TriggerSuccess();
        }
    }

    // OnStringCut 保持原樣，但確保它會檢查 currentMode

    void TriggerFailure()
    {
        SanitySystem.Instance.ChangeSanity(15);
        if (targetDoor != null) targetDoor.OpenDoor(false);
    }

    void TriggerSuccess()
    {
        SanitySystem.Instance.ChangeSanity(-15);
        if (targetDoor != null) targetDoor.OpenDoor(true);
    }
}