using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLIght : MonoBehaviour
{
    Transform mainLight;
    Transform flickerLight;
    Light2D mainLightComponent;
    Light2D flickerLightComponent;
    [SerializeField] private float minFlickIntens, maxFlickIntens;
    [SerializeField] private float minTime, maxTime;


    // Start is called before the first frame update
    void Start()
    {
        mainLight = this.transform.GetChild(0);
        flickerLight = this.transform.GetChild(1);
        mainLightComponent = mainLight.GetComponent<Light2D>();
        flickerLightComponent = flickerLight.GetComponent<Light2D>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        for (; ; ) // = while(true)
        {
            float randomIntensity = Random.Range(minFlickIntens, maxFlickIntens);
            flickerLightComponent.intensity = randomIntensity;

            float randomTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
