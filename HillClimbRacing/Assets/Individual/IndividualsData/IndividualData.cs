using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

[Serializable]
public class IndividualData
{
    public Vector2 Position = Vector2.zero;
    public float Rotation = 0.0f;
    public float DistanceToGround = 0.0f;
    public int IsAlive = 0;
    public float MaxScore = 0.0f;
    public float CurrentScore = 0.0f;
    public Vector2[] Road;

    public string GetJSON()
    {
        CultureInfo culture = CultureInfo.InvariantCulture;

        string roadJson = "[" + string.Join(", ", Road.Select(v => $@"{{""X"": ""{v.x.ToString("F3", culture)}"", ""Y"": ""{v.y.ToString("F3", culture)}""}}")) + "]";

        return $@"{{
            ""Position"": {{""X"": ""{Position.x.ToString("F3", culture)}"", ""Y"": ""{Position.y.ToString("F3", culture)}""}},
            ""Rotation"": ""{Rotation.ToString("F3", culture)}"",
            ""DistanceToGround"": ""{DistanceToGround.ToString("F3", culture)}"",
            ""IsAlive"": {IsAlive},
            ""MaxScore"": ""{MaxScore.ToString("F3", culture)}"",
            ""CurrentScore"": ""{CurrentScore.ToString("F3", culture)}"",
            ""Road"": {roadJson}
        }}";
    }
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
            _individual_data.Position = individual.transform.position;
            _individual_data.Rotation = individual.transform.eulerAngles.z;
            _individual_data.DistanceToGround = individual.Movement.DistanceToGround;
            _individual_data.IsAlive = individual.DamageTaker.IsAlive ? 1 : 0;
            _individual_data.MaxScore = individual.Score.MaxScore;
            _individual_data.CurrentScore = individual.Score.CurrentScore;
            return this;
        }
        public Builder SetRoad(Vector3[] road)
        {
            Vector2[] parsed_road = new Vector2[road.Length];
            for (int i = 0; i < road.Length; ++i)
                parsed_road[i] = road[i];
            _individual_data.Road = parsed_road;
            return this;
        }

        public IndividualData Build() { return _individual_data; }
    }
}

