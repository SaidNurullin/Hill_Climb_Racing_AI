using System;
using System.Linq;
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
    [NonSerialized] public UnityEvent OnFinishingAlgorithm = new UnityEvent();

    [SerializeField] private int _iterations_numbers = 10;
    [SerializeField] private float _interactions_number_per_second = 1;

    private int _iteration_number;
    private float _time;
    private float _time_between_sending_data;
    private bool _is_active_iteration;

    private void Awake()
    {
        _iteration_number = 0;
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
        if (_iteration_number >= _iterations_numbers)
        {
            OnFinishingAlgorithm.Invoke();
            return;
        }
        ++_iteration_number;

        AlgorithmShell.LevelController.GenerateLevel();
        Vector3 start_point = AlgorithmShell.LevelController.GetStartPoint();

        AlgorithmShell.Individuals.CreateIndividuals(start_point);

        CreatePopulation();
    }

    public void EndIteration()
    {
        _is_active_iteration = false;

        StartCoroutine(EvaluatePopulation());

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

        int deadIndividuals = 0;
        foreach(Individual ind in AlgorithmShell.Individuals.Individuals)
        {
            if (!ind.DamageTaker.IsAlive)
            {
                deadIndividuals++;
            }
        }
        if(deadIndividuals == AlgorithmShell.Individuals.IndividualsNumber)
        {
            EndIteration();
        }
    }

    public void InitializeAlgorithm()
    {
        RequestData request_data = RequestData.GetBuilder().
            SetCommand("Initialize algorithm").
            SetProcessFunction((string response) => StartNextIteration()).
            SetData("[]").Build();
        AlgorithmShell.ConnectingToNEAT.SendData(request_data);
    }
    public void CreatePopulation()
    {
        RequestData request_data = RequestData.GetBuilder().
            SetCommand("Create population").
            SetData("[]").
            SetProcessFunction((string data) =>
            {
                _is_active_iteration = true;
                _time = 0;
            }).Build();
        AlgorithmShell.ConnectingToNEAT.SendData(request_data);
    }
    public IEnumerator EvaluatePopulation()
    {
        yield return new WaitForSeconds(1);
        RequestData request_data = RequestData.GetBuilder().
            SetCommand("Evaluate population").
            SetData("[]").
            SetProcessFunction((string data) =>
            {
                Debug.Log("Start next iteration");
                StartNextIteration();
                Debug.Log("Start124214142 next iteration");
            }).Build();
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
        //Debug.Log(2 + " " + data);
        //Debug.Log($"Processing individuals commands: {data}");
        string[] innerArrays = data.Trim('[', ']').Split(new string[] { "], [" }, StringSplitOptions.None);

        bool[][] boolArray = innerArrays.Select(innerArray =>
                innerArray.Replace("[", "").Replace("]", "")
                    .Split(new string[] { ", " }, StringSplitOptions.None)
                    .Select(str => str == "true").ToArray())
            .ToArray();


        int n = 10;
        float[] individuals_inputs = new float[n];
        for (int i = 0; i < n; ++i)
        {
            float _gas = 0f;
            float _break = 0f;
            if (boolArray[i][0])
            {
                _gas = 1f;
            }
            else _gas = 0f;
            if (boolArray[i][1])
            {
                _break = -1f;
            }
            else _break = 0f;
            individuals_inputs[i] = _gas + _break;
        }

        foreach (float input in individuals_inputs)
        AlgorithmShell.Individuals.ProcessIndividualsInputs(individuals_inputs);
    }
}

