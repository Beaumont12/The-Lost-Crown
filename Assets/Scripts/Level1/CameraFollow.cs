using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.1f;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = new Vector3(player.position.x, player.position.y + 1, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
        }
    }
}
