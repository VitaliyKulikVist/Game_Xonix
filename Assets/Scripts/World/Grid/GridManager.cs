using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.World.Grid {
	public class GridManager : MonoBehaviour {

		[Header("Base")]
		[SerializeField] private LevelStorage _levelStorage = default;

		[Header("Settings")]
		[SerializeField] private int _width = 0;
		[SerializeField] private int _height = 0;
		[SerializeField] private Vector2 _offsetFirstUnit = Vector2.zero;
		[SerializeField] private int _offsetByWidthAndHeigth = 64;

		[Header("Components")]
		[SerializeField] private Transform _poolSeaContainer = default;
		[SerializeField] private Transform _poolLandContainer = default;

		#region Variable 
		private Dictionary<Vector3, GridUnitSea> _unitsSeaDictionary = default;
		private Dictionary<Vector3, GridUnitLand> _unitsLandDictionary = default;

		private List<GridUnitSea> _gridUnitSeaList = new List<GridUnitSea>();
		private List<GridUnitLand> _gridUnitLandList = new List<GridUnitLand>();
		#endregion

		private void Awake() {
			PrepareUnits();
		}

		private void OnEnable() {
			GameManager.LevelStartAction += ReactionStartGame;
			GameManager.LevelFinishAction += ReactionFinishgame;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartGame;
			GameManager.LevelFinishAction -= ReactionFinishgame;
		}

		#region Reaction
		private void ReactionStartGame() {
			GenerateGrid();
		}

		private void ReactionFinishgame(LevelResult levelResult) {

		}
		#endregion

		private void GenerateGrid() {
			_unitsSeaDictionary = new Dictionary<Vector3, GridUnitSea>();
			_unitsLandDictionary = new Dictionary<Vector3, GridUnitLand>();

			for (int x = 0; x < _width; x++) {
				for (int y = 0; y < _height; y++) {
					bool offset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
					if (offset) {

						Vector3 startPos = new Vector3(_offsetFirstUnit.x + x + x * _offsetByWidthAndHeigth, _offsetFirstUnit.y + y + y * _offsetByWidthAndHeigth);

						var unitGridLand = ShowLandUnit(startPos, GridUnitLandType.Land_1, $"Unit Land {x} {y}");
						if (unitGridLand == null) {
							throw new ArgumentNullException(nameof(unitGridLand));
						}
						unitGridLand.ChangeColor(offset);

						_unitsLandDictionary[startPos] = unitGridLand;
					}

					else {
						Vector3 startPos = new Vector3(_offsetFirstUnit.x + x + x * _offsetByWidthAndHeigth, _offsetFirstUnit.y + y + y * _offsetByWidthAndHeigth);

						var unitGridSea = ShowSeaUnit(startPos, GridUnitSeaType.Sea_1, $"Unit Sea {x} {y}");
						if(unitGridSea == null) {
							throw new ArgumentNullException(nameof(unitGridSea));
						}
						unitGridSea.ChangeColor(offset);

						_unitsSeaDictionary[startPos] = unitGridSea;
					}
				}
			}
		}

		public GridUnit<GridUnitLandType> GetUnitLandByPosition(Vector3 position) {
			if (_unitsLandDictionary.TryGetValue(position, out var land)) {
				return land;
			}

			return null!;
		}

		public GridUnit<GridUnitSeaType> GetUnitSeaByPosition(Vector3 position) {
			if (_unitsSeaDictionary.TryGetValue(position, out var sea)) {
				return sea;
			}

			return null!;
		}

		#region pool Controller
		private void PrepareUnits() {
			for (int j = 0; j < typeof(GridUnitSeaType).GetEnumValues().Length; j++) {
				AddGridSeaByType((GridUnitSeaType)j);
			}

			for (int i = 0; i < typeof(GridUnitLandType).GetEnumValues().Length; i++) {
				AddGridLandByType((GridUnitLandType)i);
			}
		}

		private void AddGridSeaByType(GridUnitSeaType gridUnitSeaType) {
			var gridUnitSea = _levelStorage.BaseLevelSettings.GetGridUnitSeaByType(gridUnitSeaType);
			var unit = Instantiate(gridUnitSea, _poolSeaContainer);
			unit.ResetUnit();

			_gridUnitSeaList.Add(unit);
		}

		private void AddGridLandByType(GridUnitLandType gridUnitLandType) {
			var gridUnitLand = _levelStorage.BaseLevelSettings.GetGridUnitLandByType(gridUnitLandType);
			var unit = Instantiate(gridUnitLand, _poolLandContainer);
			unit.ResetUnit();

			_gridUnitLandList.Add(unit);
		}

		private GridUnitLand ShowLandUnit(Vector3 _startPosition, GridUnitLandType gridUnitLandType, string name) {
			bool check = _gridUnitLandList.Find(unit => unit.IsFree && unit.GetGridUnitType == gridUnitLandType) == null;
			if (check) {
				AddGridLandByType(gridUnitLandType);
			}

			var unit = _gridUnitLandList.Find(unit => unit.IsFree && unit.GetGridUnitType == gridUnitLandType);
			if (unit != null) {
				unit.SetName(name);
				unit.ShowUnit(_startPosition, gridUnitLandType);
			}

			return unit;
		}

		private GridUnitSea ShowSeaUnit(Vector3 _startPosition, GridUnitSeaType gridUnitSeaType, string name) {
			bool check = _gridUnitSeaList.Find(unit => unit.IsFree && unit.GetGridUnitType == gridUnitSeaType) == null;
			if (check) {
				AddGridSeaByType(gridUnitSeaType);
			}

			var unit = _gridUnitSeaList.Find(unit => unit.IsFree && unit.GetGridUnitType == gridUnitSeaType);
			if (unit != null) {
				unit.SetName(name);
				unit.ShowUnit(_startPosition, gridUnitSeaType);
			}

			return unit;
		}

		#endregion
	}
}
