using UnityEngine;

public class Money : MonoBehaviour {

    [SerializeField]
    private int amount = 10000;
    [SerializeField]
    private float delay = .3f;
    [SerializeField]
	private bool ignorePlayer = false;
	public bool playerCreatedMoney = false;

    private float rotationSpeed;

    private void Awake()
    {
        IgnorePlayer = false;
        rotationSpeed = Random.Range(30f, 60f);
    }

    private void Update()
    {
        if(!playerCreatedMoney)
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null && !IgnorePlayer)
        {
            player.CurrentMoney += Amount;
            Destroy(gameObject);
        }
    }

    private void Activate()
    {
        ignorePlayer = false;
    }

    public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
        }
    }

    public bool IgnorePlayer
    {
        get
        {
            return ignorePlayer;
        }

        set
        {
            ignorePlayer = value;
            if(value == true)
            {
                Invoke("Activate", delay);
            }
        }
    }
}
