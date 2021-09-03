using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanPlayerUI : MonoBehaviour
{
    [SerializeField] private bool running;
    [SerializeField] private HumanPlayer humanPlayer;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text healthText;

    private void Start()
    {
        humanPlayer = GetComponent<HumanPlayer>();
        running = humanPlayer.isLocalPlayer;
    }

    private void Update()
    {
        if (running)
        {
            healthText.text = string.Format("Health: {0} / {1}", Mathf.CeilToInt(humanPlayer.visibleHealth), Mathf.CeilToInt(humanPlayer.visibleMaxHealth));
        }
        else
        {
            canvas.enabled = false;
        }
    }
}
