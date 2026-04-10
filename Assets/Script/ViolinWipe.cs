using UnityEngine;
using System.Collections; // 需要加入這個來使用 Coroutine

public class ViolinWipe : MonoBehaviour
{
    [Header("音效設定")]
    public AudioSource violinSource;
    public AudioClip[] progressiveClips;
    public AudioClip wipeSFX; // 這是擦拭的聲音空格
    public AudioClip sisterThanksVO;

    [Header("美好音樂設定")]
    public AudioClip beautifulViolinBGM; // 新增：美好的小提琴聲音空格
    public float fadeDuration = 2f;      // 美好聲音的淡入時間

    [Header("互動設定")]
    public int currentWipeCount = 0;
    public float wipeCooldown = 0.5f;
    private float lastWipeTime;
    public DoorManager targetDoor;

    private void Start()
    {
        if (violinSource != null && progressiveClips.Length > 0)
            violinSource.clip = progressiveClips[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cloth") && Time.time > lastWipeTime + wipeCooldown)
        {
            HapticManager.Instance.VibrateLight(OVRInput.Controller.Active);

            // 播放擦拭的聲音
            if (violinSource && wipeSFX) violinSource.PlayOneShot(wipeSFX);

            currentWipeCount++;
            lastWipeTime = Time.time;

            UpdateViolinState();
            Debug.Log("擦拭次數: " + currentWipeCount);
        }
    }

    void UpdateViolinState()
    {
        // 過程中的變化
        if (currentWipeCount < 3 && currentWipeCount < progressiveClips.Length)
        {
            violinSource.clip = progressiveClips[currentWipeCount];
            violinSource.Play();
        }

        // 達到成功次數
        if (currentWipeCount >= 3)
        {
            TriggerSuccess();
        }
    }

    void TriggerSuccess()
    {
        HapticManager.Instance.VibrateHeavy(OVRInput.Controller.Active);
        SanitySystem.Instance.ChangeSanity(-15);

        if (violinSource && sisterThanksVO) violinSource.PlayOneShot(sisterThanksVO);

        // 刺耳聲音消失，並淡入美好的聲音
        if (violinSource != null && beautifulViolinBGM != null)
        {
            violinSource.Stop();
            StartCoroutine(FadeInBeautifulMusic());
        }

        if (targetDoor != null)
        {
            targetDoor.OpenDoor(true);
            Debug.Log("溝通成功");
        }
        else
        {
            Debug.LogError("沒門");
        }
    }

    // 淡入美好音樂的協程
    IEnumerator FadeInBeautifulMusic()
    {
        violinSource.clip = beautifulViolinBGM;
        violinSource.volume = 0f; // 起始音量為0
        violinSource.Play();

        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            violinSource.volume = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
        violinSource.volume = 1f; // 確保最後音量是滿的
    }
}