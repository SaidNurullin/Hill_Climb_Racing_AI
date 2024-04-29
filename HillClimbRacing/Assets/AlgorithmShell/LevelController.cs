using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private EnvironmentGenerator _generator;
    [SerializeField] private float _min_noise_step = 0.5f;
    [SerializeField] private float _max_noice_step = 0.7f;
    [SerializeField] private Transform _start_point;
    [SerializeField] private float _road_part_radius = 3f;
    [SerializeField] private float _road_step = 0.2f;

    public void GenerateLevel()
    {
        _generator.RegenerateLevel(Random.Range(_min_noise_step, _max_noice_step));
    }

    public Vector3 GetStartPoint()
    {
        return _start_point.position;
    }
    public Vector3[] GetRoad(Vector3 car_position)
    {
        List<Vector3> road_part = _generator.GetRoadPart(car_position.x - _road_part_radius, car_position.x + _road_part_radius, _road_step);
        return road_part.ToArray();
    }
}
