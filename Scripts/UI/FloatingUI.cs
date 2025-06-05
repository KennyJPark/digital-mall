using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FloatingUI : MonoBehaviour
{
    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.1f;
    public float frequency = 1.25f;


    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("FLOATING UI: " + transform.position + "\n" + "LOCAL: " + transform.localPosition);

        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position);
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
        //rectTransform.localPosition = new Vector3(xAmplitude * Mathf.Sin(Time.time * xspeed), yAmplitude * Mathf.Sin(Time.time * yspeed), 0);
    }

    void OnEnable()
    {
        posOffset = transform.position;
    }
}
