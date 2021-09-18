using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanPlayerUI : MonoBehaviour
{
    private HumanPlayer player;
    private bool runThisScript;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Text healthText;
    [SerializeField] private Text staminaText;
    [SerializeField] private Text blinkText;
    [SerializeField] private Image overlayGasMask;
    [SerializeField] private Image[] overlayBlink;
    [SerializeField] private Vector3[] overlayBlinkOrigin;

    private void Start()
    {
        player = GetComponent<HumanPlayer>();
        runThisScript = player.isLocalPlayer;

        overlayBlinkOrigin = new Vector3[] { overlayBlink[0].transform.localPosition, overlayBlink[1].transform.localPosition };
    }

    private void Update()
    {
        if (runThisScript)
        {
            float[] health = player.GetHealth();
            float[] stamina = player.GetStamina();
            float[] blink = player.GetBlink();

            healthText.text = string.Format("HP: {0} / {1}", Mathf.CeilToInt(health[0]), Mathf.CeilToInt(health[1]));
            staminaText.text = string.Format("ST: {0} / {1}", Mathf.CeilToInt(stamina[0]), Mathf.CeilToInt(stamina[1]));
            blinkText.text = string.Format("BT: {0} / {1}", Mathf.CeilToInt(blink[0]), Mathf.CeilToInt(blink[1]));

            if (blink[0] == 0.0f)
            {
                overlayBlink[0].transform.localPosition = Vector3.Lerp(overlayBlink[0].transform.localPosition, new Vector3(0, -510, 0), 0.2f);
                overlayBlink[1].transform.localPosition = Vector3.Lerp(overlayBlink[1].transform.localPosition, new Vector3(0, 510, 0), 0.2f);
            }
            else
            {
                overlayBlink[0].transform.localPosition = Vector3.Lerp(overlayBlink[0].transform.localPosition, overlayBlinkOrigin[0], 0.2f);
                overlayBlink[1].transform.localPosition = Vector3.Lerp(overlayBlink[1].transform.localPosition, overlayBlinkOrigin[1], 0.2f);
            }

            if (player.GetGasMask())
            {
                overlayGasMask.color = new Color(overlayGasMask.color.r, overlayGasMask.color.g, overlayGasMask.color.b, 1.0f);
            }
            else
            {
                overlayGasMask.color = new Color(overlayGasMask.color.r, overlayGasMask.color.g, overlayGasMask.color.b, 0.0f);
            }
        }
        else
        {

        }
    }
}
