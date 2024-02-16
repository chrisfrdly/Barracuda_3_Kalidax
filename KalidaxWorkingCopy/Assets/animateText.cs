using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class animateText : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float duration;
    TextMeshProUGUI animText;
    float alpha = 1;
    private void Awake()
    {
        animText = GetComponent<TextMeshProUGUI>(); 
    }

    private void Start()
    {
        Invoke("DestroyText", duration);
    }

    void Update()
    {
        animText.color = new Color(animText.color.r, animText.color.g, animText.color.b, alpha);
        alpha -= speed * Time.deltaTime;

        transform.Translate(Vector2.up * speed * Time.deltaTime);

    }

    void DestroyText()
    {
        Destroy(gameObject);
    }
}
