using UnityEngine;
using System.Collections; // 必須加入這個來使用 Coroutine (協程)

public class ViolinInteraction : MonoBehaviour
{
    [Header("基礎組件")]
    public AudioSource violinSource;
    public GameObject normalViolin;

    [Header("音效與門 (剪斷)")]
    public AudioClip cutSFX;
    public AudioClip stringSnapSFX; // 新增：弦斷掉的專屬音效空格

    [Header("音效與門 (擦拭)")]
    public AudioClip wipeSFX;             // 新增：擦拭的聲音空格
    public AudioClip sisterThanksVO;
    public AudioClip beautifulViolinBGM;  // 新增：美好的小提琴聲音空格
    public float fadeDuration = 2f;       // 美好聲音的淡入時間

    [Header("目標門")]
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

        // 播放剪斷音效、斷弦音效與震動
        if (violinSource != null)
        {
            if (cutSFX != null) violinSource.PlayOneShot(cutSFX);
            if (stringSnapSFX != null) violinSource.PlayOneShot(stringSnapSFX);
        }
        HapticManager.Instance.VibrateHeavy(OVRInput.Controller.Active);

        if (cutCount >= 4)
        {
            TriggerFailure();
        }
    }

    // 當血跡被擦掉時呼叫
    public void OnStringWiped(GameObject bloodObj)
    {
        if (currentMode == "Cut")
        {
            Debug.LogWarning("因為已經剪斷弦，無法進行擦拭！");
            return;
        }

        currentMode = "Wipe";
        wipeCount++;
        Debug.Log($"擦拭進度: {wipeCount}/1");

        HapticManager.Instance.VibrateLight(OVRInput.Controller.Active);

        // 播放擦拭的聲音
        if (violinSource != null && wipeSFX != null)
        {
            violinSource.PlayOneShot(wipeSFX);
        }

        if (wipeCount >= 1)
        {
            Debug.Log("擦拭完成，觸發成功開門");
            TriggerSuccess();
        }
    }

    void TriggerFailure()
    {
        // 剪斷所有弦，刺耳的音樂立即停止
        if (violinSource != null) violinSource.Stop();

        SanitySystem.Instance.ChangeSanity(15);
        if (targetDoor != null) targetDoor.OpenDoor(false);
    }

    void TriggerSuccess()
    {
        SanitySystem.Instance.ChangeSanity(-15);

        if (violinSource != null)
        {
            // 播放妹妹的感謝語音
            if (sisterThanksVO != null) violinSource.PlayOneShot(sisterThanksVO);

            // 刺耳聲音消失，並準備淡入美好的聲音
            violinSource.Stop();
            if (beautifulViolinBGM != null)
            {
                StartCoroutine(FadeInBeautifulMusic());
            }
        }

        if (targetDoor != null) targetDoor.OpenDoor(true);
    }

    // 淡入美好音樂的協程
    IEnumerator FadeInBeautifulMusic()
    {
        violinSource.clip = beautifulViolinBGM;
        violinSource.volume = 0f; // 起始音量設為0，準備淡入
        violinSource.Play();

        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            // 隨著時間將音量從 0 平滑拉升到 1
            violinSource.volume = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
        violinSource.volume = 1f; // 確保最後音量是滿的
    }
}