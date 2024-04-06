using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform _start_point;
    public void GenerateLevel()
    {

    }

    public Vector3 GetStartPoint()
    {
        return _start_point.position;
    }
    public Vector3[] GetRoad(Vector3 car_position, int points_number)
    {
        Vector3[] road = new Vector3[points_number];

        return road;
    }
}
