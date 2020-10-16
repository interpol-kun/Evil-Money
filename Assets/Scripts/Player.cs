using UnityEngine;

public class Player : MonoBehaviour {

    public delegate void MoneyChange();
    public event MoneyChange OnMoneyChange;

    public delegate void Reload();
    public event Reload OnReload;

    public delegate void ReloadTimeChange(float time);
    public event ReloadTimeChange OnReloadTimeChange;

    public delegate void EscTimeChange(float time);
    public event EscTimeChange OnEscTimeChange;

    public delegate void ToMenu();
    public event ToMenu OnToMenu;

    public delegate void GameOver();
    public event GameOver OnGameOver;

    [SerializeField]
    private float speed = .1f;
    [SerializeField]
    private float throwSpeed = 300f;
	[SerializeField]
	private Money moneyGO = null;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private int overallMoney;
    [SerializeField]
    private int currentMoney;
    [SerializeField]
    private int throwedMoney = 10000;

    [SerializeField]
    private int goalMoney = 1000000;

    [SerializeField]
    private float slowFactor = 0;

    [SerializeField]
    [Range(1, 10)]
    private int slowFactorScale = 5;

    [SerializeField]
    private float holdTreshold = 1f;

    private bool rButtonHolded;

    private float holdTime;

    private bool freezeInput;

    private bool escButtonHolded;
    private float escholdTime;

    private AudioSource audio = null;

    [SerializeField]
    private AudioClip gameOverSound = null;
    [SerializeField]
    private AudioClip throwMoneySound = null;

    void Start () {
        audio = GetComponent<AudioSource>();
        FreezeInput = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        currentMoney = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (!freezeInput)
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            if (direction == Vector3.zero)
            {
                rb.velocity = Vector3.zero;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (CurrentMoney >= throwedMoney)
                {
                    ThrowMoney();
                }
                else
                {

                }
            }
 				
            direction.Normalize();

            rb.MovePosition(transform.position + (direction * (speed - slowFactor)));

            if (direction != Vector3.zero)
            {
                float rotationZ = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                spriteRenderer.transform.rotation = Quaternion.Euler(90f, 0.0f, rotationZ);
            }
            rb.velocity = Vector3.zero;//prevent player from sloping
        }

        if (Input.GetKeyDown(KeyCode.R) && !rButtonHolded)
        {
            holdTime = 0f;
            rButtonHolded = true;
        }

        if (rButtonHolded)
        {
            holdTime += Time.deltaTime;
            OnReloadTimeChange(holdTime);
            if (holdTime >= holdTreshold)
            {
                if (OnReload != null)
                {
                    OnReload();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
		{
			rButtonHolded = false;
			holdTime = 0f;
			OnReloadTimeChange(holdTime);
		}

        if (Input.GetKeyDown(KeyCode.Escape) && !escButtonHolded)
        {
            escholdTime = 0f;
            escButtonHolded = true;
        }

        if (escButtonHolded)
        {
            escholdTime += Time.deltaTime;
            OnEscTimeChange(escholdTime);
            if (escholdTime >= holdTreshold)
            {
                if (OnToMenu != null)
                {
                    OnToMenu();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            escButtonHolded = false;
            escholdTime = 0f;
            OnEscTimeChange(holdTime);
        }


    }

    private void ThrowMoney()
    {
        audio.PlayOneShot(throwMoneySound, 1f);
		var money = Instantiate(moneyGO.gameObject, transform.position, transform.rotation) as GameObject;
        var moneyComponent = money.GetComponent<Money>();
        moneyComponent.IgnorePlayer = true;

        var moneyRb = money.GetComponent<Rigidbody>();
		money.transform.position = new Vector3 (money.transform.position.x, 1, money.transform.position.z);
		moneyRb.AddForce(spriteRenderer.transform.right * Random.Range(throwSpeed-30f, throwSpeed+30f));
		money.transform.eulerAngles = new Vector3 (90, Random.Range (0, 360), 0);

		int _throwedMoney = throwedMoney;
		if(currentMoney*.1f >= throwedMoney){
			_throwedMoney = currentMoney /10;
		}
		moneyComponent.Amount = _throwedMoney;
		moneyComponent.playerCreatedMoney = true;
		CurrentMoney -= _throwedMoney;
    }

    public void Catched()
    {
        if(OnGameOver != null)
        {
            OnGameOver();
        }
        FreezeInput = true;
        audio.PlayOneShot(gameOverSound, 0.1f);
        gameObject.tag = "Untagged";
    }

    #region Accessors
    public int CurrentMoney
    {
        get
        {
            return currentMoney;
        }

        set
        {
            if(currentMoney < value)
                audio.PlayOneShot(audio.clip, Random.Range(0.4f, 0.7f));

            currentMoney = value;
			slowFactor = ((float)currentMoney/GoalMoney)/slowFactorScale;
            if(slowFactor > speed)
            {
                slowFactor = speed;
            }
            if(OnMoneyChange != null)
                OnMoneyChange();
        }
    }

    public int GoalMoney
    {
        get
        {
            return goalMoney;
        }

        set
        {
            goalMoney = value;
        }
    }

    public float HoldTreshold
    {
        get
        {
            return holdTreshold;
        }
    }

    public bool FreezeInput
    {
        get
        {
            return freezeInput;
        }

        set
        {
            freezeInput = value;
        }
    }

    #endregion
}
