using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  Base class for a general collectable item:
public class Collectable : MonoBehaviour
{
    [Header("Base Collectable Properties:")]
    [SerializeField] protected bool isCollectableActive = false;
    [Tooltip("MeshFilter for GFX.")]
    [SerializeField] protected MeshFilter meshFilter = null;
    [Tooltip("MeshRenderer for GFX.")]
    [SerializeField] protected MeshRenderer meshRenderer = null;

    [Space]
    [Tooltip("Should deactivate after going past camera (out of view).")]
    [SerializeField] private bool deactivatePastCamera = true;
    [Tooltip("Deactivate after this distance past the collectable.")]
    [SerializeField] private float distancePastCollectable = 5.0f;

    protected Animator animator = null;
    protected CubeExploder cubeExploder = null;

    protected PlayerMovement player = null;
    protected Camera cam = null;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        cubeExploder = GetComponent<CubeExploder>();

        player = GameObject.FindObjectOfType<PlayerMovement>();
        cam = Camera.main;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (!isCollectableActive)
            return;

        //  Check if camera has gone passed this object (out of view):
        if (deactivatePastCamera)
        {
            //  Deactivate collectable:
            if (cam.transform.position.z > (transform.position.z + distancePastCollectable))
                DeactivateCollectable();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isCollectableActive)
            return;

        if (other.CompareTag("Player"))
            OnHitPlayer();
    }

    //  When collided with player:
    protected virtual void OnHitPlayer() { }


    public virtual void OnSpawned()
    {
        isCollectableActive = true;
        meshRenderer.enabled = true;
    }

    public virtual void DeactivateCollectable()
    {
        isCollectableActive = false;
        meshRenderer.enabled = false;
    }

    public bool GetIsCollectableActive() { return isCollectableActive; }

    //  Is this collectable ready to be spawned?
    public virtual bool GetIsReadyToSpawn() { return !isCollectableActive; }
}
