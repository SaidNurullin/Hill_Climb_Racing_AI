using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualsController : MonoBehaviour
{
    [field: SerializeField]
    public AlgorithmShell AlgorithmShell { get; private set; }
    [SerializeField] private GameObject _individual_pref;

    private int _individuals_number => AlgorithmShell.Settings.IndividualsNumber;
    private Individual[] _individuals;

    public void CreateIndividuals(Vector3 start_point)
    {
        _individuals = new Individual[_individuals_number];
        for (int i = 0; i < _individuals_number; i++)
            _individuals[i] = CreateIndividual(start_point);
    }

    public void DestroyIndividuals()
    {
        foreach (var individual in _individuals)
            Destroy(individual.gameObject);
    }

    public IndividualData[] GetIndividualsAlgorithmData()
    {
        IndividualData[] individuals_data = new IndividualData[_individuals_number];
        for (int i = 0; i < _individuals_number; ++i)
            individuals_data[i] = GetIndividualAlgorithmData(_individuals[i]);

        return individuals_data;
    }
    public IndividualData[] GetIndividualsUIData()
    {
        IndividualData[] individuals_data = new IndividualData[_individuals_number];
        if (_individuals == null) return null;
        for (int i = 0; i < _individuals_number; ++i)
        {
            if (_individuals[i] == null) return null;
            individuals_data[i] = GetIndividualUIData(_individuals[i]);
        }
        return individuals_data;

    }

    public void ProcessIndividualsInputs(float[] inputs)
    {
        for (int i = 0; i < _individuals_number; ++i)
            _individuals[i].SetInput(inputs[i]);
    }

    private Individual CreateIndividual(Vector3 start_point)
    {
        GameObject individual_obj = Instantiate(_individual_pref) as GameObject;
        Individual individual = individual_obj.GetComponent<Individual>();
        individual.transform.position = start_point;

        return individual;
    }

    private IndividualData GetIndividualAlgorithmData(Individual individual)
    {
        return IndividualData.GetBuilder().
            SetIndividual(individual).
            SetRoad(AlgorithmShell.LevelController.GetRoad(individual.transform.position)).
            Build();
    }
    private IndividualData GetIndividualUIData(Individual individual)
    {
        return IndividualData.GetBuilder().
            SetIndividual(individual).
            Build();
    }

    public int IndividualsNumber { get { return _individuals_number; } }
    public Individual[] Individuals { get { return _individuals; } }
}
