using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is not found on " + gameObject.name);
        }
    }

    public void FadeOut() => anim.SetTrigger("fadeOut");
    public void FadeIn() => anim.SetTrigger("fadeIn");
}
