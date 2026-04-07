using UnityEngine;

public class FootStep : MonoBehaviour
{
    [Header("音效設定")]
    public AudioClip[] footstepSFX; 
    public AudioSource audioSource;
    
    [Header("移動設定")]
    public float stepDistance = 0.8f; 
    private Vector3 lastPosition;
    private float distanceMoved;

    void Start()
    {
        // 如果沒拉 AudioSource，自動抓取這台機器上的
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // 只有水平移動才算步數 (忽略 Y 軸，防止跳動也算走路)
        Vector3 currentPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 prevPos = new Vector3(lastPosition.x, 0, lastPosition.z);
        
        float dist = Vector3.Distance(currentPos, prevPos);
        distanceMoved += dist;
        lastPosition = transform.position;

        if (distanceMoved >= stepDistance)
        {
            PlayFootstep();
            distanceMoved = 0;
        }
    }

    void PlayFootstep()
    {
        if (footstepSFX != null && footstepSFX.Length > 0 && audioSource != null)
        {
            AudioClip clip = footstepSFX[Random.Range(0, footstepSFX.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}