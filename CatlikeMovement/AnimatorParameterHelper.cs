using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorParameterHelper : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetBool(string parameterName, bool value)
    {
        animator.SetBool(parameterName, value);
    }

    public void SetBoolTrue(string parameterName)
    {
        SetBool(parameterName, true);
    }
    public void SetBoolFalse(string parameterName)
    {
        SetBool(parameterName, false);
    }
    public void SetBoolToggle(string parameterName)
    {
        SetBool(parameterName, !animator.GetBool(parameterName));
    }
}
