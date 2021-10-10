using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionMiniCube : MonoBehaviour
{
    private MeshRenderer mr = null;
    private Rigidbody rb = null;

    [SerializeField] private bool isCubeActive = false;

    //  Height:
    private float resetHeight = -20.0f;


    private void Awake()
    {
        //  Find components:
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isCubeActive)
            return;

        //  Check world height:
        if (transform.position.y < resetHeight)
            DeactivateCube();
    }

    //  Initialise mini cube:
    public void InitMiniCube(float newResetHeight)
    {
        resetHeight = newResetHeight;
    }

    //  Activate cube (starting explosion):
    public void ActivateCube(Material newMat, Vector3 newVel)
    {
        gameObject.SetActive(true);

        mr.material = newMat;
        rb.velocity = newVel;

        isCubeActive = true;
    }

    //  Deactivate cube:
    public void DeactivateCube()
    {
        gameObject.SetActive(false);
        isCubeActive = false;

        rb.velocity = Vector3.zero;
    }

}
