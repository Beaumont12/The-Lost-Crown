using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;
    private Vector3 lastCameraPosition;
    public float parallaxSpeed = 0.5f; // Adjust for smooth effect

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxSpeed, 0, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
