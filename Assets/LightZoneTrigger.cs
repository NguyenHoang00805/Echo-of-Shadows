using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightZoneTrigger : MonoBehaviour
{
    [Header("Global Light")]
    public Light2D globalLight;
    public float targetIntensity = 1f;
    public float normalIntensity = 0.2f;
    public float lerpDuration = 1f;

    [Header("Object to Disable")]
    public GameObject objectToDisable;

    private Coroutine currentLerp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (globalLight != null)
            {
                if (currentLerp != null) StopCoroutine(currentLerp);
                currentLerp = StartCoroutine(ChangeLightIntensity(globalLight.intensity, targetIntensity));
            }

            if (objectToDisable != null)
                objectToDisable.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (globalLight != null)
            {
                if (currentLerp != null) StopCoroutine(currentLerp);
                currentLerp = StartCoroutine(ChangeLightIntensity(globalLight.intensity, normalIntensity));
            }

            if (objectToDisable != null)
                objectToDisable.SetActive(true);
        }
    }

    private IEnumerator ChangeLightIntensity(float startValue, float endValue)
    {
        float time = 0f;
        while (time < lerpDuration)
        {
            time += Time.deltaTime;
            globalLight.intensity = Mathf.Lerp(startValue, endValue, time / lerpDuration);
            yield return null;
        }
        globalLight.intensity = endValue;
    }
}
