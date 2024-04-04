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
    public void CreatePopulation()
    {
        AlgorithmShell.ConnectingToNEAT.SendData("Create population");
    }
    public void EvaluatePopulation()
    {
        AlgorithmShell.ConnectingToNEAT.SendData("Evaluate population");
    }
    public void SendIndividualsData()
    {
        string data = JsonUtility.ToJson(AlgorithmShell.Individuals.GetIndividualsData()[0]);
        AlgorithmShell.ConnectingToNEAT.SendData(data, ProcessIndividualsCommand);
    }
    private void ProcessIndividualsCommand(string data)
    {
        Debug.Log($"Processing individuals commands: {data}");
    }
}
