using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Camera cam;

    [Tooltip("Amount of shake.")]
    [SerializeField] private Vector3 shakeAmount = new Vector3(1.0f, 1.0f, 0.0f);

    [Tooltip("Duration of shake.")]
    [SerializeField] private float shakeDuration = 1;

    [Tooltip("Speed to shake.")]
    [SerializeField] private float shakeSpeed = 10;

    [Tooltip("Shake amount over lifetime [0.0f, 1.0f].")]
    [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);

    [Tooltip("True: The camera position is set in reference to the old position of the camera.\n" +
        "False: The camera position is set in absolute values or is fixed to an object.")]
    [SerializeField] private bool deltaMovement = true;

    private float time = 0;
    private Vector3 lastPos;
    private Vector3 nextPos;
    private float lastFoV;
    private float nextFoV;
    private bool destroyAfterPlay = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Shake()
    {
        ResetCam();
        time = shakeDuration;
    }

    private void LateUpdate()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time > 0)
            {
                //  Perlin noise for next pos:
                nextPos = (Mathf.PerlinNoise(time * shakeSpeed, time * shakeSpeed * 2) - 0.5f) * shakeAmount.x * transform.right * shakeCurve.Evaluate(1f - time / shakeDuration) +
                          (Mathf.PerlinNoise(time * shakeSpeed * 2, time * shakeSpeed) - 0.5f) * shakeAmount.y * transform.up * shakeCurve.Evaluate(1f - time / shakeDuration);
                nextFoV = (Mathf.PerlinNoise(time * shakeSpeed * 2, time * shakeSpeed * 2) - 0.5f) * shakeAmount.z * shakeCurve.Evaluate(1f - time / shakeDuration);

                cam.fieldOfView += (nextFoV - lastFoV);
                cam.transform.Translate(deltaMovement ? (nextPos - lastPos) : nextPos);

                lastPos = nextPos;
                lastFoV = nextFoV;
            }
            else
            {
                //last frame
                ResetCam();
                if (destroyAfterPlay)
                    Destroy(this);
            }
        }
    }

    private void ResetCam()
    {
        //  Reset the last delta:
        cam.transform.Translate(deltaMovement ? -lastPos : Vector3.zero);
        cam.fieldOfView -= lastFoV;

        //  Clear values:
        lastPos = nextPos = Vector3.zero;
        lastFoV = nextFoV = 0f;
    }

}
