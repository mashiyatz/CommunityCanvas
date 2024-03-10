using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentParameters : MonoBehaviour
{
    [SerializeField]
    private int budget;

    public int GetBudget()
    {
        return budget;
    }
}
