using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{ 
    private float _start_point;
    private float _max_distance;
    private float _current_distance;

    private void FixedUpdate()
    {
        _current_distance = transform.position.y;
        if (_current_distance > _max_distance)
            _max_distance = _current_distance;
    }
    public void SetStartPosition(float start_point)
    {
        _start_point = start_point;
        _max_distance = start_point;
        _current_distance = start_point;
    }

    public float CurrentScore { get { return _current_distance - _start_point; } }
    public float MaxScore { get { return _max_distance - _start_point; } }
}
