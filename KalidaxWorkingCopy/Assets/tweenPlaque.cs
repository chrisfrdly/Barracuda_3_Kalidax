using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tweenPlaque : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveLocalY(gameObject, 0, 1).setEaseOutBack();
    }

   
}
