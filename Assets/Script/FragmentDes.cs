using UnityEngine;

public class FragmentDes : MonoBehaviour
{
    

    [Header("目標門與環境")]
    public DoorManager targetDoor;  
    public int totalFragments = 9;

    [Header("預製物替換 (Prefab)")]
    public GameObject burntPaperPrefab; 
    public GameObject completePaperPrefab; 
    public Transform spawnPoint; 

    [Header("生成物旋轉角度 (Euler Angles)")]
    public Vector3 completePaperRotation = Vector3.zero;
    public Vector3 burntPaperRotation = Vector3.zero;

    [Header("音效與特效")]
    public AudioSource audioSource;
    public AudioClip parentsSigh; 
    public AudioClip fatherAngry;

    [Header("火的咚咚")]
    public AudioSource fireSource;
    public AudioClip fireLoopSFX;

    void StartBurning()
    {
        fireSource.clip = fireLoopSFX;
        fireSource.loop = true;
        fireSource.Play();
    }

    void StopBurning()
    {
        fireSource.Stop();
    }

    private static int gluedCount = 0;
    private static bool isFinished = false;

    private bool isThisPieceGlued = false;

    private void Start()
    {
        gluedCount = 0;
        isFinished = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFinished) return;

        if (other.CompareTag("lighter"))
        {
            StartBurning();
            TriggerFinalState(false);
        }

        if (other.CompareTag("glue") && !isThisPieceGlued)
        {
            isThisPieceGlued = true;
            gluedCount++;
            
            GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.1f, 0.8f); 
            HapticManager.Instance.VibrateLight(OVRInput.Controller.Active);

            Debug.Log($"已修復: {gluedCount}/{totalFragments}");

            if (gluedCount >= totalFragments)
            {
                TriggerFinalState(true);
            }
        }
    }

    void TriggerFinalState(bool isSuccess)
        {
            if (isFinished) return;
            isFinished = true;

            GameObject[] allFragments = GameObject.FindGameObjectsWithTag("Fragment");
            foreach (GameObject f in allFragments) f.SetActive(false);

            if (isSuccess)
            {
                Debug.Log("拼湊成功！生成完整紙張。");
                if (completePaperPrefab != null) 
                {
                    // 使用 Quaternion.Euler 將 Vector3 轉換為角度
                    Quaternion spawnRotation = Quaternion.Euler(completePaperRotation);
                    Instantiate(completePaperPrefab, spawnPoint.position, spawnRotation);
                }
                
                if (audioSource && parentsSigh) audioSource.PlayOneShot(parentsSigh);
                if (targetDoor != null) targetDoor.OpenDoor(true);
                SanitySystem.Instance.ChangeSanity(-15);
            }
            else
            {
                Debug.Log("被燒毀了！生成燒焦紙張。");

                if (burntPaperPrefab != null) 
                {
                    // 使用 Quaternion.Euler 將 Vector3 轉換為角度
                    Quaternion spawnRotation = Quaternion.Euler(burntPaperRotation);
                    Instantiate(burntPaperPrefab, spawnPoint.position, spawnRotation);
                }

                if (audioSource && fatherAngry) {
                    audioSource.PlayOneShot(fatherAngry);
                    StopBurning();
                }
                
                HapticManager.Instance.VibrateHeavy(OVRInput.Controller.Active);
                SanitySystem.Instance.ChangeSanity(15);
                if (targetDoor != null) targetDoor.OpenDoor(false);
            }
        }
    }