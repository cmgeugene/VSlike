using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Slot")]
    [Tooltip("무기가 생성될 위치입니다. 플레이어의 자식 오브젝트로 설정합니다.")]
    public Transform weaponSlot; // ????? (무기가 부착될 Transform)

    [Header("Initial Weapons")]
    [Tooltip("게임 시작 시 장착할 초기 무기 프리팹 목록입니다.")]
    public List<GameObject> initialWeaponPrefabs; // ????? (초기 무기 프리팹 리스트)

    // 현재 활성화된 무기 인스턴스들을 관리하는 리스트
    private List<BaseWeapon> activeWeapons; // ????? (활성화된 무기 인스턴스들을 저장할 리스트)

    void Start()
    {
        // 1. activeWeapons 리스트를 초기화합니다.
        activeWeapons = new List<BaseWeapon>();

        // 2. weaponSlot이 Inspector에서 할당되지 않았다면,
        //    'Weapon Slot'이라는 이름의 자식 오브젝트를 찾거나 새로 생성하여 할당합니다.
        if (weaponSlot == null)
        {
            var slot = transform.Find("Weapon Slot");
            if(slot == null)
            {

                slot = new GameObject("Weapon Slot").transform;
                slot.SetParent(transform);
                slot.localPosition = Vector3.zero;
            }
            weaponSlot = slot;
        }

        // 3. initialWeaponPrefabs 리스트에 있는 모든 초기 무기를 AddWeapon 메서드를 사용하여 장착합니다.
        foreach (var prefab in initialWeaponPrefabs)
        {
            AddWeapon(prefab);
        }
    }

    /// <summary>
    /// 지정된 무기 프리팹을 새로 장착합니다.
    /// </summary>
    /// <param name="weaponPrefab">장착할 무기 프리팹</param>
    public void AddWeapon(GameObject weaponPrefab)
    {
        // 1. weaponPrefab이 null인지 확인하여 오류를 방지합니다.
        if (weaponPrefab == null)
        {
            Debug.LogError("Weapon Prefab이 null입니다.");
            return;
        }

        // 2. weaponSlot을 부모로 하여 무기 프리팹을 인스턴스화(Instantiate)합니다.
        GameObject weaponInstance = Instantiate(weaponPrefab, weaponSlot);
        
        // 3. 생성된 무기 인스턴스에서 BaseWeapon 컴포넌트를 가져옵니다.
        BaseWeapon weapon = weaponInstance.GetComponent<BaseWeapon>();
        if (weapon != null)
        {
            // 4. 활성 무기 리스트에 추가합니다.

            activeWeapons.Add(weapon); // 수정: 이전 턴에서 여기를 실수로 채워버림
            Debug.Log($"{weapon.name} 무기가 장착되었습니다.");
        }
        else
        {
            Debug.LogError($"{weaponPrefab.name} 프리팹에 BaseWeapon을 상속받는 컴포넌트가 없습니다.");
            Destroy(weaponInstance); // 잘못된 프리팹이면 파괴
        }
    }

    // 추가적으로 무기 레벨업 또는 다른 무기 관련 로직을 위한 메서드를 여기에 구현할 수 있습니다.
    // 예: public void LevelUpWeapon(int weaponId) { ... }
}