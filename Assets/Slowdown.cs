using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
        void OnTriggerStay2D(Collider2D other)
        {
            RubyController controller = other.GetComponent<RubyController>();

            if (controller != null)
            {
            controller.moveBack();
            controller.speed = 1.5f;
        }
        }
    public void OnCollisionEnter2D(Collision2D other)
    {
        RubyController controller = other.gameObject.GetComponent<RubyController>();
        if(controller != null)
        {
            controller.speed = 1.5f;
        }
        else
        {
            controller.speed = 3f;
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        RubyController controller = other.gameObject.GetComponent<RubyController>();
        if(controller != null)
        {
            controller.speed = 3;
        }
    }

}
