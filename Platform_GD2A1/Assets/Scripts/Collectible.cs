using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private PlayerScript player;
    public int direction = 1;
    private float height= 2;
    private float timePassed = 0;
    void Update()
    {
        Vector2 position =  transform.position;
        position.y += (height * Time.deltaTime * direction);
        transform.position = position; 

        timePassed += Time.deltaTime;
        if (timePassed > 2)
        {
            timePassed = 0;
            direction *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.tag=="Collectible" && collision.gameObject.tag == "Player")
        {
            player.AddCollectible();
            Destroy(this.gameObject);
        }
    }
}
