using UnityEngine;

public class ViolinCut : MonoBehaviour
{
    public GameObject normalViolin;
    public GameObject brokenViolin;  // 缺！斷絃版本
    public AudioSource violinSource;
    public AudioClip cutSFX;

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
        if (violinSource != null) violinSource.Stop();

        if (brokenViolin != null)
        {
            Instantiate(brokenViolin, transform.position, transform.rotation);
            if (violinSource != null && cutSFX != null) violinSource.PlayOneShot(cutSFX);

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
        }else{
            Debug.Log("沒門");
        }
        
        
    }
}