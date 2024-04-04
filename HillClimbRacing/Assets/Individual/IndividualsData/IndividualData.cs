using System;
using UnityEngine;

[Serializable]
public class IndividualData
{
    public SerializedVector2 Position;
    public float Rotation;
    public float DistanceToGround;
    public int IsAlive;
    public float MaxScore;
    public float CurrentScore;
    public SerializedVector2[] Road;

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
            _individual_data.Rotation = individual.transform.eulerAngles.z;
            _individual_data.DistanceToGround = individual.DistanceToGround;
            _individual_data.IsAlive = individual.IsAlive ? 1 : 0;
            _individual_data.MaxScore = individual.MaxScore;
            _individual_data.CurrentScore = individual.CurrentScore;
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

