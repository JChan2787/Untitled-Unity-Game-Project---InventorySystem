using System;
using UnityEngine;

namespace Player_Info
{
	/// <summary>
	/// Interface that will contain the Player's Weight.
	/// </summary>
	public interface ICarryable
	{
		int Player_Max_Weight
		{
			get;
			set;
		}
	}

	/// <summary>
	/// This interface is used to check if the character is
	/// the current selected character.
	/// </summary>
	public interface IControllable
	{
		bool Character_Is_Selected
		{
			get;
			set;
		}
	}
}

