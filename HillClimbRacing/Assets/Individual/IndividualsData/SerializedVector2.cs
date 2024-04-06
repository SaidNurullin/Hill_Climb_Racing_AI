using System;
using UnityEngine;

[Serializable]
public class SerializedVector2
{
    public float X, Y;

    public SerializedVector2(float x, float y)
    {
        X = x;
        Y = y;
    }
    public SerializedVector2(Vector2 vector) : this(vector.x, vector.y) { }
    public SerializedVector2(Vector3 vector) : this(vector.x, vector.y) { }
    public static SerializedVector2[] Parse(Vector3[] points)
    {
        SerializedVector2[] parsed_points = new SerializedVector2[points.Length];
        for (int i = 0; i < points.Length; ++i)
            parsed_points[i] = new SerializedVector2(points[i]);
        return parsed_points;
    }
}
