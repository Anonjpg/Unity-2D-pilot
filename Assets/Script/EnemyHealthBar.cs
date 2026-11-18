using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBarFill;
    public Slider healthBarSlider;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBarSlider.value = currentHealth;
        healthBarSlider.maxValue = maxHealth;
    } 
}
