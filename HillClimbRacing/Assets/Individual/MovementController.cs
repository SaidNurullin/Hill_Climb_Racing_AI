using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTyreRB;
    [SerializeField] private Rigidbody2D backTyreRB;
    [SerializeField] private Rigidbody2D carRB;

    [SerializeField] private float speed = 150f;
    [SerializeField] private float rotationSpeed = 300f;
    [SerializeField] private float maxAngularVelocity = 100f;

    [SerializeField] private LayerMask _ground_layer_mask;

    private float _input = 0;
    private float _distance_to_ground = 0;

    public void FixedUpdate()
    {
        Move();
        UpdateDistanceToGround();
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
    public void UpdateDistanceToGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, _ground_layer_mask);

        if (hit.collider != null)
            _distance_to_ground = transform.position.y - hit.point.y;
        else _distance_to_ground = Mathf.Infinity;
    }

    public void SetInput(float input)
    {
        _input = input;
    }
    public float DistanceToGround { get { return _distance_to_ground; } }
}
