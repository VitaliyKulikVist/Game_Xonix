using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Ui {
	[Serializable]
	public class ButtonControll {
		
		[SerializeField]private ButtonType buttonType = ButtonType.None;
		[SerializeField] private Sprite buttonSprite = default;

		#region Get\Set
		public ButtonType ButtonType { get => buttonType; }
		public Sprite Sprite { get => buttonSprite; }
		#endregion
	}
}
