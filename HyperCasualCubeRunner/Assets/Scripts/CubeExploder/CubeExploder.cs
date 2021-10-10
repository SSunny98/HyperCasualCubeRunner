using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExploder : MonoBehaviour
{
    [Tooltip("Mini cube size.")]
    [SerializeField] private float cubeSize = 0.2f;
    [Tooltip("Number of cubes in a row.")]
    [SerializeField] private int rowCount= 5;

    List<ExplosionMiniCube> allMiniCubes = new List<ExplosionMiniCube>();

    [Space]
    [SerializeField] private bool isExploding = false;

    [Header("MiniCubes Self Reset")]
    [Tooltip("Time for explosion cubes to stay alive for.")]
    [SerializeField] private float resetTime = 5.0f;
    [SerializeField] private float currentTimer = 0.0f;
    [Tooltip("World height at which explosion cubes is reset.")]
    [SerializeField] private float resetHeight = -20.0f;

    private GameObject explodedCubeParent = null;

    //  Using particle system instead:
    [Header("Alternative Particle System")]
    [Tooltip("Toggle to use particle system instead of custom mesh explosion.")]
    [SerializeField] private bool useParticleSystem = true;
    [SerializeField] private GameObject pfExplosion = null;
    private ParticleSystem psExplosion = null;
    private ParticleSystemRenderer psExplosionRenderer = null;


    //  Delegates for specific events:
    public delegate void OnFinishedExploding();
    public OnFinishedExploding onFinishedExploding;

    private void Start()
    {
        //  Create minicubes:
        InitMiniCubes();
    }

    private void Update()
    {
        if (!isExploding)
            return;

        //  Update self reset timer:
        currentTimer += Time.deltaTime;
        if (currentTimer >= resetTime)
            ResetExplosion();
    }

    public void InitMiniCubes()
    {

        if (useParticleSystem)
        {
            explodedCubeParent = Instantiate(pfExplosion, this.transform);
            psExplosion = explodedCubeParent.GetComponent<ParticleSystem>();
            psExplosionRenderer = explodedCubeParent.GetComponent<ParticleSystemRenderer>();
        }
        else
        {
            //  Create a parent for exploded miniCubes:
            explodedCubeParent = new GameObject("ExplodedCube");
            explodedCubeParent.transform.parent = this.transform;
            explodedCubeParent.transform.localPosition = Vector3.zero;

            //  Create a x*y*z cube made from mini cubes:
            for (int x = 0; x < rowCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    for (int z = 0; z < rowCount; z++)
                    {
                        CreateMiniCube(x, y, z);
                    }
                }
            }
        }

    }

    void CreateMiniCube(int x, int y, int z)
    {
        //  New GO:
        GameObject goMiniCube;

        //  Add cube mesh:
        goMiniCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //  Re-parent:
        goMiniCube.transform.parent = explodedCubeParent.transform;

        //  Add Rigid body:
        Rigidbody rb = goMiniCube.AddComponent<Rigidbody>();
        rb.mass = 0.2f;
        rb.velocity = Vector3.zero;
        //  Add minicube script:
        ExplosionMiniCube miniCube = goMiniCube.AddComponent<ExplosionMiniCube>();
        miniCube.InitMiniCube(resetHeight);
        miniCube.DeactivateCube();
        allMiniCubes.Add(miniCube);

        //  Set layer to debris so that player doesnt collide with mini cubes:
        goMiniCube.layer = LayerMask.NameToLayer("Debris");

        //  Set cube position and scale:
        goMiniCube.transform.localPosition = /*transform.position +*/ new Vector3((cubeSize * x * 0.8f), cubeSize * y * 0.8f, cubeSize * z * 0.8f);
        goMiniCube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
    }

    //  Explode all mini cubes (set new material and transfer the velocity):
    public void ExplodeCube(Vector3 transferVelocity, Material mat)
    {
        isExploding = true;
        currentTimer = 0.0f;

        if (useParticleSystem)
        {
            psExplosionRenderer.material = mat;
            psExplosion.Play();
        }
        else
        {
            int counter = 0;
            //  Create a x*y*z cube made from mini cubes:
            for (int x = 0; x < rowCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    for (int z = 0; z < rowCount; z++)
                    {
                        allMiniCubes[counter].transform.localPosition =/*transform.position +*/ new Vector3((cubeSize * x * 0.8f), cubeSize * y * 0.8f, cubeSize * z * 0.8f);
                        allMiniCubes[counter].ActivateCube(mat, transferVelocity);
                        counter++;
                    }
                }
            }
        }
    }

    //  Reset explosion:
    public void ResetExplosion()
    {
        isExploding = false;
        currentTimer = 0.0f;

        if (!useParticleSystem)
        {
            foreach (ExplosionMiniCube miniCube in allMiniCubes)
            {
                miniCube.DeactivateCube();
            }
        }

        //  Call delegate:
        if (onFinishedExploding != null)
            onFinishedExploding.Invoke();

    }

    public bool GetIsExploding() { return isExploding; }

}
