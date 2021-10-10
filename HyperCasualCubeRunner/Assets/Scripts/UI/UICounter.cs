using System.Collections;
using UnityEngine;
using TMPro;

public class UICounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt = null;

    [Header("String Format:")]
    [Tooltip("Text before counter.")]
    [SerializeField] private string preString = "";
    [SerializeField] private string postString = "";

    [Space]
    [Tooltip("Duration of counting animation")]
    [SerializeField] private float animDuration = 0.5f;

    int current = 0;
    int target = 0;


    Coroutine currentRoutine = null;

    private void Awake()
    {
        if (txt == null)
            Debug.LogWarning(gameObject.name + ": Text not referenced for counter.");
    }

    public void CountTo(int toTarget)
    {
        if (txt == null)
            return;

        target = toTarget;

        //  If we are currently already in an anim:
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(Counter());
    }

    private IEnumerator Counter()
    {
        int start = current;

        for (float timer = 0; timer < animDuration; timer += Time.unscaledDeltaTime)
        {
            float progress = timer / animDuration;
            current = (int)Mathf.Lerp(start, target, progress);

            SetUpdatedString();

            yield return null;
        }

        current = target;
        SetUpdatedString();

        currentRoutine = null;
    }

    private void SetUpdatedString()
    {
        txt.SetText(preString + current.ToString() + postString);
    }
}
