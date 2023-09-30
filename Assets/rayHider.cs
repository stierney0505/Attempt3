using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using static Unity.VisualScripting.AnnotationUtility;

public class rayHider : MonoBehaviour
{
    public GameObject ray;
    Component[] components;
    public PrimaryButtonWatcher watcher;
    public bool IsPressed = false;

    // Start is called before the first frame update
    void Start()
    {
       watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
       components = ray.GetComponents<MonoBehaviour>();
    }

    public void onPrimaryButtonEvent(bool pressed)
    {
        IsPressed = pressed;
        foreach (MonoBehaviour c in components) 
        {
            c.enabled = pressed;
        }
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
