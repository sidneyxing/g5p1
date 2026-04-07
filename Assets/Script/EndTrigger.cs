
using UnityEngine;
using System.Collections;

public class EndTrigger : MonoBehaviour
{
    [Header("UI 連結")]
    public GameObject endingUI;
    public EndTalk endTalkScript; 
    [Header("延遲設定")]
    public float delaySeconds = 6.0f; 

    private bool hasTriggered = false; 

    void Start()
    {
        if (endingUI != null) endingUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            Debug.Log("玩家已進入結局區域，啟動計時...");
            StartCoroutine(StartEndingSequenceWithDelay());
        }
    }

    IEnumerator StartEndingSequenceWithDelay()
    {
        yield return new WaitForSeconds(delaySeconds);

        if (endingUI != null)
        {
            endingUI.SetActive(true);
            
            if (endTalkScript != null)
            {
                endTalkScript.StartEndingSequence();
            }
        }
    }
}