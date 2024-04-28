using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmShell : MonoBehaviour
{
    [field: SerializeField]
    public IterationsController IterationsController { get; private set; }
    [field: SerializeField]
    public ConnectingToNEAT ConnectingToNEAT { get; private set; }
    [field: SerializeField]
    public IndividualsController Individuals { get; private set; }
    [field: SerializeField]
    public LevelController LevelController { get; private set; }

    [SerializeField] private AlgorithmSettings _presettings = new AlgorithmSettings();

    public AlgorithmSettings Settings { get; private set; } = new AlgorithmSettings();

    private void Awake()
    {
        SetPresettings();
        ConnectingToNEAT.OnCreatingConnection.AddListener(StartAlgorithm);
    }

    public void SetupAlgorithm()
    {
        InitializeAlgorithmShell();
    }

    public void InitializeAlgorithmShell()
    {
        ConnectingToNEAT.CreateConnection();
    }
    private void StartAlgorithm()
    {
        IterationsController.InitializeAlgorithm();
    } 

    public void SetPresettings()
    {
        Settings.SetSettings(_presettings);
    }

    public void setSettingIndividualsNumber(string value)
    {
        Settings.SetIndividualsNumber(value);
    }
    public void setSettingIterationsNumber(string value)
    {
        Settings.SetIterationsNumber(value);
    }
    public void setSettingIterationDuration(string value)
    {
        Settings.SetIterationDuration(value);
    }
}

[System.Serializable]
public class AlgorithmSettings
{
    public int IndividualsNumber = 10;
    public int IterationsNumber = 10;
    public float IterationDuration = 30f;

    public AlgorithmSettings() { }
    public AlgorithmSettings(AlgorithmSettings settings)
    {
        SetSettings(settings);
    }
    public AlgorithmSettings(int individuals_number, int iterations_number, float iteration_duration)
    {
        IndividualsNumber = individuals_number;
        IterationsNumber = iterations_number;
        IterationDuration = iteration_duration;
    }
    public void SetSettings(AlgorithmSettings settings)
    {
        IndividualsNumber = settings.IndividualsNumber;
        IterationsNumber = settings.IterationsNumber;
        IterationDuration = settings.IterationDuration;

    }

    public void SetIndividualsNumber(string value)
    {
        IndividualsNumber = int.Parse(value);
    }
    public void SetIterationsNumber(string value)
    {
        IterationsNumber = int.Parse(value);
    }
    public void SetIterationDuration(string value)
    {
        IterationDuration = int.Parse(value);
    }
}