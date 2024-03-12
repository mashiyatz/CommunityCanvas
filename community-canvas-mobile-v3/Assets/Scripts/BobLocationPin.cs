using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobLocationPin : MonoBehaviour
{
    private float y;
    [SerializeField]
    private float bobHeight;
    [SerializeField]
    private float rotSpeed;

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = 1 + 0.1f * Mathf.Sin(Time.time);
        transform.position = pos;
        transform.Rotate(new Vector3(0, 0, rotSpeed * Time.deltaTime));
    }
}
