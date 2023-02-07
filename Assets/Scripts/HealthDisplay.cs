using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] string healthIcon = default;
    string healthIcons;
    Text healthBar;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        healthBar = GetComponent<Text>();
        if (FindObjectOfType<Player>())
        {
            healthIcons = string.Concat(System.Linq.Enumerable.Repeat(healthIcon, FindObjectOfType<Player>().GetHealth()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.text = healthIcons;
    }
    
    public void SubtractHealth()
    {
        healthIcons = healthIcons.Remove(healthIcons.Length - 1);
    }

    public void AddHealth()
    {
        if(player.GetHealth() <= 8)
        {
            healthIcons += healthIcon;
            player.AddHealth();
        }
    }

    public void LoseAllLives()
    {
        healthIcons = "";
    }
}
