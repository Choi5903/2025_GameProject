using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    public Transform layerTransform;
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;
}

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTarget;
    public ParallaxLayer[] layers;

    private Vector3 previousCamPos;

    private void Start()
    {
        if (cameraTarget == null)
        {
            cameraTarget = Camera.main.transform;
        }
        previousCamPos = cameraTarget.position;
    }

    private void LateUpdate()
    {
        Vector3 camDelta = cameraTarget.position - previousCamPos;

        foreach (var layer in layers)
        {
            if (layer.layerTransform == null) continue;

            Vector3 newPos = layer.layerTransform.position + new Vector3(camDelta.x * layer.parallaxFactor, camDelta.y * layer.parallaxFactor, 0);
            layer.layerTransform.position = newPos;
        }

        previousCamPos = cameraTarget.position;
    }
}
