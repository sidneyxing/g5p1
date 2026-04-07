using UnityEngine;

public class EnterRoom : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hint;
    public bool playOnlyOnce = true;
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            if (audioSource && hint)
            {
                audioSource.PlayOneShot(hint);
                if (playOnlyOnce) hasPlayed = true;
            }
        }
    }
}