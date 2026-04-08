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
    public int stringsHandled = 0; // 已處理的弦數量
    private bool interactionTypeLocked = false; // 鎖定互動類型
    private string currentMode = ""; // "Cut" or "Wipe"

    // 當任何一根弦被剪斷時呼叫
    public void OnStringCut(GameObject stringObj)
    {
        if (currentMode == "Wipe") return; // 如果已經開始擦拭，則禁止剪斷
        
        currentMode = "Cut";
        stringsHandled++;
        
        // 播放剪斷音效與震動
        if(violinSource && cutSFX) violinSource.PlayOneShot(cutSFX);
        HapticManager.Instance.VibrateHeavy(OVRInput.Controller.Active);

        if (stringsHandled >= 4)
        {
            TriggerFailure();
        }
    }

    // 當血跡被擦掉時呼叫
    public void OnStringWiped(GameObject bloodObj)
    {
        if (currentMode == "Cut") return; // 互斥邏輯：若已剪斷則無法擦拭

        currentMode = "Wipe";
        stringsHandled++; // 借用這個變數來計數擦拭了多少血跡

        // 播放擦拭音效與輕微震動
        HapticManager.Instance.VibrateLight(OVRInput.Controller.Active);

        if (stringsHandled >= 4) // 假設有四處血跡
        {
            TriggerSuccess();
        }
    }

    // OnStringCut 保持原樣，但確保它會檢查 currentMode

    void TriggerFailure()
    {
        SanitySystem.Instance.ChangeSanity(20);
        if (targetDoor != null) targetDoor.OpenDoor(false);
    }

    void TriggerSuccess()
    {
        SanitySystem.Instance.ChangeSanity(-15);
        if (targetDoor != null) targetDoor.OpenDoor(true);
    }
}