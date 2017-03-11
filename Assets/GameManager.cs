using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameObject audioPrefab;
    public GameObject audioPrefabSource;
    public AudioClip shotSound;

	private List<ManagedUpdateBehavor> managedObjects = new List<ManagedUpdateBehavor>();

    public static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioPrefab = audioPrefabSource;
			Application.targetFrameRate = 60;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    public void Shot()
    {
        GameObject go = GameObject.Instantiate(audioPrefab) as GameObject;
        go.transform.parent = instance.transform;

        Audio a = go.GetComponent<Audio>();
        a.PlaySoundOnce(shotSound);
    }

	public void GameOver()
	{
		Debug.Log(transform.Find("Canvas"));
		transform.Find("Canvas").transform.Find("GameOver").gameObject.SetActive(true);
	}

	public void YouWin()
	{
		Debug.Log(transform.Find("Canvas"));
		transform.Find("Canvas").transform.Find("YouWin").gameObject.SetActive(true);
	}

	public void Restart()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}

	public void NextLevel()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
	}

	void Update()
	{
		int count = managedObjects.Count;
		for (int i = 0; i < count; ++i) {
			managedObjects[i].ManagedUpdate();
		}
	}

	public void AddManagedUpdate(ManagedUpdateBehavor obj)
	{
		managedObjects.Add(obj);
	}

	public void RemoveManagedUpdate(ManagedUpdateBehavor obj)
	{
		managedObjects.Remove(obj);
	}

}
