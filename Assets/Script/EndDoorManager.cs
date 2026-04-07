using UnityEngine;
using System.Collections;
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
 

    [Header("音效設定 (SFX)")]
    public AudioSource sfxSource;
    public AudioClip gentleOpenSound;
    public AudioClip roughOpenSound;

    [Header("結局音樂設定 (BGM)")]
    public AudioSource bgmSource; 
    public AudioClip endingWarmBGM;
    public AudioClip endingColdBGM;
    public float sanityThreshold = 50f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        targetRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);

        if (sfxSource != null) sfxSource.playOnAwake = false;
        
    }

    void Update()//測試用?
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


    
    void PlayEndingBGM()
    {
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
        bgmSource.Play();
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