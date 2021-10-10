using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Slow Motion")]
    [Tooltip("Value of timescale during slow mo.")]
    [SerializeField] private float slowmoFactor = 0.05f;
    [Tooltip("Duration of the slow motion.")]
    [SerializeField] private float slowmoDuration = 2.0f;
    bool slowmoActive = false;

    private float defaultFixedDelta = 0.02f;

    private void Awake()
    {
        defaultFixedDelta = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (!slowmoActive)
            return;

        //  Slowly increase timescale again:
        Time.timeScale += (1.0f / slowmoDuration) * Time.unscaledDeltaTime;
        //  If we are back up to normal speed:
        if (Time.timeScale > 1.0f)
            DeactivateSlowMo();
    }

    public void ActivateSlowMo()
    {
        //  Update timescales:
        Time.timeScale = slowmoFactor;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Time.fixedDeltaTime = Time.timeScale * defaultFixedDelta;

        slowmoActive = true;
    }

    public void DeactivateSlowMo()
    {
        //  Reet timescale and bool:
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * defaultFixedDelta;
        slowmoActive = false;
    }
}
