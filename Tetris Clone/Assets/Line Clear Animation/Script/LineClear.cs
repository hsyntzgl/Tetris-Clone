using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineClear : MonoBehaviour
{
    private Animator animator;

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        DisableGameObject();
    }
    private void OnEnable()
    {
        animator.SetTrigger("StartAnimation");
        Invoke("DisableGameObject", 1f);
    }

    private void OnDisable()
    {
        animator.Rebind();
    }
}
