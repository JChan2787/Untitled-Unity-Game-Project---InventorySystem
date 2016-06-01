/* CharacterInventory - version 1.0
 * 
 * 
 * Created by			    : Jay Chan Jr.
 * Date					    : 17 MAY 2016
 * Email					: jaychan027@gmail.com
 * 
 * Contributors			    
 * Kien Ngoc Nguyen - https://github.com/tgsoon2002
 * 					- tgsoon2002@gmail.com
 * 
 * Matthew Bower    - https://github.com/bowerm386
 *                  - bowerm386@gmail.com
 * ---------------------------------------------------
 * 
 * This class will be the Character object's Inventory.
 * Uses ItemInfo to keep track of item information,
 * and can be used to manipulate items inside it via
 * the defined public methods.
 */

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items_And_Inventory;
using Player_Info;

public class CharacterInventory : MonoBehaviour
{
	#region Data Members

	[SerializeField]
	private List<ItemInfo> itemList;
	private int currentWeight;
	private int tempMaxWeight;			//Delete sthis shit later.

	public GameObject playerReference;	//Player Reference.
	public GameObject lootPrefab;    	//Prefab for a Loot this class
								    	//shall instantiate.

	#region Events

	/*
	 *  This is the delegate for the event - ItemEvent.
	 *	PARAMETERS:
	 *  - item will be passed in so listeners shall know which
	 *    item to update and the quantity to update.
	 *  - state is mainly used for InventoryMenu
	 *	  true  : Create a slot
	 *	  false : Update the slot and possibly remove it
	 *	- excludeQuickBar's value will tell if the QuickBar needs
	 *	  to update its contents.
	 */
	public delegate void ItemAction(ItemInfo item, bool state, bool excludeQuickBar);
	public event ItemAction ItemEvent;

	#endregion

	#endregion

	#region Setters & Getters

	#endregion

	#region Built-in Unity Methods

