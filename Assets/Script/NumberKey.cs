using UnityEngine;

public class NumberKey : MonoBehaviour
{
    public int value;
    private SafeManager manager;

    void Start()
    {
        manager = GetComponentInParent<SafeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
       
        // if (other.CompareTag("Player") || other.CompareTag("GameController"))
        // {
        manager.SendInput(value);
        // temporate delete
        // }
    }
}