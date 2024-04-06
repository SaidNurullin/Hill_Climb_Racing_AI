using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour
{
    [SerializeField] private DriveCar _drive_car;

    private float _start_point;
    private float _max_distance;
    private float _current_distance;
    private bool _is_alive;

    private void Awake()
    {
        _is_alive = true;
    }

    public void SetStartPoint(float start_point) { _start_point = start_point; }

    public float CurrentScore { get { return _current_distance - _start_point; } }
    public float MaxScore { get { return _max_distance - _start_point; } }
    public bool IsAlive { get { return _is_alive; } }
    public float DistanceToGround { get { return 0f; } }

    public void SetInput(float input)
    {
        if (!_is_alive) return;

        _drive_car.SetInput(input);
    }
}
