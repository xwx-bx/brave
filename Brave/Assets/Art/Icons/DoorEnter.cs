using UnityEditor.Rendering;
using UnityEngine;

public class DoorEnter : MonoBehaviour
{
    [SerializeField] private Transform backDoor;

    private bool isDoor;
    private Transform playerTransform;
    [SerializeField] private GameObject signText;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isDoor && Time.timeScale == 1)
        {
           playerTransform.position = backDoor.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")
           && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            signText.SetActive(true);
            isDoor = true;
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
            isDoor = false;
        }
    }
}
