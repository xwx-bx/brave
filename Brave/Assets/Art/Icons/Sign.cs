using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject signText;

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;

    [TextArea]
    [SerializeField] private string signTextContent;
    private bool isPlayerInSign;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && isPlayerInSign && Time.timeScale == 1)
        {
            dialogText.text = signTextContent;
            dialogBox.SetActive(true);
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

            if (dialogBox != null)
            {
                dialogBox.SetActive(false); 
            }
        }
    }
}
