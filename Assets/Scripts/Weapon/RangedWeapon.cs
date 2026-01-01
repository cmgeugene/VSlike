using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Rendering;

public class RangedWeapon : BaseWeapon
{
    [Header("Ranged Weapon Settings")]
    [Tooltip("투사체 풀 이름")]
    public string projectilePoolName;

    [Tooltip("총알 시작 위치(기본값 플레이어 오브젝트 중심)")]
    public Transform firePoint;

    private Scanner scanner;
    private float timer;

    protected override void Awake()
    {
        base.Awake();
        scanner = GetComponentInParent<Scanner>();
    }

    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if(timer > fireRate)
        {
            Debug.Log($"Timer check passed. FireRate: {fireRate}");
            timer = 0f;
            Attack();
        }
    }

    protected override void Attack()
    {
        Debug.Log("ranged weapon attack called");
        // scanner 체크
        if(scanner == null || scanner.nearestTarget == null)
        {
            Debug.Log("scanner check failed");
            return;  
        } 

        // 투사체 가져오기
        GameObject projectileObj = ProjectileManager.instance.Get(projectilePoolName);

        // ProjectileManager 인스턴스 확인
        Debug.Log("Projectile Manager Instance checking");
        if(ProjectileManager.instance == null)
        {
            Debug.LogError("PM.instance is null!");

            var pm = FindFirstObjectByType<ProjectileManager>();
            if(pm != null)
            {
                Debug.LogWarning("Found PM, but instance was null. fixing...");
                ProjectileManager.instance = pm;
            }
            else
            {
                Debug.LogError("ProjectileManager not found in scene at all!");
                return;
            }
        }


        Debug.Log($"projectileObj 확인 : {projectileObj}");
        if(projectileObj != null)
        {
            projectileObj.transform.position = (firePoint != null) ? firePoint.position : transform.position; 

            Vector3 targetPos = scanner.nearestTarget.position;
            Vector2 direction = (targetPos - transform.position).normalized;

            Projectile projectile = projectileObj.GetComponent<Projectile>();
            if(projectile != null)
            {
                // count 를 관통수로 전달
                projectile.Init(damage, count, speed, direction);
                Debug.Log("Gun shot fired!");
            }
        }
    }

    protected override void ApplyStats(int level)
    {
        base.ApplyStats(level);
    }
}
