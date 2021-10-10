using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : Collectable
{
    [Header("Converter Properties:")]
    [Tooltip("When collected, change player cube type to this ID.")]
    [SerializeField] int changeToTypeID = 0;

    CubeTypeManager cubeTypeManager = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        //  Get manager instance:
        cubeTypeManager = CubeTypeManager.Instance;
    }

    public override void OnSpawned()
    {
        base.OnSpawned();

        //  Get a new type excluding current type:
        CubeType newType = cubeTypeManager.GetRandomCubeTypeExclude(player.GetCurrentCubeType());

        //  Apply properties:
        if (newType != null)
        {
            changeToTypeID = newType.GetCubeID();
            meshRenderer.material = newType.GetCubeMaterial();
        }

        animator.SetTrigger("Hover");
    }

    //  Deactivate converter, should be called after collection animation:
    public override void DeactivateCollectable()
    {
        base.DeactivateCollectable();
    }

    protected override void OnHitPlayer()
    {
        base.OnHitPlayer();

        //  Convert player cube type:
        player.OnHitConverter(cubeTypeManager.GetCubeTypeWithID(changeToTypeID));

        //  Animate:
        animator.SetTrigger("Collected");
        //  Cube exploder:
        cubeExploder.ExplodeCube(player.GetCurrentVelocity(), meshRenderer.material);
    }

    public int GetCubeTypeToChangeTo() { return changeToTypeID; }


    //  Converter is ready to spawn if it is currently NOT active and NOT exploding:
    public override bool GetIsReadyToSpawn() { return !isCollectableActive && !cubeExploder.GetIsExploding(); }
}
