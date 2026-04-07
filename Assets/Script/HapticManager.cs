using UnityEngine;
using System.Collections;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 觸發手把震動
    /// </summary>
    /// <param name="controller">哪隻手 (RTouch, LTouch, 或 Active)</param>
    /// <param name="duration">持續時間 (秒)</param>
    /// <param name="frequency">頻率 (0.0~1.0)</param>
    /// <param name="amplitude">強度 (0.0~1.0)</param>
    public void Vibrate(OVRInput.Controller controller, float duration, float frequency, float amplitude)
    {
        StartCoroutine(DoVibrate(controller, duration, frequency, amplitude));
    }

    private IEnumerator DoVibrate(OVRInput.Controller controller, float duration, float frequency, float amplitude)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, controller);
    }

    // 較輕的
    public void VibrateLight(OVRInput.Controller controller)
    {
        Vibrate(controller, 0.05f, 0.2f, 0.2f);
    }

    // 較重的
    public void VibrateHeavy(OVRInput.Controller controller)
    {
        Vibrate(controller, 0.2f, 0.6f, 0.8f);
    }
}