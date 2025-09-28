using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool AlreadyPassed;
    private PlayerStatus playerStatus;
    [SerializeField] private int GetCoins = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();    
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!AlreadyPassed)
            {
                // my_source.PlayOneShot(my_source.clip);
                playerStatus.CoinSet(GetCoins);
                Destroy(this.gameObject);
                AlreadyPassed = true;
            }

        }
    }
}
