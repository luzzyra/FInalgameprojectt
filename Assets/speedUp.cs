using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedUp : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {

            controller.ChangeCogs(4);
            controller.speed = 6f;
            Destroy(gameObject);

            controller.PlaySound(collectedClip);

        }

    }
}