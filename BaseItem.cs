using UnityEngine;
using System.Collections;
using Items_And_Inventory;

/// <summary>
/// Base Item is an object that is created by
/// the Item Data base. This class will act
/// as the main container for Item Information.
/// </summary>
public class BaseItem
{
	#region Data Members

	protected int itemID;					// The item's ID number
	protected string itemName;				// The item name
	protected string itemDescription;		// The item's description
	public Sprite itemIcon;					// The sprite for the item icon
	protected int price;					// The price of the item
	protected BaseItemType type;			// The type of item.
	protected bool isStackable;				// Whether or not the item is stackable
	protected int itemWeight;               // Item's weight value

	#endregion

	#region Setters & Getters

	// Gets and sets the item ID
	public int Item_ID 
	{
		get {return itemID;}
		set {itemID = value;}
	}

	// Gets and sets the item name
	public string Item_Name
	{
		get {return itemName;}
		set {itemName = value;}
	}

	// Gets and sets the item description
	public string Item_Description
	{
		get {return itemDescription;}
		set {itemDescription = value;}
	}

	// Gets and sets the item icon
	public Sprite Item_Sprite
	{
		get {return itemIcon;}
		set {itemIcon = value;}
	}

	// Gets and sets the item price
	public int Item_Price
	{
		get {return price;}
		set {price = value;}
	}

	/// <summary>
	/// Return type of item (equipment,consumeable,non Usable)
	/// </summary>
	/// <value>The type of the base item.</value>
	public BaseItemType Base_Item_Type
	{
		get {return type;}
		set {type = value;}
	}

	// Gets and sets the item's stackability
	public bool Is_Stackable 
	{
		get {return isStackable;}
		set {isStackable = value;}
	}

	//Gets and sets the item's weight
	public int Item_Weight
	{
		get { return itemWeight; }
		set { itemWeight = value; }
	}

	#endregion

	#region Built-In Unity Methods

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseItem"/> class.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="name">Name.</param>
	/// <param name="desc">Desc.</param>
	/// <param name="spr">Spr.</param>
	/// <param name="price">Price.</param>
	/// <param name="_type">Type.</param>
	/// <param name="stack">If set to <c>true</c> stack.</param>
	/// <param name="_weight">Weight.</param>
	public BaseItem(int id, string name, string desc, Sprite spr, int price, BaseItemType _type, bool stack, int _weight)
	{
		itemID = id;
		itemName = name;
		itemDescription = desc;
		itemIcon = spr;
		type = _type;
		isStackable = stack;
		itemWeight = _weight;
	}
		
	#endregion
}
