using UnityEngine;
using System.Collections;
using System.Collections.Generic; // 新增這行，為了使用 List 清單功能
using UnityEngine.InputSystem;

public class EndDoorManager : MonoBehaviour
{
    [Header("旋轉設定")]
    public float openAngle = 90f;
    public float gentleSpeed = 1f;
    public float roughSpeed = 5f;

    [Header("物件設定")]
    public GameObject happy;
    public GameObject sad;

    [Header("環境底噪設定 (Ambient)")]
    public AudioSource ambientSource; // 播放底噪的 AudioSource
    [Tooltip("請在這裡放入4首底噪/音效")]
    public AudioClip[] ambientClips = new AudioClip[4]; // 幫你準備好4個音效欄位

    private List<AudioClip> unplayedAmbientClips = new List<AudioClip>(); // 追蹤這輪還沒播過的音效
    private Coroutine ambientRoutine; // 儲存播放背景音的協程，方便結局時打斷

    [Header("音效設定 (SFX)")]
    public AudioSource sfxSource;
    public AudioClip gentleOpenSound;
    public AudioClip roughOpenSound;

    [Header("結局音樂設定 (BGM)")]
    public AudioSource bgmSource;
    public AudioClip endingWarmBGM;
    public AudioClip endingColdBGM;
    public float sanityThreshold = 50f;
    public float bgmFadeDuration = 2f; // BGM 淡入的時間長度

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        targetRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);

        if (sfxSource != null) sfxSource.playOnAwake = false;

        // 確保環境底噪不會單首無限循環，並開始播放隨機歌單
        if (ambientSource != null)
        {
            ambientSource.loop = false; // 強制關閉單首循環
            ambientRoutine = StartCoroutine(PlayAmbientPlaylist());
        }
    }

    void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame) OpenDoor(true);
        if (Keyboard.current.lKey.wasPressedThisFrame) OpenDoor(false);
    }

    public void OpenDoor(bool isGentle)
    {
        if (isOpen) return;
        isOpen = true;

        float speed = isGentle ? gentleSpeed : roughSpeed;
        AudioClip sfxClip = isGentle ? gentleOpenSound : roughOpenSound;

        if (sfxSource != null && sfxClip != null)
        {
            sfxSource.PlayOneShot(sfxClip);
        }

        PlayEndingBGM();
        StartCoroutine(AnimateDoor(speed));
    }

    // --- 新增：處理底噪隨機不重複播放的協程 ---
    IEnumerator PlayAmbientPlaylist()
    {
        while (!isOpen) // 只要結局門還沒開，就繼續自動播
        {
            // 如果未播放清單空了（4首都播完了），就重新把所有音效加進來洗牌
            if (unplayedAmbientClips.Count == 0)
            {
                foreach (AudioClip clip in ambientClips)
                {
                    if (clip != null) unplayedAmbientClips.Add(clip);
                }
            }

            // 如果陣列裡什麼都沒放，就跳出避免當機
            if (unplayedAmbientClips.Count == 0) yield break;

            // 隨機抽一首
            int randomIndex = Random.Range(0, unplayedAmbientClips.Count);
            AudioClip clipToPlay = unplayedAmbientClips[randomIndex];

            // 從「未播放清單」中移除這首，確保這輪不會再抽到重複的
            unplayedAmbientClips.RemoveAt(randomIndex);

            // 播放這首音效
            ambientSource.clip = clipToPlay;
            ambientSource.Play();

            // 讓程式等待，直到這首音效的長度播完，再進入下一次迴圈
            yield return new WaitForSeconds(clipToPlay.length);
        }
    }

    void PlayEndingBGM()
    {
        // 結局觸發，立刻停止底噪的「自動播放協程」
        if (ambientRoutine != null)
        {
            StopCoroutine(ambientRoutine);
        }

        // 停止當前正在出聲的底噪
        if (ambientSource != null && ambientSource.isPlaying)
        {
            ambientSource.Stop();
        }

        if (bgmSource == null) return;

        float currentSanity = 50f;
        if (SanitySystem.Instance != null)
        {
            currentSanity = SanitySystem.Instance.currentSanity;
        }

        if (currentSanity > sanityThreshold)
        {
            bgmSource.clip = endingWarmBGM;
            if (happy != null) happy.SetActive(true);
            if (sad != null) sad.SetActive(false);
            Debug.Log("好結局！");
        }
        else
        {
            bgmSource.clip = endingColdBGM;
            if (happy != null) happy.SetActive(false);
            if (sad != null) sad.SetActive(true);
            Debug.Log("壞結局...");
        }

        bgmSource.loop = true;
        bgmSource.volume = 0f; // 將音量設為0，準備淡入
        bgmSource.Play();

        // 啟動淡入效果
        StartCoroutine(FadeInBGM(bgmSource, bgmFadeDuration));
    }

    IEnumerator FadeInBGM(AudioSource audioSource, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }
        audioSource.volume = 1f;
    }

    IEnumerator AnimateDoor(float speed)
    {
        float time = 0;
        while (time < 1f)
        {
            transform.localRotation = Quaternion.Slerp(closedRotation, targetRotation, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
        transform.localRotation = targetRotation;
    }
}