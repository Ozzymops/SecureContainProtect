using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    private Image healthBlocks;
    private Image staminaBlocks;
    private Image blinkMeterTop;
    private Image blinkMeterBot;
    private Image blinkOverlayTop;
    private Image blinkOverlayBot;

    public void GetElements(Canvas canvas)
    {
        blinkOverlayTop = canvas.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        blinkOverlayBot = canvas.transform.GetChild(0).GetChild(1).GetComponent<Image>();
        healthBlocks = canvas.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
        staminaBlocks = canvas.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>();
        blinkMeterTop = canvas.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>();
        blinkMeterBot = canvas.transform.GetChild(4).GetChild(0).GetChild(1).GetComponent<Image>();
    }

    public void UpdateElements(float[] data)
    {
        healthBlocks.fillAmount = Mathf.Ceil((data[0] / data[1]) * 10.0f) * 0.1f;
        staminaBlocks.fillAmount = Mathf.Ceil((data[2] / data[3]) * 10.0f) * 0.1f;
        blinkMeterTop.fillAmount = data[4] / data[5];
        blinkMeterBot.fillAmount = data[4] / data[5];
    }

    public void Blink(bool blinking)
    {
        if (blinking)
        {
            blinkOverlayTop.transform.localPosition = new Vector2(0, Mathf.Lerp(blinkOverlayTop.transform.localPosition.y, 0, 0.05f));
            blinkOverlayBot.transform.localPosition = new Vector2(0, Mathf.Lerp(blinkOverlayBot.transform.localPosition.y, 0, 0.05f));
        }
        else
        {
            blinkOverlayTop.transform.localPosition = new Vector2(0, Mathf.Lerp(blinkOverlayTop.transform.localPosition.y, 1080, 0.05f));
            blinkOverlayBot.transform.localPosition = new Vector2(0, Mathf.Lerp(blinkOverlayBot.transform.localPosition.y, -1080, 0.05f));
        }
    }
}
