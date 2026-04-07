using UnityEngine;

public class LighterInteraction : MonoBehaviour 
{
    [Header("模型與特效")]
    public GameObject burnedVersion; 
    public ParticleSystem fireEffect; 

    [Header("目標門設定")]
    public DoorManager targetDoor;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (hasTriggered) return;

        if (other.CompareTag("Fragment")) 
        {
            hasTriggered = true;
            if (fireEffect != null) 
            {
                fireEffect.transform.position = other.transform.position;
                fireEffect.Play(); 
            }

            if (burnedVersion != null)
            {
                GameObject newBurned = Instantiate(burnedVersion, other.transform.position, other.transform.rotation);
                newBurned.SetActive(true); 
            }

            other.gameObject.SetActive(false); 
            
            if (targetDoor != null)
            {
                targetDoor.OpenDoor(false);
            }

            Debug.Log("結局 B 觸發：燃燒並重重撞門");
        }
    }
}