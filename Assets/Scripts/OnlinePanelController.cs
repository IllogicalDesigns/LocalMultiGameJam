using UnityEngine;
using System.Collections;

public class OnlinePanelController : MonoBehaviour {
	public GameObject finderGroup;
	public GameObject creatorGroup;

	// Use this for initialization
	void Start () {
		finderGroup.SetActive (true);
		creatorGroup.SetActive (false);
	
	}
	public void SwitchToFinderGroup()
	{
		finderGroup.SetActive (true);
		creatorGroup.SetActive (false);
	}
	public void SwitchToCreaterGroup()
	{
		finderGroup.SetActive (false);
		creatorGroup.SetActive (true);
	}
}
