using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;
    [SerializeField, Range(3, 10000)] private int levelLength = 50;
    [SerializeField, Range(0.1f, 50f)] private float xMultiplyer = 2f;
    [SerializeField, Range(0.1f, 50f)] private float yMultiplyerInitial = 2f;
    [SerializeField, Range(0f, 0.5f)] private float yMultiplyerDelta = 0.01f;
    [SerializeField, Range(0f, 1f)] private float curveSmoothness = 0.5f;
    [SerializeField] private float noiseStep = 0.5f;
    [SerializeField] private float bottom = 10f;
    [SerializeField] private LayerMask roadLayers;

    private Vector3 lastPosition;
    private float yMultiplyerCurrent;
    private List<Vector3> road;

    public void OnValidate()
    {
        GenerateLevel();
    }
    private void GenerateLevel()
    {
        spriteShapeController.spline.Clear();
        yMultiplyerCurrent = yMultiplyerInitial;
        road = new List<Vector3>();

        for (int i = 0; i < levelLength; ++i)
        {
            yMultiplyerCurrent += yMultiplyerDelta;
            lastPosition = transform.position + new Vector3(i * xMultiplyer, Mathf.PerlinNoise(0, i * noiseStep) * yMultiplyerCurrent);
            spriteShapeController.spline.InsertPointAt(i, lastPosition);
            road.Add(lastPosition);

            if (i != 0 && i != levelLength - 1)
            {
                spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetLeftTangent(i, Vector3.left * xMultiplyer * curveSmoothness);
                spriteShapeController.spline.SetRightTangent(i, Vector3.right * xMultiplyer * curveSmoothness);
            }
        }

        spriteShapeController.spline.InsertPointAt(levelLength, new Vector3(lastPosition.x, transform.position.y - bottom));
        spriteShapeController.spline.InsertPointAt(levelLength + 1, new Vector3(transform.position.x, transform.position.y - bottom));
        road.Add(new Vector3(lastPosition.x, transform.position.y - bottom));
        road.Add(new Vector3(transform.position.x, transform.position.y - bottom));
    }

    public List<Vector3> GetRoadPart(float start, float finish)
    {
        List<Vector3> roadPart = new List<Vector3>();
        foreach (Vector3 point in road)
        {
            if (point.x >= start)
            {
                if (point.x < finish)
                    roadPart.Add(point);
                else break;
            }
        }

        return roadPart;
    }
    public List<Vector3> GetRoadPart(float start, float finish, float step)
    {
        List<Vector3> roadPart = new List<Vector3>();
        for (float x_pos = start; x_pos < finish; x_pos += step)
            if (GetPoint(x_pos, out Vector3 pos))
                roadPart.Add(pos);
        return roadPart;
    }
    private bool GetPoint(float x, out Vector3 pos)
    {
        pos = Vector3.zero;
        int MAX_Y = 1000;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, MAX_Y), Vector2.down, Mathf.Infinity, roadLayers);
        if (hit.collider != null)
        {
            pos = hit.point;
            return true;
        }

        return false;
    }

    public void RegenerateLevel(float noise)
    {
        noiseStep = noise;
        GenerateLevel();
    }
}
