using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenFunctionality : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        animator.SetTrigger("start");
    }
}
