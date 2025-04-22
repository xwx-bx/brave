using UnityEngine;

public class shop : MonoBehaviour
{
    private bool isDoor;
    [SerializeField] private GameObject signText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isDoor && Time.timeScale == 1)
        {
            if(PlayerManager.instance.currency >= 1000)
            {
                PlayerManager.instance.currency -= 1000;
                PlayerManager.instance.iPlayerLevel += 1;
                PlayerManager.instance.player.fx.CreatePopUpText("�����ɹ�");
            }
            else
            {
                PlayerManager.instance.player.fx.CreatePopUpText("��Ҳ���");
            }
            
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
