using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMove : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Tracks.instance.active)
        {
            transform.position += new Vector3(0.1f, 0, 0);
            if (transform.position.x > 13.2f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
