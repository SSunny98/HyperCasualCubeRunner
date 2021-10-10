using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ModelDisplayManager : MonoBehaviour
{
    [Tooltip("All styles for the player.")]
    [SerializeField] private List<Texture> allStyles = new List<Texture>();

    [Header("Style Display")]
    [Tooltip("Should the model spin?")]
    [SerializeField] private bool shouldSpin = true;
    [Tooltip("Spin speed of model.")]
    [SerializeField] private float spinSpeed = 0.2f;
    [Tooltip("Text to dislpay currently chosen Style ID.")]
    [SerializeField] private TextMeshProUGUI txtStyleID = null;

    [Header("Style Selection")]
    [Tooltip("Should the list loop?")]
    [SerializeField] private bool shouldLoop = true;
    [Tooltip("Currently being displayed style ID.")]
    [SerializeField] int currentlyDisplayedStyle = 0;

    private MeshRenderer meshRenderer = null;
    private PlayerSettings playerSettings = null;
    private CubeTypeManager cubeTypeManager = null;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        cubeTypeManager = CubeTypeManager.Instance;
        if(cubeTypeManager == null)
            Debug.LogWarning(gameObject.name + ": CubeTypeManager Instance not found.");

        playerSettings = GameObject.FindObjectOfType<PlayerSettings>();
        
        //  Get currently saved:
        if (playerSettings)
        {
            SetModelStyleAndText(playerSettings.GetPlayerSkinID());
            SetModelCubeType(playerSettings.GetPlayerStartingType());
        }
        //  Default:
        else
        {
            SetModelStyleAndText(0);
            SetModelCubeType(0);
        }
    }

    private void Update()
    {
        //  Spin model:
        if (shouldSpin)
            UpdateSpin();

    }

    //  Spin:
    private void UpdateSpin()
    {
        Vector3 currentRot = transform.rotation.eulerAngles;
        currentRot.y += spinSpeed;
        if (currentRot.y <= -360.0f || currentRot.y >= 360.0f)
            currentRot.y = 0.0f;
        transform.rotation = Quaternion.Euler(currentRot);
    }

    //  Set model stlye:
    private void SetModelStyleAndText(int styleID)
    {
        currentlyDisplayedStyle = styleID;

        //  Update text:
        txtStyleID.SetText(styleID.ToString());

        if (styleID >= 0 && styleID < allStyles.Count)
            meshRenderer.material.SetTexture("_EmissionMap", allStyles[styleID]);
    }

    public void ChangeStyle(bool next)
    {
        //  Next style in list:
        if (next)
            currentlyDisplayedStyle++;
        //  Previous
        else
            currentlyDisplayedStyle--;

        //  Loop or clamp the ID:
        CheckIDLoopOrClamp(ref currentlyDisplayedStyle, 0, allStyles.Count - 1, shouldLoop);

        //  Update model & Text:
        SetModelStyleAndText(currentlyDisplayedStyle);

        //  Save to PlayerSettings:
        playerSettings.SaveNewPlayerStyle(currentlyDisplayedStyle, allStyles[currentlyDisplayedStyle]);
    }

    private void CheckIDLoopOrClamp(ref int val, int min, int max, bool loop)
    {
        if (loop)
        {
            if (val > max)
                val = min;
            else if (val < min)
                val = max;
        }
        else
            val = Mathf.Clamp(val, min, max);
    }

    //  Change starting colour:
    public void ChangeStartingColour()
    {
        if (!playerSettings || !cubeTypeManager)
            return;

        int currentStarting = playerSettings.GetPlayerStartingType();

        //  Next style in list:
        currentStarting++;

        //  Loop or clamp the ID:
        CheckIDLoopOrClamp(ref currentStarting, 0, cubeTypeManager.GetTotalTypes() - 1, true);

        //  Update model & Text:
        SetModelCubeType(currentStarting);

        //  Save to PlayerSettings:
        playerSettings.SaveNewPlayerStartingType(currentStarting);
    }

    private void SetModelCubeType(int cubeTypeID)
    {
        if (!cubeTypeManager)
            return;

        if (cubeTypeID >= 0 && cubeTypeID < cubeTypeManager.GetTotalTypes())
            meshRenderer.material = cubeTypeManager.GetCubeTypeWithID(cubeTypeID)?.GetCubeMaterial();

        //  Reapply style:
        if (playerSettings)
            SetModelStyleAndText(playerSettings.GetPlayerSkinID());
    }
}
