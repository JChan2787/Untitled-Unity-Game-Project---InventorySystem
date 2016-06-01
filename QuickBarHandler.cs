using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Items_And_Inventory;

public class QuickBarHandler : MonoBehaviour, IContainable
{
	#region Data Members

	public GameObject itemSlotContainerPrefab;

	[SerializeField]
	private List<GameObject> quickSlotList;
	private bool isInventory = false;

	#endregion

	#region Setters & Getters

	public List<GameObject> Item_Containers
	{
		get { return quickSlotList; }
	}

	public bool Is_Inventory_Grid
	{
		get { return isInventory; }
		set { isInventory = value; }
	}

	#endregion

	#region Built-in Unity Methods

	void Start()
	{
		quickSlotList = new List<GameObject>();

		for(int i = 0; i < 4; i++)
		{
			GameObject tmp = Instantiate(itemSlotContainerPrefab);
			tmp.GetComponent<ItemContainer>().Grid_Value = i;
			quickSlotList.Add(tmp);
			tmp.transform.SetParent(transform);
		}
	}
		
	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
