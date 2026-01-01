using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        exp,
        level,
        kill,
        score,
        health,
        time
    }

    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        if (GameManager.instance == null || GameManager.instance.Player == null) return;
        PlayerStatusInfo status = GameManager.instance.Player.StatusInfo;
        if (status == null) return;

        switch (type)
        {
            case InfoType.exp:
                float curExp = status.exp;
                int level = status.level;
                
                if (level < status.nextExp.Length)
                {
                    float nextExp = status.nextExp[level];
                    mySlider.value = nextExp > 0 ? curExp / nextExp : 0;
                }
                break;
            case InfoType.level:
                myText.text = string.Format("Lv. {0:F0}", status.level);
                break;
            case InfoType.kill:
                myText.text = string.Format(":{0:F0}",GameManager.instance.killCount);
                break;
            case InfoType.score:
                break;
            case InfoType.health:
                if (status != null)
                {
                    float curHp = status.health;
                    float maxHp = status.maxHealth;
                    mySlider.value = maxHp > 0 ? curHp / maxHp : 0;
                }
                break;
            case InfoType.time:
                // 남은 시간을 분과 초로 계산
                int min = Mathf.FloorToInt(GameManager.instance.timer / 60);
                int sec = Mathf.FloorToInt(GameManager.instance.timer % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            default:
                break;
        }
    }
}
