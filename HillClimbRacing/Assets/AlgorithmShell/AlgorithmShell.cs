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

    private void Start()
    {
        InitializeAlgorithmShell();
        IterationsController.StartNextIteration();
    }

    public void InitializeAlgorithmShell()
    {
        ConnectingToNEAT.CreateConnection();
    }
}
