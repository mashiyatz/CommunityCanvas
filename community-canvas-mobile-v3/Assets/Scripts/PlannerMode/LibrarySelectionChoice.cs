using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibrarySelectionChoice : MonoBehaviour
{
    private Button button;
    private PlannerMain main;
    public GameObject modelPrefab;

    private void Awake()
    {
        main = GameObject.Find("Main").GetComponent<PlannerMain>();
    }

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        main.ChangeState(1);
        main.StartWaitForPlacement(modelPrefab);
    }
}
