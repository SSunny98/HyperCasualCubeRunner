using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Tweening:
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [Header("To Default motion")]
    [SerializeField] private Transform defaultCamPos = null;
    [SerializeField] private float defaultSwitchDuration = 0.2f;
    [SerializeField] private Ease defaultEase = Ease.InOutBack;

    [Header("To Fast motion")]
    [SerializeField] private Transform fastCamPos = null;
    [SerializeField] private float fastModeSwitchDuration = 0.2f;
    [SerializeField] private Ease fastModeEase = Ease.InOutExpo;

    CameraShaker cameraShaker = null;

    [Space]
    [SerializeField] private Material currentBGMat = null;
    private Material hyperBGMaterial = null;
    PlayerMovement player = null;

    private void Awake()
    {
        cameraShaker = GetComponent<CameraShaker>();
        player = GameObject.FindObjectOfType<PlayerMovement>();

    }

    private void Start()
    {
        //  Subscribe to player hyper events:
        if(player != null)
        {
            player.onHyperActivated += ApplyHyperBGMaterial;
            player.onHyperDeactivated += ApplyCurrentBackgroundMaterial;
        }

        //  Get hyper material:
        hyperBGMaterial = GameObject.FindObjectOfType<CubeTypeManager>()?.GetHyperBGMaterial();
    }

    public void ToDefaultCam()
    {
        //  To new position:
        transform.DOLocalMove(defaultCamPos.localPosition, defaultSwitchDuration)
            .SetEase(defaultEase);
        //  To new rotation:
        transform.DOLocalRotateQuaternion(defaultCamPos.localRotation, defaultSwitchDuration)
            .SetEase(defaultEase);

    }

    public void ToFastCam()
    {
        //  To new position:
        transform.DOLocalMove(fastCamPos.localPosition, fastModeSwitchDuration)
            .SetEase(fastModeEase);
        //  To new rotation:
        transform.DOLocalRotateQuaternion(fastCamPos.localRotation, fastModeSwitchDuration)
            .SetEase(fastModeEase);
    }

    //  Camera shake:
    public void CameraShake()
    {
        cameraShaker.Shake();
    }

    //  Background colour:
    public void SetBackgroundColour(Material newMat)
    {
        //  Save mat:
        currentBGMat = newMat;

        //  Apply new material if player is NOT in hyper mode:
        if (!player.GetIsHyper())
            ApplyCurrentBackgroundMaterial();
    }

    //  Apply currently saved material:
    private void ApplyCurrentBackgroundMaterial()
    {
        RenderSettings.skybox = currentBGMat;
    }

    //  Apply hyper material:
    private void ApplyHyperBGMaterial()
    {
        //if(hyperBGMaterial)
            RenderSettings.skybox = hyperBGMaterial;
    }
}
