using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBoolAnimation : MonoBehaviour
{
    public string paramName;
    public bool boolValue;
    public float firstTime;
    public float repeatTime;

    public Animator animator;

    void Start()
    {
        animator.SetBool(paramName, boolValue);
        InvokeRepeating("SwitchAnim", firstTime, repeatTime);
    }

    void SwitchAnim()
    {
        boolValue = !boolValue;
        animator.SetBool(paramName, boolValue);
    }
}
