using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement movement; // 인스펙터에서 할당하거나 자동으로 가져오도록 설정
    public Movement Movement => movement;

    void Awake()
    {
        if (movement == null) movement = GetComponent<Movement>();
    }
}
