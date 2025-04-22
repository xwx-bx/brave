using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    protected float cooldownTimer;

    protected Player player;
    protected UI_InGame uiInGame;

    protected virtual void Start()
    {
        uiInGame = GameObject.FindObjectOfType<UI_InGame>(true);

        player = PlayerManager.instance.player;

        Invoke("CheckUnlock", 0.1f); 
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
            player.fx.CreatePopUpText("¼¼ÄÜÀäÈ´");
        return false;
    }

    public virtual bool CanUseSkill2()
    {
        if (cooldownTimer < 0)
            return true;
        
        return false;
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }

    
}
