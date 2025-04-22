using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulsDropAmount;

    [Header("Level details")]
    [SerializeField] private int level = 0;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = .4f;

    public void setLevel(int _level)
    {
        int z = _level;
        for (int i = 0; i < z; i++)
        {
            float modifier = strength.GetValue() * percantageModifier;
            strength.AddModifier(Mathf.RoundToInt(modifier));
            float agilityModifier = agility.GetValue() * percantageModifier;
            agility.AddModifier(Mathf.RoundToInt(agilityModifier));
            float intelligenceModifier = intelligence.GetValue() * percantageModifier;
            intelligence.AddModifier(Mathf.RoundToInt(intelligenceModifier));
            float vitalityModifier = vitality.GetValue() * percantageModifier;
            vitality.AddModifier(Mathf.RoundToInt(vitalityModifier));

            float damageModifier = damage.GetValue() * percantageModifier;
            damage.AddModifier(Mathf.RoundToInt(damageModifier));
            float critChanceModifier = critChance.GetValue() * percantageModifier;
            critChance.AddModifier(Mathf.RoundToInt(critChanceModifier));
            float critPowerModifier = critPower.GetValue() * percantageModifier;
            critPower.AddModifier(Mathf.RoundToInt(critPowerModifier));

            float maxHealthModifier = maxHealth.GetValue() * percantageModifier;
            maxHealth.AddModifier(Mathf.RoundToInt(maxHealthModifier));
            currentHealth = GetMaxHealthValue();
            float armorModifier = armor.GetValue() * percantageModifier;
            armor.AddModifier(Mathf.RoundToInt(armorModifier));
            float evasionModifier = evasion.GetValue() * percantageModifier;
            evasion.AddModifier(Mathf.RoundToInt(evasionModifier));
            float magicResistanceModifier = magicResistance.GetValue() * percantageModifier;
            magicResistance.AddModifier(Mathf.RoundToInt(magicResistanceModifier));

            float fireDamageModifier = fireDamage.GetValue() * percantageModifier;
            fireDamage.AddModifier(Mathf.RoundToInt(fireDamageModifier));
            float iceDamageModifier = iceDamage.GetValue() * percantageModifier;
            iceDamage.AddModifier(Mathf.RoundToInt(iceDamageModifier));
            float lightingDamageModifier = lightingDamage.GetValue() * percantageModifier;
            lightingDamage.AddModifier(Mathf.RoundToInt(lightingDamageModifier));

            float soulsDropAmountModifier = soulsDropAmount.GetValue() * percantageModifier;
            soulsDropAmount.AddModifier(Mathf.RoundToInt(soulsDropAmountModifier));
        }
        onHealthChanged();
    }
    protected override void Start()
    {
        ApplyLevelModifiers();
        soulsDropAmount.SetDefaultValue(100);

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 0; i < level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        myDropSystem.GenerateDrop();


        Destroy(gameObject,5f);
    }
}
