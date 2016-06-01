using UnityEngine;
using System.Collections;

public class ItemSlotOption : MonoBehaviour
{
	#region Data Members

	public GameObject amtDropOptionMenuPrefab;
	public Transform amtDropOptionDisplay;

	private GameObject itmSlotRef;

	#endregion

	#region Setters & Getters

	public GameObject Item_Slot_Reference
	{
		get { return itmSlotRef; }
		set { itmSlotRef = value; }
	}

	#endregion

	#region Built-in Unity Methods

	#endregion

	#region Public Methods

	public void DropItem()
	{
		amtDropOptionMenuPrefab.SetActive(true);
		amtDropOptionMenuPrefab.transform.position = amtDropOptionDisplay.position;
		amtDropOptionMenuPrefab.GetComponent<DropOptionMenu>().DropItemCall(true, itmSlotRef);
		this.gameObject.SetActive(false);
	}

	#endregion

	#region Private Methods

	#endregion
}
