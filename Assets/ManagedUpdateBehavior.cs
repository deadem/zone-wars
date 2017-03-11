using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedUpdateBehavor : MonoBehaviour {
	public virtual void Start()
	{
		GameManager.instance.AddManagedUpdate(this);
	}

	public virtual void ManagedUpdate()
	{
	}
	
	void OnDestroy()
	{
		GameManager.instance.RemoveManagedUpdate(this);
	}
}
