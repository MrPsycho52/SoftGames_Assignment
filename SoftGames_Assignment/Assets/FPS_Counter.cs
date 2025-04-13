using UnityEngine;
using TMPro;

public class FPS_Counter : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI fpsText;

    [Header("Settings")]
    public float updateInterval = 0.5f;

    private float timeSinceLastUpdate;
    private int frames;
    private float fps;

    void Update()
    {
        frames++;
        timeSinceLastUpdate += Time.unscaledDeltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            fps = frames / timeSinceLastUpdate;
            fpsText.text = $"FPS: {Mathf.RoundToInt(fps)}";

            frames = 0;
            timeSinceLastUpdate = 0f;
        }
    }
}
