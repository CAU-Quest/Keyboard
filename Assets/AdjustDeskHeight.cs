using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustDeskHeight : MonoBehaviour
{
    public float height;
    private float offset = 0.01f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    public void Up()
    {
        transform.position += new Vector3(0, offset, 0);
    }

        public void Down()
    {
        transform.position += new Vector3(0, -offset, 0);
    }
}
