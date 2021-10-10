using UnityEngine;


public class Powerup : Collectable
{
    [Header("Powerup Properties:")]
    [SerializeField] private Power power = null;

    private PowerupManager powerupManager = null;
    private PowerupController powerupController = null;

    protected override void Awake()
    {
        base.Awake();

        powerupManager = GameObject.FindObjectOfType<PowerupManager>();
        powerupController = GameObject.FindObjectOfType<PowerupController>();
    }

    //  Collided with player:
    protected override void OnHitPlayer()
    {
        base.OnHitPlayer();

        //  Add power to player:
        powerupController.AddPower(power);

        //  Animate:
        animator.SetTrigger("Collected");

        //  Cube exploder:
        cubeExploder.ExplodeCube(player.GetCurrentVelocity(), meshRenderer.material);
    }

    public override void OnSpawned()
    {
        base.OnSpawned();

        //  Get a new power:
        power = powerupManager.GetRandomPower();

        //  Apply new properties:
        meshFilter.mesh = power.GetPowerMesh();
        meshRenderer.material = power.GetPowerMeshMaterial();

        //  Animate:
        animator.SetTrigger("Hover");
    }

    public override void DeactivateCollectable()
    {
        base.DeactivateCollectable();
    }

    //  Powerup is ready to spawn if it is currently NOT active and NOT exploding:
    public override bool GetIsReadyToSpawn() { return !isCollectableActive && !cubeExploder.GetIsExploding(); }

}