	void Start()
	{
		itemList = new List<ItemInfo>();
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Initialiazes the inventory menu.
	/// Called when the Inventory Menu is
	/// opened by the user.
	/// </summary>
	public void InitializeMenu()
	{
		Debug.Log("InitializeMenu" + " " + "called");

		foreach(ItemInfo item in itemList)
		{
			Debug.Log("Init: " + item.Grid_Index);
			TriggerItemEvent(item, true, false);		
		}
	}

	/// <summary>
	/// Updates the inventory. This is called
	/// when the user closes the inventory, or
	/// switches to a different character.
	/// </summary>
	public void UpdateInventory()
	{
		Debug.Log("UpdateInventory is called");

		if(InventoryMenu.Instance.Item_Slots.Count > 0)
		{
			foreach(GameObject itm in InventoryMenu.Instance.Item_Slots)
			{
				Debug.Log("Grid: " + itm.GetComponent<ISlottable>().Grid_Position);

				string tmpID = itm.GetComponent<ISlottable>().Item_ID;
				int tmpGridPos = itm.GetComponent<ISlottable>().Grid_Position;

				//TO DO for next revision:
				//Refactor List to a Dictionary to increase performance.
				int index = itemList.FindIndex(o => o.Inventory_Unique_ID == itm.GetComponent<ISlottable>().Item_ID);

				//Make a check
				if(index <= -1)
				{
					Debug.LogError("WARNING -- Item from Menu does not exist in CharacterInventory");
				}


				itemList[index].Grid_Index = itm.GetComponent<ISlottable>().Grid_Position;
				itemList[index].Quickbar_Index = itm.GetComponent<ISlottable>().Is_On_Slot;

				//Check whether the item slot resides in the main grid
				//or the quick bar
//				if(itm.GetComponent<ISlottable>().Is_On_Slot <= -1)
//				{
//					itemList[index].Grid_Index = tmpGridPos;
//					itemList[index].Quickbar_Index = -1;	//Just a sanity check
//				}
//				else
//				{
//					itemList[index].Grid_Index = -1;
//					itemList[index].Quickbar_Index = itm.GetComponent<ISlottable>().Is_On_Slot;
//				}
			}	
		}
	}
		
	/// <summary>
	/// Called when adding an Item to the CharacterInventory
	/// </summary>
	/// <param name="newItem">New item.</param>
	public void AddItem(ItemInfo newItem)
	{
		//Checks if the character's weight can carry the new item.
		if(newItem.TotalWeight() + currentWeight <= playerReference.GetComponent<ICarryable>().Player_Max_Weight)
		{
			//Checking if the new item is stackable
			if(newItem.Item_Info.Is_Stackable)
			{
				//Check if newItem already exists in the list.
				int index = itemList.FindIndex(item => item.Item_Info.Item_ID == newItem.Item_ID);

				//IList.FindIndex returns -1, if the list does not contain the new item
				if(index > -1)
				{
					itemList[index].Item_Quantity += newItem.Item_Quantity;

					if(playerReference.GetComponent<IControllable>().Character_Is_Selected)
					{
						//Trigger an event!
						//Passing the item to be updated.
						//The second argume////////////////nt is false because we
						//are just updating existing information.
						TriggerItemEvent(itemList[index], false, true);
					}		
				}
				//This case is when a new Stackable item is going to be added
				//to the Inventory.
				else
				{
					ItemInfo temp = newItem;
					temp.Inventory_Unique_ID = GenerateUniqueIDForItem(newItem);
					itemList.Add(temp);

					//Checking if Inventory Menu UI is displayed
					if(InventoryMenu.Instance.menuPrefab.activeInHierarchy)
					{
						//If true, trigger an event and pass true to create
						//an item slot element on the Menu UI!
						TriggerItemEvent(temp, true, false);
					}
				}
			}
			else
			{
				ItemInfo temp = newItem;
				temp.Inventory_Unique_ID = GenerateUniqueIDForItem(newItem);
				itemList.Add(temp);

				//Checking if Inventory Menu UI is displayed
				if(InventoryMenu.Instance.menuPrefab.activeInHierarchy)
				{
					//If true, trigger an event and pass true to create
					//an item slot element on the Menu UI!
					TriggerItemEvent(temp, true, false);
				}
			}
		}
	}

	/// <summary>
	/// Removes the item.
	/// </summary>
	/// <param name="itemID">Item I.</param>
	/// <param name="qty">Qty.</param>
	/// <param name="toDrop">If set to <c>true</c> to drop.</param>
	public void RemoveItem(string itemID, int qty, bool toDrop)
	{
		//Declaring local variables
		ItemInfo toRemove;
		int index;

		//Find the index if the item exists
		index = itemList.FindIndex(item => item.Inventory_Unique_ID == itemID);

		//If the index returns -1, the catch state shall be executed.
		try
		{
			//Check if the Item is Stackable
			if(itemList[index].Item_Info.Is_Stackable)
			{
				//Checks if the quantity to be removed is going to
				//completely zero out the item.
				if(qty >= itemList[index].Item_Quantity)
				{
					toRemove = itemList[index];
					toRemove.Item_Quantity = 0;
			
					//Completely remove the item
					itemList.RemoveAt(index);
				}
				else
				{
					//Simply update the item's quantity.
					itemList[index].Item_Quantity -= qty;
					toRemove = itemList[index];
				}

				//Reverse the sign of the quantity, so the menu knows to subtract it.
				//toRemove.Item_Quantity *= -1;

				//Trigger the event:
				//Pass false, since we will be 
				//updating the item slot. Or in
				//this case, completely removing it.
				TriggerItemEvent(toRemove, false, true);

				//Check if removing means dropping it to the Game World.
				if(toDrop)
				{
					//Instantiate a new ItemInfo object to be used by the
					//Loot GameObject.
					ItemInfo itemToDrop = new ItemInfo(toRemove.Item_Info, qty);

					//Call DropLoot
					DropLoot(itemToDrop);
				}
			}
			else
			{
				//Update all items' address in the list first
				//before removing it.


				//This is for the case if the item to be removed is an
				//Equipment item.
				toRemove = itemList[index];
				itemList.RemoveAt(index);

				toRemove.Item_Quantity = 0;

				//Trigger the event:
				//Pass false, since we will be 
				//updating the item slot. Or in
				//this case, completely removing it.
				TriggerItemEvent(toRemove, false, false);
			}
		}
		catch
		{
			//Log an error to the console (for now).
			Debug.LogError("WARNING - Removing Item in Inventory that does not exist.");
		}
	}

	/// <summary>
	/// Equips the item.
	/// </summary>
	/// <param name="itemID">Item I.</param>
	public void EquipItem(int itemID)
	{
		
	}

	/// <summary>
	/// Uses the item. This shall be
	/// mainly used by the QuickSlot, and 
	/// sometimes used by the InventoryMenu
	/// </summary>
	/// <param name="itemID">Item I.</param>
	public void UseItem(int itemID)
	{
		
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Triggers the item event.
	/// </summary>
	/// <param name="newItem">New item.</param>
	private void TriggerItemEvent(ItemInfo newItem, bool state, bool excludeQB)
	{
		//First make sure there are any listeners
		if(ItemEvent != null)
		{
			//Trigger the event if there are.
			ItemEvent(newItem, state, excludeQB);
		}
	}

	/// <summary>
	/// Drops the loot game object onto the game world,
	/// along with the associated ItemInfo
	/// </summary>
	/// <param name="droppedItem">Dropped item.</param>
	private void DropLoot(ItemInfo droppedItem)
	{
		GameObject loot = Instantiate(lootPrefab);

		//Initialize loot components.
		loot.GetComponent<ILootable>().SetLootContents(droppedItem);

		//Initialize loot position with character's position plus offset.
		//The offset will be temporary.
		loot.transform.position = new Vector3(playerReference.transform.position.x + 3.0f,
											  playerReference.transform.position.y + 1.5f,
											  playerReference.transform.position.z);
	}

	/// <summary>
	/// Updates the item indeces.
	/// </summary>
	/// <param name="start">Start.</param>
	private string GenerateUniqueIDForItem(ItemInfo _item)
	{
		//Declaring local variables
		string id = "";

		if(_item.Item_Info.Is_Stackable)
		{
			//If the item is a Stackable type,
			//simply use the item ID
			id = _item.Item_Info.Item_ID.ToString();

		}
		else
		{
			Guid g = Guid.NewGuid();
			id = g.ToString();
		}

		return id;
	}

	#endregion

}