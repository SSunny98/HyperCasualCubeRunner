using UnityEngine;

public enum PowerType
{
    HyperBoost,
    ExtraPoints,
    ExtraLife
}

[System.Serializable]
public class Power
{
    [SerializeField] private string powerName = "UNKNOWN";

    [Tooltip("Powerup Model")]
    [SerializeField] private Mesh powerMesh = null;
    [Tooltip("Powerup Model Material")]
    [SerializeField] private Material powerMeshMaterial = null;

    [Tooltip("Powerup Icon")]
    [SerializeField] private Sprite powerSprite = null;

    [Tooltip("Powerup Value")]
    [SerializeField] private float powerValue = 5;

    [Tooltip("Powerup Type")]
    [SerializeField] private PowerType powerType = PowerType.HyperBoost;

    [Tooltip("Is this powerup isntantly used or needs a timer?")]
    [SerializeField] private bool needsTimer = true;

    public Mesh GetPowerMesh() { return powerMesh; }
    public Material GetPowerMeshMaterial() { return powerMeshMaterial; }
    public Sprite GetPowerSprite() { return powerSprite; }
    public float GetPowerValue() { return powerValue; }
    public PowerType GetPowerType() { return powerType; }
    public bool GetPowerNeedsTimer() { return needsTimer; }
}
