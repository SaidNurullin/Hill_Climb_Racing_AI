using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour
{
    [field: SerializeField]
    public MovementController Movement { get; private set; }
    [field: SerializeField]
    public ScoreController Score { get; private set; }
    [field: SerializeField]
    public DamageTaker DamageTaker { get; private set; }


    public void SetPreset(float start_point) 
    { 
        Score.SetStartPosition(start_point); 
    }

    public void SetInput(float input)
    {
        if (!DamageTaker.IsAlive)
        {
            Movement.SetInput(0);
            return;
        }

        Movement.SetInput(input);
    }

}
