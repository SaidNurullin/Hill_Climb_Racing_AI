using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    [SerializeField] private GameObject Graph;
    [SerializeField] private GameObject Leaderboard;
    [SerializeField] private AlgorithmShell _algorithmShell;

    public void Start()
    {
        Graph.SetActive(false);
        Leaderboard.SetActive(false);
    }

    public void StartWork()
    {
        _algorithmShell.SetupAlgorithm();
        Graph.SetActive(true);
        Leaderboard.SetActive(true);
        gameObject.SetActive(false);
    }
}
