using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MeleeWeapon : BaseWeapon
{
    [Header("Weapon Instance")]
    [Tooltip("실제 생성될 무기 오브젝트의 프리팹입니다.")]
    public GameObject weaponInstancePrefab;

    private List<GameObject> spawnedInstances = new List<GameObject>();

    // Awake()나 Start() 대신 ApplyStats()에서 레벨에 맞게 생성/재배치합니다.
    // BaseWeapon의 LevelUp()이 ApplyStats()를 호출하기 때문입니다.
    protected override void ApplyStats(int level)
    {
        base.ApplyStats(level); // damage, count, range, speed 등 능력치를 SO에서 불러옵니다.
        RepositionWeaponInstances();
    }

    protected override void Update()
    {
        base.Update();
        // speed 값에 따라 컨트롤러(자기 자신)를 회전시킵니다.
        // 자식으로 붙어있는 weaponInstance들이 함께 회전하며 공전 효과를 만듭니다.
        if (speed > 0)
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }
    }

    private void RepositionWeaponInstances()
    {
        // 프리팹이 할당되지 않았다면 오류를 출력하고 종료합니다.
        if (weaponInstancePrefab == null)
        {
            Debug.LogError("weaponInstancePrefab이 할당되지 않았습니다!");
            return;
        }

        // 기존에 생성된 인스턴스들을 모두 파괴합니다.
        foreach (var instance in spawnedInstances)
        {
            Destroy(instance);
        }
        spawnedInstances.Clear();

        // 'count' 개수만큼 인스턴스를 원형으로 배치합니다.
        for (int i = 0; i < count; i++)
        {
            float angle = i * 360f / count;
            
            // 'range' 값을 사용하여 컨트롤러로부터의 거리를 조절합니다.
            Vector3 rotVec = Vector3.forward * angle;
            Quaternion rot = Quaternion.Euler(rotVec);
            Vector3 position = rot * Vector3.up * range;
            
            
            // weaponInstancePrefab을 컨트롤러의 자식으로 생성합니다.
            GameObject instance = Instantiate(weaponInstancePrefab, transform);
            instance.transform.localPosition = position;
            instance.transform.localRotation = rot;
            
            // 생성된 인스턴스에 데미지 값을 설정해줍니다.
            MeleeWeaponInstance instanceScript = instance.GetComponent<MeleeWeaponInstance>();
            if (instanceScript != null)
            {
                instanceScript.damage = this.damage;
            }

            spawnedInstances.Add(instance);
        }
    }

    protected override void Attack()
    {
        Debug.Log("공격!");
    }
}
