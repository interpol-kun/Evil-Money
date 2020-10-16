using UnityEngine;

public class LevelInit : MonoBehaviour {

	void Awake () {
        Application.targetFrameRate = 70;
        Instantiate(Resources.Load("Main camera"));
	}

}
