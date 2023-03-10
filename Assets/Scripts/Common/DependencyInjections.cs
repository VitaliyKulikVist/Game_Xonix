using Assets.Scripts.Ui;
using Assets.Scripts.World.Grid;
using UnityEngine;

namespace Assets.Scripts.Common {
	[CreateAssetMenu(menuName = "ScriptableObjects/DependencyInjections", fileName = "DependencyInjections")]
	public class DependencyInjections : ScriptableObject {
		public RectTransform PlayerPosition { get; set; }
		public GridManager GridManager { get; set; }
		public SwipeController SwipeController { get; set; }
	}
}
