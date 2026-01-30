using UnityEngine;

public class Mask : MonoBehaviour
{
    public Player player;
    public float timer {  get; set; }
    public bool isOn;
    public int maskIndex;

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
        isOn = true;
    }
    public void UnWearMask()
    {
        isOn = false;
    }
}
