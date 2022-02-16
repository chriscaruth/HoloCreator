using MRTK.Tutorials.MultiUserCapabilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    private float time = 0f;
    private float touchTimer = 1;
    // Start is called before the first frame update

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Menu")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
        }

    }
        private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NetA")
        {
            GameManager.Instance.UpdateScore(0, 1);
        }

        if (other.tag == "NetB")
        {
            GameManager.Instance.UpdateScore(1, 0);
        }

        RoomManager.Instance.ResetBall();
    }

}
