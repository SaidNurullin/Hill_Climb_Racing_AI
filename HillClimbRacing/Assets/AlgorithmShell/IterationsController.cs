using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IterationsController : MonoBehaviour
{
    [field: SerializeField]
    public AlgorithmShell AlgorithmShell { get; private set; }

    [NonSerialized] public UnityEvent OnStartingIteration = new UnityEvent();
    [NonSerialized] public UnityEvent OnEndingIteration = new UnityEvent();

    [SerializeField] private int _iterations_number = 10;
    [SerializeField] private float _interactions_number_per_second = 1;
    private float _time;
    private float _time_between_sending_data;
    private bool _is_active_iteration;
    private void Awake()
    {
        _time = 0;
        _time_between_sending_data = 1 / _interactions_number_per_second;
        _is_active_iteration = false;
    }

    public void StartFirstIteration()
    {
        InitializeAlgorithm();
    }

    public void StartNextIteration()
    {
        AlgorithmShell.LevelController.GenerateLevel();
        Vector3 start_point = AlgorithmShell.LevelController.GetStartPoint();

        AlgorithmShell.Individuals.CreateIndividuals(start_point);

        CreatePopulation();

        _is_active_iteration = true;
        _time = 0;
    }

    public void EndIteration()
    {
        _is_active_iteration = false;

        EvaluatePopulation();

        AlgorithmShell.Individuals.DestroyIndividuals();
    }


    private void Update()
    {
        if (!_is_active_iteration)
            return;

        _time += Time.deltaTime;
        if (_time >= _time_between_sending_data)
        {
            _time -= _time_between_sending_data;
            SendIndividualsData();
        }
    }

    public void InitializeAlgorithm()
    {
        RequestData request_data = RequestData.GetBuilder().
            SetCommand("Initialize algorithm").
            SetProcessFunction((string response) => StartNextIteration()).Build();
        AlgorithmShell.ConnectingToNEAT.SendData(request_data);
    }
    public void CreatePopulation()
    {
        RequestData request_data = RequestData.GetBuilder().
            SetCommand("Create population").Build();
        AlgorithmShell.ConnectingToNEAT.SendData(request_data);
    }
    public void EvaluatePopulation()
    {
        RequestData request_data = RequestData.GetBuilder().
            SetCommand("Evaluate population").Build();
        AlgorithmShell.ConnectingToNEAT.SendData(request_data);
    }
    public void SendIndividualsData()
    {

        List<string> jsonStrings = new List<string>();
        foreach (var individualData in AlgorithmShell.Individuals.GetIndividualsAlgorithmData())
        {
            string jsonString = JsonUtility.ToJson(individualData);
            jsonStrings.Add(jsonString);
        }
        string data = "[" + string.Join(",", jsonStrings) + "]";
        RequestData request_data = RequestData.GetBuilder().
            SetCommand("Process individuals data").
            SetData(data).
            SetProcessFunction(ProcessIndividualsCommand).Build();
        AlgorithmShell.ConnectingToNEAT.SendData(request_data);
    }
    private void ProcessIndividualsCommand(string data)
    {
        Debug.Log($"Processing individuals commands: {data}");
        int n = 30;
        float[] individuals_inputs = new float[n];
        for (int i = 0; i < n; ++i)
            individuals_inputs[i] = UnityEngine.Random.Range(-1, 2);

        foreach (float input in individuals_inputs)
            Debug.Log(input);
        AlgorithmShell.Individuals.ProcessIndividualsInputs(individuals_inputs);
    }
}
