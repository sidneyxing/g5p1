using UnityEngine;

public class ViolinWipe : MonoBehaviour
{
    [Header("音效設定")]
    public AudioSource violinSource;
    public AudioClip[] progressiveClips; // 音效！！分別放入0(刺耳) 1(稍微緩和) 2(柔和) 3(旋律)
    public AudioClip wipeSFX; 
    public AudioClip sisterThanksVO;
    
    [Header("互動設定")]
    public int currentWipeCount = 0;
    public float wipeCooldown = 0.5f; 
    private float lastWipeTime;
    public DoorManager targetDoor;

    

    private void Start()
    {
        if(violinSource != null && progressiveClips.Length > 0)
            violinSource.clip = progressiveClips[0]; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cloth") && Time.time > lastWipeTime + wipeCooldown)
        {
            HapticManager.Instance.VibrateLight(OVRInput.Controller.Active);
            if(violinSource && wipeSFX) violinSource.PlayOneShot(wipeSFX);
            currentWipeCount++;
            lastWipeTime = Time.time;

            UpdateViolinState();
            Debug.Log("擦拭次數: " + currentWipeCount);
        }
    }

    void UpdateViolinState()
    {
        if (currentWipeCount <= 3)
        {
            violinSource.clip = progressiveClips[currentWipeCount];
            violinSource.Play();
        }

        if (currentWipeCount >= 3)
        {
            TriggerSuccess();
        }
    }

    void TriggerSuccess()
    {
        HapticManager.Instance.VibrateHeavy(OVRInput.Controller.Active);
        SanitySystem.Instance.ChangeSanity(-15);
        if(violinSource && sisterThanksVO) violinSource.PlayOneShot(sisterThanksVO);
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
}