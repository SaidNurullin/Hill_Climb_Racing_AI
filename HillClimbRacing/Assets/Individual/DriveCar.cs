using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveCar : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTyreRB;
    [SerializeField] private Rigidbody2D backTyreRB;
    [SerializeField] private Rigidbody2D carRB;

    [SerializeField] private float speed = 150f;
    [SerializeField] private float rotationSpeed = 300f;
    [SerializeField] private float maxAngularVelocity = 100f;

    public void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Move(moveInput);
    }

    public void Move(float moveInput)
    {
        if(Mathf.Abs(frontTyreRB.angularVelocity) < maxAngularVelocity)
        {
            frontTyreRB.AddTorque(-moveInput * speed * Time.fixedDeltaTime);
        }
        if (Mathf.Abs(backTyreRB.angularVelocity) < maxAngularVelocity)
        {
            backTyreRB.AddTorque(-moveInput * speed * Time.fixedDeltaTime);
        }

        carRB.AddTorque(moveInput * rotationSpeed * Time.fixedDeltaTime);
    }
}
