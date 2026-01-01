using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    private Player player;

    void Awake()
    {
        // 부모 오브젝트에서 Player 검색
        player = GetComponentInParent<Player>();
        if (hpSlider == null) hpSlider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        if (player != null && player.StatusInfo != null) player.StatusInfo.OnHealthChanged += UpdateHPUI;
    }

    void OnDisable()
    {
        if (player != null && player.StatusInfo != null) player.StatusInfo.OnHealthChanged -= UpdateHPUI;
    }

    private void UpdateHPUI(float currentHealth, float maxHealth)
    {
        if (hpSlider != null && maxHealth > 0)
            hpSlider.value = currentHealth / maxHealth;
    }
}