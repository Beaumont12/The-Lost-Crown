using UnityEngine;

public class SurikenMover : MonoBehaviour
{
    public enum MovementDirection { Horizontal, Vertical, RightDiagonal, LeftDiagonal }
    public MovementDirection direction = MovementDirection.Horizontal;

    public float speed = 2f;
    public float distance = 3f;
    public float rotationSpeed = 180f; // Spin speed

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float movement = Mathf.PingPong(Time.time * speed, distance);
        Vector3 offset = Vector3.zero;

        switch (direction)
        {
            case MovementDirection.Horizontal:
                offset = new Vector3(movement, 0f, 0f);
                break;
            case MovementDirection.Vertical:
                offset = new Vector3(0f, movement, 0f);
                break;
            case MovementDirection.RightDiagonal:
                offset = new Vector3(movement, movement, 0f);
                break;
            case MovementDirection.LeftDiagonal:
                offset = new Vector3(-movement, movement, 0f);
                break;
        }

        transform.position = startPos + offset;

        // Optional spinning effect
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
