using UnityEngine;
using Oculus.Interaction; 
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public List<SnapInteractor> sockets; 
    public GameObject successEffect;

    public DoorManager targetDoor; 

    private bool isFinished = false;

    void Update()
    {
        if (isFinished) return;
        CheckPuzzleStatus();
    }

    void CheckPuzzleStatus()
    {
        int filledCount = 0;
        foreach (var socket in sockets)
        {
            if (socket.HasSelectedInteractable)
            {
                filledCount++;
            }
        }

        if (filledCount == sockets.Count && sockets.Count > 0)
        {
            OnPuzzleComplete();
        }
    }

    void OnPuzzleComplete()
    {
        isFinished = true;
        Debug.Log("拼圖完成！開啟指定的門。");
        
        if (successEffect != null) successEffect.SetActive(true);
        
        if (targetDoor != null)
        {
            targetDoor.OpenDoor(true);
        }
        else
        {
            Debug.LogWarning("未指定 targetDoor！請在 Inspector 中設定。");
        }

        this.enabled = false; 
    }
}