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

    private float _input = 0;

    public void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(Mathf.Abs(frontTyreRB.angularVelocity) < maxAngularVelocity)
        {
            frontTyreRB.AddTorque(-_input * speed * Time.fixedDeltaTime);
        }
        if (Mathf.Abs(backTyreRB.angularVelocity) < maxAngularVelocity)
        {
            backTyreRB.AddTorque(-_input * speed * Time.fixedDeltaTime);
        }

        carRB.AddTorque(_input * rotationSpeed * Time.fixedDeltaTime);
    }


    public void SetInput(float input)
    {
        _input = input;
    }
}
