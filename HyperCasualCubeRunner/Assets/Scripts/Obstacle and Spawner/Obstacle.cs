using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool isObstacleActive = false;
    CubeExploder cubeExploder = null;
    [SerializeField] private MeshRenderer meshRenderer = null;
    private Material mainMaterial = null;
    private Material hyperMaterial = null;

    private ObstaclesSpawner spawner = null;
    private Camera cam = null;

    int cubeTypeID = 0;

    private PlayerMovement player = null;

    private void Awake()
    {
        //  Find exploder component:
        cubeExploder = GetComponent<CubeExploder>();
        player = GameObject.FindObjectOfType<PlayerMovement>();

        //  Find main camera:
        cam = Camera.main;
    }

    private void Start()
    {
        //  Subscribe to events:
        if (player != null)
        {
            player.onHyperActivated += ApplyHyperMaterial;
            player.onHyperDeactivated += ApplyMainMaterial;
        }

        if (cubeExploder != null)
        {
            cubeExploder.onFinishedExploding += OnFinishedExploding;
        }

        //  Get hyper material:
        hyperMaterial = GameObject.FindObjectOfType<CubeTypeManager>()?.GetHyperMaterial();
    }

    private void Update()
    {
        if (isObstacleActive)
        {
            //  Check if camera has gone passed this obstacle (out of view):
            if (cam.transform.position.z > (transform.position.z + 5))
            {
                //  Deactivate obstacle:
                DeactivateObstacle();

                //  Notify spawner:
                spawner.OnPassedObstacle(transform.position.z);
            }
        }
    }

    //  Set reference to thise spawner:
    public void SetSpawnerRef(ObstaclesSpawner newSpawner) { spawner = newSpawner; }

    private void OnTriggerEnter(Collider other)
    {
        if (!isObstacleActive)
            return;

        if (other.CompareTag("Player"))
            player.OnHitObstacle(this);
    }

    //  Activate:
    public void ActivateObstacle(CubeType cType)
    {
        //  Apply cube type properties:
        if (cType != null)
        {
            mainMaterial = cType.GetCubeMaterial();

            if (player.GetIsHyper())
                ApplyHyperMaterial();
            else
                ApplyMainMaterial();

            cubeTypeID = cType.GetCubeID();
        }

        isObstacleActive = true;
        meshRenderer.enabled = true;
    }

    //  Deactivate:
    public void DeactivateObstacle()
    {
        isObstacleActive = false;
        meshRenderer.enabled = false;
    }

    public void ApplyHyperMaterial()
    {
        if (hyperMaterial)
            meshRenderer.material = hyperMaterial;
    }
    public void ApplyMainMaterial()
    {
        if (mainMaterial)
            meshRenderer.material = mainMaterial;
    }


    //  Deactivate & Explode:
    public void ExplodeObstacle(Vector3 transferVelocity)
    {
        //  Reset:
        DeactivateObstacle();

        //  Explode:
        cubeExploder.ExplodeCube(transferVelocity, meshRenderer.material);
    }

    public void OnFinishedExploding()
    {
        //  Spawn:
        spawner.SpawnRowOfObstacles();
    }

    public bool GetIsObstacleActive() { return isObstacleActive; }

    public bool GetIsExploding() { return cubeExploder.GetIsExploding(); }

    public int GetCubeType() { return cubeTypeID; }

}
