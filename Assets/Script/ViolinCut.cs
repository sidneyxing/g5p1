using UnityEngine;

public class ViolinCut : MonoBehaviour
{
    public GameObject normalViolin;
    public GameObject brokenViolin;  // 缺！斷絃版本
    public AudioSource violinSource;

    [Header("音效設定")]
    public AudioClip cutSFX;         // 剪刀喀嚓聲 (如果有)
    public AudioClip stringSnapSFX;  // 新增：弦斷掉的專屬音效空格

    [Header("目標門設定")]
    public DoorManager targetDoor;
    private bool isCut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCut) return;
        if (other.CompareTag("scissor"))
        {
            ExecuteCut();
        }
    }

    void ExecuteCut()
    {
        isCut = true;

        // 刺耳的音樂立即停止
        if (violinSource != null) violinSource.Stop();

        if (brokenViolin != null)
        {
            Instantiate(brokenViolin, transform.position, transform.rotation);

            // 播放斷弦音效與剪刀音效
            if (violinSource != null)
            {
                if (cutSFX != null) violinSource.PlayOneShot(cutSFX);
                if (stringSnapSFX != null) violinSource.PlayOneShot(stringSnapSFX);
            }
        }

        normalViolin.SetActive(false);

        HapticManager.Instance.VibrateHeavy(OVRInput.Controller.Active);
        TriggerFailure();
    }

    void TriggerFailure()
    {
        SanitySystem.Instance.ChangeSanity(20);
        if (targetDoor != null)
        {
            targetDoor.OpenDoor(false);
            Debug.Log("琴弦被剪斷，門重重打開");
        }
        else
        {
            Debug.Log("沒門");
        }
    }
}