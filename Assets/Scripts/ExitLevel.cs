using UnityEngine;

public class ExitLevel : MonoBehaviour {

    [SerializeField]
	private UIHandler _UI = null;

    private AudioSource audio = null;

    private void Start()
    {
        _UI = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();
        audio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
		if(player != null && player.CurrentMoney > 0)
        {
            audio.PlayOneShot(audio.clip, 0.8f);
            _UI.Finish();
        }
    }
}
