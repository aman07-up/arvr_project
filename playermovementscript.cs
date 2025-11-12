using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Joystick joystick;

    void Update()
    {
        // get joystick direction
        Vector3 input = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        // if joystick moved
        if (input.magnitude > 0.1f)
        {
            // use camera's facing direction for movement
            Transform cam = Camera.main.transform;
            Vector3 forward = cam.forward;
            Vector3 right = cam.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 moveDir = forward * input.z + right * input.x;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
}
