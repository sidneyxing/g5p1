using UnityEngine;
using TMPro;
using System.Collections;

public class SafeManager : MonoBehaviour
{
    [Header("密碼解謎")]
    public string correctPassword = "1234";
    private string currentInput = "";
    public TextMeshProUGUI displayText;
    public Transform doorPivot;
    public float openAngle = -90f;
    private bool isProcessed = false;

    [Header("hammer破壞")]
    public int maxHealth = 3; 
    private int currentHealth;
    public GameObject normalSafeModel;
    public GameObject brokenSafePrefab;
    public GameObject destroyedLetter;

    [Header("音效與震動")]
    public AudioSource audioSource;
    public AudioClip beepSound; //按按鈕的聲音   
    public AudioClip errorSound;  // 有錯誤的聲音
    public AudioClip successSound; // 成功打開的聲音
    public AudioClip metalImpactSFX; // 敲擊聲
    public AudioClip boxCrushSFX; // 爆開的聲音

    private Quaternion closedRotation;
    private Quaternion targetRotation;

    [Header("要開的門")]
    public EndDoorManager targetDoor;

    void Start()
    {
        currentHealth = maxHealth;
        closedRotation = doorPivot.localRotation;
        targetRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
        //UpdateDisplay();
    }

    // 輸入密碼
    public void SendInput(int number)
    {
        if (isProcessed) return;

        PlaySound(beepSound);
        HapticManager.Instance.VibrateLight(OVRInput.Controller.Active);

        string nextDigit = number.ToString();
        
        if (correctPassword.StartsWith(currentInput + nextDigit))
        {
            currentInput += nextDigit;
           // UpdateDisplay();

            if (currentInput == correctPassword)
            {
                SuccessPath();
            }
        }
        else
        {
            StartCoroutine(HandleError());
        }
    }

    // hammer打
    private void OnTriggerEnter(Collider other)
    {
        if (isProcessed) return;

        if (other.CompareTag("hammer"))
        {
            currentHealth--;

            
            HapticManager.Instance.VibrateHeavy(OVRInput.Controller.Active);
            if(audioSource && metalImpactSFX) audioSource.PlayOneShot(metalImpactSFX);

            Debug.Log("保險箱被砸了！" + currentHealth);

            if (currentHealth <= 0)
            {
                FailurePath();
                
            }
        }
    }


    void SuccessPath()
    {
        isProcessed = true;
        PlaySound(successSound);
        //displayText.text = "OPEN";
        
        StartCoroutine(AnimateDoor());
        SanitySystem.Instance.ChangeSanity(-15);
        
        NotifyDoorManager(true);
    }

    void FailurePath()
    {
        if(audioSource && boxCrushSFX) audioSource.PlayOneShot(boxCrushSFX);
        isProcessed = true;
        //displayText.text = "SAD";
        SanitySystem.Instance.ChangeSanity(20);
        if(audioSource && boxCrushSFX) audioSource.PlayOneShot(boxCrushSFX);
        if (normalSafeModel != null) normalSafeModel.SetActive(false);
        if (brokenSafePrefab != null)
        {
            Quaternion correction = transform.rotation * Quaternion.Euler(-90f, 0, 0);
            Instantiate(brokenSafePrefab, transform.position, transform.rotation);
        }
        
        
        NotifyDoorManager(false);
    }

    // --- 工具函數 ---

    void NotifyDoorManager(bool isGentle)
    {
            if (targetDoor != null)
        {
            targetDoor.OpenDoor(isGentle);
        }
        else
        {
            Debug.LogError("把門拉進來");
        }
       
    }

    IEnumerator HandleError()
    {
        PlaySound(errorSound);
        //displayText.text = "ERROR";
        currentInput = "";
        yield return new WaitForSeconds(1.0f);
        //UpdateDisplay();
    }

    //void UpdateDisplay()
    //{
    //    if (displayText != null) displayText.text = currentInput == "" ? "----" : currentInput;
    //}

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null) audioSource.PlayOneShot(clip);
    }

    IEnumerator AnimateDoor()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            doorPivot.localRotation = Quaternion.Slerp(closedRotation, targetRotation, t);
            yield return null;
        }
    }
}