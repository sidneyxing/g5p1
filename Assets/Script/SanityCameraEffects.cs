using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // 必須引用 URP 命名空間

public class SanityCameraEffects : MonoBehaviour
{
    // 拖入 CenterEyeAnchor 上的 Volume
    public Volume sanityVolume;

    // 用來儲存效果的變數
    private ColorAdjustments _colorAdjustments;
    private Vignette _vignette;
    private ChromaticAberration _chromatic;

    void Start()
    {
        // 從 Volume Profile 中取得對應的效果
        if (sanityVolume.profile.TryGet(out _colorAdjustments))
        {
            _colorAdjustments.active = true;
        }
        if (sanityVolume.profile.TryGet(out _vignette))
        {
            _vignette.active = true;
        }
        if (sanityVolume.profile.TryGet(out _chromatic))
        {
            _chromatic.active = true;
        }
    }

    void Update()
    {
        // 1. 計算瘋狂係數 (0為正常，1為完全瘋狂)
        // 假設你的 SanitySystem.Instance.currentSanity 是 100~0
        float currentSan = SanitySystem.Instance.currentSanity;
        float madness = 1f - (currentSan / 100f); 

        // 2. 平滑套用效果 (使用 Mathf.Lerp)
        
        // 飽和度：0(正常) -> -100(黑白)
        _colorAdjustments.saturation.value = Mathf.Lerp(0f, -100f, madness);
        
        // 對比度：0 -> 50
        _colorAdjustments.contrast.value = Mathf.Lerp(0f, 50f, madness);

        // 邊緣黑色遮罩強度：0.2 -> 0.6
        _vignette.intensity.value = Mathf.Lerp(0.2f, 0.6f, madness);

        // 色散強度：0 -> 1
        _chromatic.intensity.value = Mathf.Lerp(0f, 1f, madness);
    }
}