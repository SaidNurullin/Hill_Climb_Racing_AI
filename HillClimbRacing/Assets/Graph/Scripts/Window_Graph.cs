
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private int cellsX = 10;
    [SerializeField] private IndividualsController _individualsController;
    [SerializeField] private IterationsController _iterationsController;
    [SerializeField] private TextMeshProUGUI _currentScore;
    [SerializeField] private TextMeshProUGUI _bestScore;
    [SerializeField] private LeaderboardController _leaderboardController;
    [SerializeField] private GameObject finishBlock;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private RectTransform dashes;
    private RectTransform connections;
    
    
    float graphHeight;
    float yMaximum = 1000f;
    float xSize = 50f;
    private int iteration = 1;
    private float currentScore = 0;
    private float bestScore = 0;

    private List<float> dots = new List<float>() {0};

    private void Start()
    {
        GenerateGraph();
        ShowGraph(dots, (int _i) => ""+(_i), (float _f) => "" + Mathf.RoundToInt(_f));
    }

    private void Update()
    {
        int controllerIteration = _iterationsController.GetIterationNumber();
        if (controllerIteration > _iterationsController.GetIterationsNumbers())
        {
            finishBlock.SetActive(true);
        }
        currentScore = Mathf.Max(_leaderboardController.getBestScore(), currentScore);
        bestScore = Mathf.Max(currentScore, bestScore);
        Debug.Log(currentScore);
        if (controllerIteration != iteration)
        {
            CreateNextDot();
            iteration = controllerIteration;
            currentScore = 0;
        }

        _currentScore.text = currentScore.ToString();
        _bestScore.text = bestScore.ToString();
    }

    public void GenerateGraph() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        dashes =  graphContainer.Find("Dashes").GetComponent<RectTransform>();
        connections =  graphContainer.Find("Connections").GetComponent<RectTransform>();
        graphHeight = graphContainer.sizeDelta.y;
        
        
        for (int i = 0; i < cellsX; i++) {
            float xPosition =  i * xSize;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -24f);
            labelX.GetComponent<TextMeshProUGUI>().text = i.ToString();
        }

        for (int i = 0; i < cellsX-1; i++) {
            float xPosition =  (i+1) * xSize;
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(dashes, false);
            dashX.SetAsFirstSibling();
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
        }
        
        int separatorCount = 10;
        
        for (int i = 0; i <= separatorCount; i++) {
            float normalizedValue = i * 1f / separatorCount;
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(dashes, false);
            dashY.SetAsFirstSibling();
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-24f, normalizedValue * graphHeight);
            labelY.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt((normalizedValue * yMaximum)).ToString();
        }

    }
    
    public void CreateNextDot()
    {
        if (dots.Count > 9)
        {
            dots.RemoveAt(0);
            ClearGraph();
        }
        dots.Add(0);
        UpdateGraph();
    }

    public void UpdateGraph()
    {

        dots[^1] = currentScore;
        ShowGraph(dots, (int _i) => ""+(_i), (float _f) => "" + Mathf.RoundToInt(_f));
    }

    private void ClearGraph()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "circle" || obj.name == "dotConnection" )
            {
                Destroy(obj);
            }
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<float> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }


        
        

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition =  i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

  
    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(connections, false);
        gameObject.transform.SetAsFirstSibling();
        gameObject.GetComponent<Image>().color = new Color(0.2f,0.59f,0.855f, 1f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

}
