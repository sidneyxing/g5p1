using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; 

public class DoorManager : MonoBehaviour
{
    [Header("旋轉設定")]
    public float openAngle = 90f;     
    public float gentleSpeed = 1f;  
    public float roughSpeed = 5f; 

    [Header("音效設定")]
    public AudioSource audioSource;
    public AudioClip gentleOpenSound;
    public AudioClip roughOpenSound; 

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        
    
        targetRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
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
        AudioClip clip = isGentle ? gentleOpenSound : roughOpenSound;

        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }

        StartCoroutine(AnimateDoor(speed));
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