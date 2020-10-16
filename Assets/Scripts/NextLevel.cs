using UnityEngine;

public class NextLevel : MonoBehaviour {


	void OnEnable(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex + 1);
	}

}
