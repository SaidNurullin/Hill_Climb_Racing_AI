using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowFirstIndividual : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Individual[] individuals = FindObjectsOfType<Individual>();
        float maxX = 0f;

        foreach(Individual ind in individuals)
        {
            if(ind.transform.position.x > maxX)
            {
                maxX = ind.transform.position.x;
                GetComponent<CinemachineVirtualCamera>().Follow = ind.transform;
            }
        }
    }
}
