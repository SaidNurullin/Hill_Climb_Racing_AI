using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;
    [SerializeField, Range(3, 10000)] private int levelLength = 50;
    [SerializeField, Range(1f, 50f)] private float xMultiplyer = 2f;
    [SerializeField, Range(1f, 50f)] private float yMultiplyer = 2f;
    [SerializeField, Range(0f, 1f)] private float curveSmoothness = 0.5f;
    [SerializeField] private float noiseStep = 0.5f;
    [SerializeField] private float bottom = 10f;

    private Vector3 lastPosition;

    public void OnValidate()
    {
        spriteShapeController.spline.Clear();

        for (int i = 0; i < levelLength; i++)
        {
            yMultiplyer += 0.01f;
            lastPosition = transform.position + new Vector3(i * xMultiplyer, Mathf.PerlinNoise(0, i * noiseStep) * yMultiplyer);
            spriteShapeController.spline.InsertPointAt(i, lastPosition);

            if(i != 0 && i != levelLength - 1)
            {
                spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetLeftTangent(i, Vector3.left * xMultiplyer * curveSmoothness);
                spriteShapeController.spline.SetRightTangent(i, Vector3.right * xMultiplyer * curveSmoothness);
            }
        }

        spriteShapeController.spline.InsertPointAt(levelLength, new Vector3(lastPosition.x, transform.position.y - bottom));
        spriteShapeController.spline.InsertPointAt(levelLength + 1, new Vector3(transform.position.x, transform.position.y - bottom));
    }
}
