using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Items_And_Inventory;

public class InventoryGridHandler : MonoBehaviour, IPointerClickHandler, IContainable
{
	#region Data Members

	[SerializeField]
	private List<GameObject> containers;
	private bool isGrid = true;

	public GameObject itemSlotOptionPrefab;
	public GameObject containerPrefab;
	public int gridNumber;

	#endregion

	#region Setters & Getters

	public List<GameObject>Item_Containers
	{
		get { return containers; }
	}

	public bool Is_Inventory_Grid
	{
		get { return isGrid; }
		set { isGrid = value; }
	}

	#endregion

	#region Built-in Unity Methods

	// Use this for initialization
	void Awake () 
	{
		containers = new List<GameObject>();

		for(int i = 0; i < gridNumber; i++)
		{
			GameObject tmp = Instantiate(containerPrefab);
			tmp.GetComponent<ItemContainer>().Grid_Value = i;
			containers.Add(tmp);
			tmp.transform.SetParent(transform);
		}
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Raises the pointer click event.
	/// Called when the user clicks on an ItemSlot.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerClick(PointerEventData eventData)
	{
		//When user presses the Right Mouse click
		if(Input.GetMouseButtonUp(1))
		{
			//Checks if the Right Mouse click pressed an Item Container GameObject
			if(eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInChildren<ISlottable>() != null)
			{
				//Makes the sub-menu appear on the menu.
				//When any button is pressed on this prefab,
				//blocksRayCasts should be set to true again.
				itemSlotOptionPrefab.SetActive(true);
				//itemSlotOptionPrefab.transform.SetParent(transform);

				Vector3 temp = eventData.pointerPressRaycast.gameObject.transform.position;;

				itemSlotOptionPrefab.transform.position = new Vector3(temp.x + 30.0f, temp.y + 15.0f, temp.z);
				itemSlotOptionPrefab.GetComponent<ItemSlotOption>().Item_Slot_Reference = eventData.pointerCurrentRaycast.gameObject;

				//If it is, then disable the Inventory, and all of its children
				//so the user won't be clicking by accident all over the place.
				gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

				Debug.Log("Uh...");
			}
		}
	}

	public void AddNewItemSlotToGrid(GameObject itm)
	{
		//Declaring local variables
		bool isPlaced = false;

		Debug.Log("AddNewItemSlotToGrid is called");
		Debug.Log(containers.Count);

		foreach(GameObject container in containers)
		{
			if(container.GetComponentInChildren<ISlottable>() == null && isPlaced == false)
			{
				itm.transform.SetParent(container.transform);
				itm.transform.position = container.transform.position;
				itm.GetComponent<ISlottable>().Grid_Position = container.GetComponent<ItemContainer>().Grid_Value;
				itm.GetComponent<ISlottable>().Slot_Parent = container.transform;
				isPlaced = true;
			}
		}
	}
		
	#endregion

	#region Private Methods

	#endregion


}
