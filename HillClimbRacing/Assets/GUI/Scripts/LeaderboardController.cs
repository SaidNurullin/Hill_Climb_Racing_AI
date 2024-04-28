    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class LeaderboardController : MonoBehaviour
    {
        public TextMeshProUGUI[] individualIndexes;
        public TextMeshProUGUI[] individualScores;
        [SerializeField] IndividualsController individualsController;
        private IndividualData[] individualsData;
        public int topCount = 5;
        private int[] indexes;
        void Update()
        {
            individualsData = individualsController.GetIndividualsUIData();
            UpdateTopIndividualsUI();
        }

        public void UpdateTopIndividualsUI()
        {
            indexes = new int[individualsData.Length]; // Создаем массив исходных индексов

            // Заполняем массив исходных индексов
            for (int i = 0; i < individualsData.Length; i++)
            {
                indexes[i] = i;
            }
            
            SortIndividualsDataByScore();

            for (int i = 0; i < topCount; i++)
            {
                individualIndexes[i].text = "PLAYER: " + indexes[i];
                individualScores[i].text = "SCORE: " + Mathf.Round(individualsData[i].CurrentScore * Mathf.Pow(10, 4)) / Mathf.Pow(10, 4);
            }
        }

        public float getBestScore()
        {
            return individualsData[0].CurrentScore;
        }

        private void SortIndividualsDataByScore()
        {
            
            System.Array.Sort(indexes, (x, y) => individualsData[y].CurrentScore.CompareTo(individualsData[x].CurrentScore));
            System.Array.Sort(individualsData, (x, y) => y.CurrentScore.CompareTo(x.CurrentScore));
        }
    }
