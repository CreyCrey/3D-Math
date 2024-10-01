using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private float conveyorSpeed;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 pos = rb.position;
        rb.position += Vector3.back * conveyorSpeed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}
