using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    Vector2 startingPosition;
    float startingz;
    Vector2 camMoveSinceStart => (Vector2) cam.transform.position - startingPosition;
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => (cam.transform.position.z - (zDistanceFromTarget > 0 ? cam.nearClipPlane: cam.farClipPlane));
    float parallexFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingz = transform.position.z;
    } 

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallexFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingz);
    }
}
