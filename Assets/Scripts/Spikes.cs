using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
   private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Movement>())
        {
            collision.collider.GetComponent<Movement>().Die();
        }
    }
}

