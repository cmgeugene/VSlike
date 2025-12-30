using System;
using UnityEngine;

public class RepositionTileMap : MonoBehaviour
{
    [SerializeField] string triggerTag = "Area"; // Inspector로 바꿀 수 있게

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(triggerTag))
        {
            Debug.Log($"OnTriggerExit2D ignored tag:{collision.tag}");
            return;
        }
        Debug.Log($"OnTriggerExit2D fired by {collision.name} ({collision.tag})");

        if (GameManager.instance == null || GameManager.instance.Player == null || GameManager.instance.Player.Movement == null) return;

        Vector3 playerPosition = GameManager.instance.Player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = MathF.Abs(playerPosition.x - myPos.x);
        float diffY = MathF.Abs(playerPosition.y - myPos.y);

        Vector2 playerDir = GameManager.instance.Player.Movement.inputVector;
        if (playerDir == Vector2.zero)
        {
            // 입력이 없으면 플레이어의 상대 위치로 방향 결정
            playerDir = new Vector2(MathF.Sign(playerPosition.x - myPos.x), MathF.Sign(playerPosition.y - myPos.y));
        }

        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                break;
            default:
                break;
        }

    }
}
