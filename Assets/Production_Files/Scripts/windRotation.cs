using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windRotation : MonoBehaviour
{
    public int maxRange = 60, minRange = -60;
    public float rotationSpeed;
    public float currentMaxRange = 0, currentMinRange = 0;
    public bool up = false;

    private void Start()
    {
        int temp = Random.Range(0, 2);
        if (temp == 0)
            up = true;
        chooseRange();
    }

    // Update is called once per frame
    void Update()
    {
        if (up) 
            transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        else
            transform.Rotate(-1 * Vector3.up * Time.deltaTime * rotationSpeed);

        if (up && UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y > currentMaxRange)
        {
            up = !up;
            chooseRange();
        }
        else if(!up && UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y < currentMinRange)
        {
            up = !up;
            chooseRange();
        }
    }

    void chooseRange() //This chooses a range for the wind object to rotate to
    {
        float max = Random.value, min = Random.value;
        currentMaxRange = max * maxRange;
        currentMinRange = min * minRange;
    }
}
