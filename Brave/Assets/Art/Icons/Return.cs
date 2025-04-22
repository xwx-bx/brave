using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Return : MonoBehaviour
{
    [SerializeField] private GameObject signText;

    
    private bool isPlayerInSign;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isPlayerInSign)
        {
            SaveManager.instance.SaveGame();
            Application.Quit();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            signText.SetActive(true);
            isPlayerInSign = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if (signText != null)
            {
                signText.SetActive(false);
            }
            isPlayerInSign = false;
        }
    }
}
