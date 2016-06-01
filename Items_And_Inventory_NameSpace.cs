using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items_And_Inventory
{
	#region Items Enumerations

	/// <summary>
	/// Enumerates different types of
	/// 'base' items in the game.
	/// </summary>
	public enum BaseItemType 
	{
		EQUIPMENT,
		CONSUMABLE,
		NON_CONSUMABLE
	};

	#endregion

	#region Inventory Interfaces

	/// <summary>
	/// Stores the most basic information that ItemInfo
	/// contains.
	/// </summary>
	public interface IStoreable
	{
		int Item_ID 
		{
			get;
		}

		string Item_Name 
		{
			get;
			set;
		}

		string Item_Description
		{
			get;
			set;
		}

		int Item_Type 
		{
			get;
			set;
		}

		int Item_Quantity 
		{
			get;
			set;
		}
			
		Sprite Item_Sprite
		{
			get;
		}

		int Quickbar_Index
		{
			get;
			set;
		}

		string Inventory_Unique_ID
		{
			get;
			set;
		}

		int Grid_Index
		{
			get;
			set;
		}
	}


	/// <summary>
	/// Used by the Inventory Menu when
	/// handling Equipment items passed by
	/// the ItemInfo class.
	/// </summary>
	public interface IEquippable :IStoreable
	{
		bool Is_Equipped
		{
			get;
			set;
		}
	}
		
	#endregion

	#region Item Slots Interface

	/// <summary>
	/// Used by the Item Slot Game Object
	/// to decouple its component class.
	/// </summary>
	public interface ISlottable
	{
		int Item_Quantity
		{
			get;
			set;
		}

		string Item_ID
		{
			get;
			set;
		}

		int Grid_Position
		{
			get;
			set;
		}

		int Is_On_Slot
		{
			get;
			set;
		}

		Transform Root_Transform
		{
			get;
			set;
		}

		Transform Slot_Parent
		{
			get;
			set;
		}

		void InitializeItemSlot(IStoreable item);
		void ReInitializeTransform(Transform newParent, int _gridIndex);
		void UpdateQuantity(int qty);
	}

	/// <summary>
	/// Interface to be used by Inventory 
	/// and QuickBar menus.
	/// </summary>
	public interface IContainable
	{
		List<GameObject> Item_Containers
		{
			get;
		}

		bool Is_Inventory_Grid
		{
			get;
			set;
		}
	}
		
	#endregion

	#region Loot Interfaces

	/// <summary>
	/// Interface for the Loot GameObject,
	/// since the only thing we need from it
	/// is the ItemInfo object it contains.
	/// </summary>
	public interface ILootable
	{
		ItemInfo Item_Info
		{
			get;
		}

		void SetLootContents(ItemInfo itm);
	}

	#endregion
}

