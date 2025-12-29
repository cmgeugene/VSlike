using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // 버그 방지를 위해 직접 InputAction을 가지기 보다 참조값을 갖도록
    [SerializeField] InputActionReference wasdRef;
    [SerializeField] float movementSpeed = 5f;

    // 내부 상태로 현재 입력을 보관하고 외부에서 읽기만 가능하게 함
    public Vector2 inputVector { get; private set; }
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anmt;

    void OnEnable()
    {
        if (wasdRef == null || wasdRef.action == null)
        {
            Debug.Log("액션 할당해주세요");
        }
        else if (!wasdRef.action.enabled)
        {
            wasdRef.action.Enable();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anmt = GetComponent<Animator>();
    }

    void OnDisable()
    {
        if (wasdRef != null && wasdRef.action != null && wasdRef.action.enabled)
        {
            wasdRef.action.Disable();
        }
    }

    void FixedUpdate()
    {
        Move();    
    }

    void LateUpdate()
    {
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        if (inputVector.x != 0)
        {
            sr.flipX = inputVector.x < 0;
        }
        // 벡터의 크기만 있으면 되기 때문에 magnitude 사용
        anmt.SetFloat("velocity", inputVector.magnitude);
    }

    void Move()
    {
        if (wasdRef == null || wasdRef.action == null) return;

        Vector2 rawInput = wasdRef.action.ReadValue<Vector2>();
        rawInput = Vector2.ClampMagnitude(rawInput, 1f);

        // 상태 갱신
        inputVector = rawInput;

        if (rawInput != Vector2.zero)
        {
            //Debug.Log("현재 입력: " + rawInput);
            rb.MovePosition(rb.position + rawInput * movementSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        // optionally update read-only property via a controlled setter method if needed
    }
}
