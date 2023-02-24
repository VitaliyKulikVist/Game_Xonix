﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEditor;
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

#if UNITY_EDITOR
		[Header("On Draw Gizmos settings")]
		[SerializeField] private bool _onHandles = false;
#endif

		#region Variable 
		private Dictionary<Vector3, GridUnitSea> _unitsSeaDictionary = default;
		private Dictionary<Vector3, GridUnitLand> _unitsLandDictionary = default;

		private List<GridUnitSea> _gridUnitSeaList = new List<GridUnitSea>();
		private List<GridUnitLand> _gridUnitLandList = new List<GridUnitLand>();

		private Vector3 _tempStartPosition = Vector3.zero;
		private bool _tempOffset = false;
		private GridUnitLand _tempGridUnitLand = null!;

		private GridUnitSea _tempGridUnitSea = null!;
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
			ResetAllVariable();
			GenerateGrid();
			
		}

		private void ReactionFinishgame(LevelResult levelResult) {
			ResetAllVariable();
		}
		#endregion

		private void GenerateGrid() {
			_unitsSeaDictionary = new Dictionary<Vector3, GridUnitSea>();
			_unitsLandDictionary = new Dictionary<Vector3, GridUnitLand>();

			for (int x = 0; x < _width; x++) {
				for (int y = 0; y < _height; y++) {

					_tempStartPosition = new Vector3(_offsetFirstUnit.x + x + x * _offsetByWidthAndHeigth, _offsetFirstUnit.y + y + y * _offsetByWidthAndHeigth);

					_tempOffset = (x == _width - 1 || y == _height - 1) || (x == 0 || y == 0);

					if (_tempOffset) {

						_tempGridUnitLand = ShowLandUnit(_tempStartPosition, GridUnitLandType.Land_1, $"Land {x} {y}");
						if (_tempGridUnitLand == null) {
							throw new ArgumentNullException(nameof(_tempGridUnitLand));
						}
						_unitsLandDictionary[_tempStartPosition] = _tempGridUnitLand;
					}

					else {
						_tempGridUnitSea = ShowSeaUnit(_tempStartPosition, GridUnitSeaType.Sea_1, $"Sea {x} {y}");
						if (_tempGridUnitSea == null) {
							throw new ArgumentNullException(nameof(_tempGridUnitSea));
						}

						_unitsSeaDictionary[_tempStartPosition] = _tempGridUnitSea;
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

			int min = _width * _height;

			for (int calc = 0; calc < min; calc++) {
				for (int j = 0; j < typeof(GridUnitSeaType).GetEnumValues().Length; j++) {
					AddGridSeaByType((GridUnitSeaType)j);
				}

				for (int i = 0; i < typeof(GridUnitLandType).GetEnumValues().Length; i++) {
					AddGridLandByType((GridUnitLandType)i);
				}
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

		private void ResetAllVariable() {
			_tempStartPosition = Vector3.zero;
			_tempOffset = false;
			_tempGridUnitLand = null!;
			_tempGridUnitSea = null!;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			if (_onHandles) {
				foreach (var land in _unitsLandDictionary) {
					Handles.color = Color.blue;
					Handles.Label(land.Key, land.Value.gameObject.name);
				}
				foreach (var sea in _unitsSeaDictionary) {
					Handles.color = Color.green;
					Handles.Label(sea.Key, sea.Value.gameObject.name);
				}
			}
		}
#endif
	}
}