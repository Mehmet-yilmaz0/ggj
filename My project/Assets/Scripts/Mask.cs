using UnityEngine;

public class Mask : MonoBehaviour
{
    public Player player;
    public float timer {  get; set; }
    public bool isOn;
    public int maskIndex;
    public float rangeBonus;
    public float attackSpeedBonus;
    public float attackBonus;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (!player.masks.Contains(this)) player.masks[maskIndex] = this;
            WearMask();
        }
    }
    public void WearMask()
    {
        player.wearedMask = this;
        player.attackDamage += attackBonus;
        player.attackRange += rangeBonus;
        player.attackSpeed += attackSpeedBonus;
        isOn = true;
    }
    public void UnWearMask()
    {
        player.attackDamage += attackBonus;
        player.attackRange += rangeBonus;
        player.attackSpeed += attackSpeedBonus;
        isOn = false;
    }
}
