using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("音效設定")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips; // 放入多個腳步聲，隨機播放聽起來更自然

    [Header("移動設定")]
    public float walkStepInterval = 0.5f; // 走路時每步的間隔（秒）
    public float runStepInterval = 0.3f;  // 跑步時的間隔
    public float velocityThreshold = 0.1f; // 啟動腳步聲的速度閾值

    private float stepTimer;
    private CharacterController characterController; // 假設你使用 CharacterController

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 取得當前水平移動速度
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        // 如果速度大於閾值且玩家在地面上
        if (currentSpeed > velocityThreshold && characterController.isGrounded)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0)
            {
                PlayFootstep();
                // 根據速度動態調整步頻 (選配)
                stepTimer = (currentSpeed > 3f) ? runStepInterval : walkStepInterval;
            }
        }
        else
        {
            // 停止移動時重置計時器，確保下次起步立刻有聲音
            stepTimer = 0;
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            // 隨機選一個音效
            int index = Random.Range(0, footstepClips.Length);
            // 稍微改變音高，增加隨機感
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(footstepClips[index]);
        }
    }
}