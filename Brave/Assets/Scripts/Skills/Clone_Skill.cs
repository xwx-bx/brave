using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggressive clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneMultiplier;
    public bool canApplyOnHitEffect{ get; private set; }

    [Header("Multiple clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    
    [Header("CryStal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot cryStalInsteadCloneUnlockButton;
    public bool crystallInseadClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        cryStalInsteadCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCryStalInstead);
    }

    #region Unlock region

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockMultiClone();
        UnlockCryStalInstead();
    }

    private void UnlockCloneAttack()
    {
        if(cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCryStalInstead()
    {
        if (cryStalInsteadCloneUnlockButton.unlocked)
        {
            crystallInseadClone = true;
        }
    }

    #endregion

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystallInseadClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.transform.GetComponent<Clone_Skill_Controller>()
            .SetuopClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(_clonePosition), canDuplicateClone, chanceToDuplicate, player,attackMultiplier);
    }



    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCorotine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);

    }
}
