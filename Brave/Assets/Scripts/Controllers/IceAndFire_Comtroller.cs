using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Comtroller : ThunderStrike_Controller
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            AudioManager.instance.PlaySFX(15,this.transform);
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
