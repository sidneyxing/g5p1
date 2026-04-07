using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndTalk : MonoBehaviour
{
    public Image blackFadeImage; 
    public TextMeshProUGUI endingText; 
    public float fadeSpeed = 0.5f;

    [TextArea(3, 10)]
    public string happyEndingQuote = "溝通的過程雖然辛苦，但最終可以找回彼此相愛的模樣。";
    [TextArea(3, 10)]
    public string sadEndingQuote = "沈默並非和平，它只是讓愛，變成了再也聽不見的雜訊。";

    public void StartEndingSequence()
    {
        // 判斷理智值決定文字
        float sanity = 50f;
        if (SanitySystem.Instance != null) sanity = SanitySystem.Instance.currentSanity;

        endingText.text = (sanity > 50) ? happyEndingQuote : sadEndingQuote;

        StartCoroutine(ExecuteFade());
    }

    IEnumerator ExecuteFade()
    {
        float alpha = 0;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            blackFadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        float textAlpha = 0;
        while (textAlpha < 1f)
        {
            textAlpha += Time.deltaTime * fadeSpeed;
            endingText.color = new Color(1, 1, 1, textAlpha);
            yield return null;
        }
    }
}