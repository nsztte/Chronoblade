using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHUD : MonoBehaviour
{
    public Slider hpBar;
    public Slider mpBar;
    public Slider staminaBar;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI goldText;

    public void UpdateHP(int current, int max)
    {
        hpBar.value = (float)current / max;
    }

    public void UpdateMP(int current, int max)
    {
        mpBar.value = (float)current / max;
    }

    public void UpdateStamina(int current, int max)
    {
        staminaBar.value = (float)current / max;
    }

    public void UpdateAmmo(int current, int total)
    {
        if(ammoText != null)
        {
            ammoText.text = current >= 0 ? $"{current}/{total}" : $"{total}";
        }
    }

    public void UpdateGold(int amount)
    {
        if(goldText != null)
        {
            goldText.text = $"{amount.ToString()} G";
        }
    }
}
