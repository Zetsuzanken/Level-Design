using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    private Vector3 externalForce;
    public float externalForceDamping = 5f;

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Calculate the movement velocity.
        Vector3 movementVelocity = transform.rotation * new Vector3(targetVelocity.x, 0, targetVelocity.y);

        // Apply damping to the external force
        externalForce = Vector3.Lerp(externalForce, Vector3.zero, externalForceDamping * Time.fixedDeltaTime);

        // Combine movement velocity and external force
        Vector3 velocity = movementVelocity + externalForce;

        // Apply the calculated velocity, preserving the vertical velocity (gravity, jumping).
        velocity.y = rigidbody.velocity.y;

        rigidbody.velocity = velocity;
    }

    public void AddExternalForce(Vector3 force)
    {
        externalForce += force;
    }
}