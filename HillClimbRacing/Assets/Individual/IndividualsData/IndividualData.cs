using System;
using UnityEngine;

[Serializable]
public class IndividualData
{
    public SerializedVector2 Position = new SerializedVector2();
    public float Rotation = 0.0f;
    public float DistanceToGround = 0.0f;
    public int IsAlive = 0;
    public float MaxScore = 0.0f;
    public float CurrentScore = 0.0f;
    public SerializedVector2[] Road = null;

    public static Builder GetBuilder() { return new Builder(); }
    public class Builder
    {
        private IndividualData _individual_data;
        public Builder()
        {
            _individual_data = new IndividualData();
        }

        public Builder SetIndividual(Individual individual)
        {
            _individual_data.Position = new SerializedVector2(individual.transform.position);
            _individual_data.Rotation = (float)Math.Round(individual.transform.eulerAngles.z, 3);
            _individual_data.DistanceToGround = (float)Math.Round(individual.Movement.DistanceToGround);
            _individual_data.IsAlive = individual.DamageTaker.IsAlive ? 1 : 0;
            _individual_data.MaxScore = (float)Math.Round(individual.Score.MaxScore);
            _individual_data.CurrentScore = (float)Math.Round(individual.Score.CurrentScore);
            return this;
        }
        public Builder SetRoad(SerializedVector2[] road)
        {
            _individual_data.Road = road;
            return this;
        }

        public IndividualData Build() { return _individual_data; }
    }


    public void DebugLOGGG()
    {
        Debug.Log($"{Position.X} {Position.Y} {Rotation} {DistanceToGround} {IsAlive} {MaxScore} {CurrentScore}");
    }
}

