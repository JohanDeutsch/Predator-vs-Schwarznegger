using System;
using System.Security.Cryptography;
using UnityEngine;

public class SchwarzPhysik : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y <= -6.2f)
        {
            Destroy(gameObject);
        }
        transform.position -= new Vector3(0, GameVariables.current_fall_speed * Time.deltaTime, 0);
    }
}