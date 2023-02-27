using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.World.Grid {
	public class GridManager : MonoBehaviour {

		[Header("Base")]
		[SerializeField] private LevelStorage _levelStorageSO = default;
		[SerializeField] private PlayerStorage _playerStorageSO = default;
		[SerializeField] private DependencyInjections _dependencyInjectionsSO = default;

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
		#region Get\Set
		public int GetWidthAndHeigth { get => _offsetByWidthAndHeigth; }
		#endregion

		#region Variable 
		private Dictionary<Vector3, GridUnitSea> _unitsSeaDictionary = default;
		private Dictionary<Vector3, GridUnitLand> _unitsLandDictionary = default;

		private List<GridUnitSea> _gridUnitSeaList = new List<GridUnitSea>();
		private List<GridUnitLand> _gridUnitLandList = new List<GridUnitLand>();

		private Vector3 _tempStartPosition = Vector3.zero;
		private bool _tempOffset = false;
		private GridUnitLand _tempGridUnitLand = null!;
		private GridUnitSea _tempGridUnitSea = null!;

		private Coroutine _spawnGridCorotine = null!;

		private Level _currentLevel = null!;
		private GridUnitSeaType _tempGridUnitSeaType = default;
		private GridUnitLandType _tempGridUnitLandType = default;

		private int _tempRecurcyCalculationLand = 500;
		private int _tempRecurcyCalculationSea = 500;
		#endregion

		private void Awake() {
			_dependencyInjectionsSO.GridManager = this;
			//_spawnGridCorotine = StartCoroutine(PrepareUnits());
		}

		private void OnEnable() {
			GameManager.LevelStartAction += ReactionStartGame;
			GameManager.LevelFinishAction += ReactionFinishgame;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartGame;
			GameManager.LevelFinishAction -= ReactionFinishgame;

			ResetSpawnGridCorotine();
			ResetAllVariable();
		}

		private void OnDestroy() {
			ResetAllVariable(() => {
				ResetSpawnGridCorotine();
			});
		}

		#region Reaction
		private void ReactionStartGame() {
			ResetAllVariable(() => {
				PrepareVariables();
				HideAllunit();
				GenerateGrid();
			});
		}

		private void ReactionFinishgame(LevelResult levelResult) {
			ResetAllVariable();
		}
		#endregion


		private void PrepareVariables() {
			_currentLevel = _levelStorageSO.GetNextLevel(_playerStorageSO.ConcretePlayer.PlayerLevel);

			if (_currentLevel == null) {
				throw new ArgumentNullException(nameof(_currentLevel));
			}

			_tempGridUnitSeaType = _currentLevel.SeaType;
			_tempGridUnitLandType = _currentLevel.LandType;
		}

		private void GenerateGrid() {
			_unitsSeaDictionary = new Dictionary<Vector3, GridUnitSea>();
			_unitsLandDictionary = new Dictionary<Vector3, GridUnitLand>();

			for (int x = 0; x < _width; x++) {
				for (int y = 0; y < _height; y++) {

					_tempStartPosition = new Vector3(_offsetFirstUnit.x + x + x * _offsetByWidthAndHeigth, _offsetFirstUnit.y + y + y * _offsetByWidthAndHeigth);

					_tempOffset = (x == _width - 1 || y == _height - 1) || (x == 0 || y == 0);

					if (_tempOffset) {
						_tempGridUnitSea = ShowSeaUnit(_tempStartPosition, _tempGridUnitSeaType, $"{_tempGridUnitSeaType} [{x}] [{y}]");
						if (_tempGridUnitSea == null) {
							throw new ArgumentNullException(nameof(_tempGridUnitSea));
						}

						else {
							ResetRecurcy();
						}

						_unitsSeaDictionary[_tempStartPosition] = _tempGridUnitSea;
					}

					else {
						_tempGridUnitLand = ShowLandUnit(_tempStartPosition, _tempGridUnitLandType, $"{_tempGridUnitLandType} [{x}] [{y}]");
						if (_tempGridUnitLand == null) {
							throw new ArgumentNullException(nameof(_tempGridUnitLand));
						}

						else {
							ResetRecurcy();
						}
						_unitsLandDictionary[_tempStartPosition] = _tempGridUnitLand;
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

		public List<Vector3> GetListAllSeaPosition() {
			List<Vector3> tempList = new List<Vector3>();

			if (_unitsSeaDictionary != null && _unitsSeaDictionary.Count > 0) {
				foreach (var sea in _unitsSeaDictionary) {
					tempList.Add(sea.Key);
				}
			}

			return tempList;
		}

		public List<Vector3> GetListAllLandPosition() {
			List<Vector3> tempList = new List<Vector3>();

			if (_unitsLandDictionary != null && _unitsLandDictionary.Count > 0) {
				foreach (var land in _unitsLandDictionary) {
					tempList.Add(land.Key);
				}
			}

			return tempList;
		}

		#region pool Controller
		private IEnumerator PrepareUnits() {

			int min = _width * _height;

			for (int calc = 0; calc < min; calc++) {
				for (int j = 0; j < typeof(GridUnitSeaType).GetEnumValues().Length; j++) {
					yield return AddGridSeaByTypeAsync((GridUnitSeaType)j);
				}

				for (int i = 0; i < typeof(GridUnitLandType).GetEnumValues().Length; i++) {
					yield return AddGridLandByTypeAsync((GridUnitLandType)i);
				}
			}

			_spawnGridCorotine = null!;
		}

		private GridUnitSea AddGridSeaByType(GridUnitSeaType gridUnitSeaType, Func<GridUnitSea> callBack) {
			var gridUnitSea = _levelStorageSO.BaseLevelSettings.GetGridUnitSeaByType(gridUnitSeaType);
			var unit = Instantiate(gridUnitSea, _poolSeaContainer);
			unit.ResetUnit();

			_gridUnitSeaList.Add(unit);

			return callBack?.Invoke();
		}

		private IEnumerator AddGridSeaByTypeAsync(GridUnitSeaType gridUnitSeaType) {
			var gridUnitSea = _levelStorageSO.BaseLevelSettings.GetAssetReferenceSeaByType(gridUnitSeaType);
			var unitTask = gridUnitSea.InstantiateAsync(_poolSeaContainer);

			yield return unitTask;

			if (unitTask.IsDone) {
				var unit = unitTask.Result.GetComponent<GridUnitSea>();
				unit.ResetUnit();

				_gridUnitSeaList.Add(unit);
			}
		}

		private GridUnitLand AddGridLandByType(GridUnitLandType gridUnitLandType, Func<GridUnitLand> callBack) {
			var gridUnitLand = _levelStorageSO.BaseLevelSettings.GetGridUnitLandByType(gridUnitLandType);
			var unit = Instantiate(gridUnitLand, _poolLandContainer);
			unit.ResetUnit();

			_gridUnitLandList.Add(unit);

			return callBack?.Invoke();
		}

		private IEnumerator AddGridLandByTypeAsync(GridUnitLandType gridUnitLandType) {
			var gridUnitLand = _levelStorageSO.BaseLevelSettings.GetAssetReferenceLandByType(gridUnitLandType);
			var unitTask = gridUnitLand.InstantiateAsync(_poolLandContainer);

			yield return unitTask;

			if (unitTask.IsDone) {
				var unit = unitTask.Result.GetComponent<GridUnitLand>();
				unit.ResetUnit();

				_gridUnitLandList.Add(unit);
			}
		}

		private GridUnitLand ShowLandUnit(Vector3 _startPosition, GridUnitLandType gridUnitLandType, string name) {
			var tempLand = _gridUnitLandList.Find(unit => unit.IsFree && unit.GetGridUnitType == gridUnitLandType);

			if (tempLand == null) {
				_tempRecurcyCalculationLand--;
				return AddGridLandByType(gridUnitLandType, () => {
					if (_tempRecurcyCalculationLand > 0) {
						return ShowLandUnit(_startPosition, gridUnitLandType, name);
					}

					else {
						Debug.LogError($"Can`t Spasn Land Unit\t {gridUnitLandType}");

						return null;
					}
				});
			}

			else if (tempLand != null) {
				tempLand.SetName(name);
				tempLand.ShowUnit(_startPosition, gridUnitLandType);
				ResetRecurcy();

				return tempLand;
			}

			return null;
		}

		private GridUnitSea ShowSeaUnit(Vector3 _startPosition, GridUnitSeaType gridUnitSeaType, string name) {

			var tempSea = _gridUnitSeaList.FirstOrDefault(unit => unit.IsFree && unit.GetGridUnitType == gridUnitSeaType);

			if (tempSea == null) {
				_tempRecurcyCalculationSea--;
				return AddGridSeaByType(gridUnitSeaType, () => {
					if (_tempRecurcyCalculationSea > 0) {
						return ShowSeaUnit(_startPosition, gridUnitSeaType, name);
					}

					else {
						Debug.LogError($"Can`t Spasn Sea Unit\t {gridUnitSeaType}");

						return null;
					}
				});
			}

			else if (tempSea != null) {
				tempSea.SetName(name);
				tempSea.ShowUnit(_startPosition, gridUnitSeaType);
				ResetRecurcy();

				return tempSea;
			}

			return null;
		}

		private void HideAllunit() {
			_gridUnitSeaList.FindAll(unit => !unit.IsFree).ForEach(unit => unit.HideUnit());
			_gridUnitLandList.FindAll(unit => !unit.IsFree).ForEach(unit => unit.HideUnit());
		}

		#endregion

		private void ResetAllVariable(Action callBack = null) {
			_tempStartPosition = Vector3.zero;
			_tempOffset = false;
			_tempGridUnitLand = null!;
			_tempGridUnitSea = null!;

			_tempGridUnitSeaType = default;
			_tempGridUnitLandType = default;

			ResetRecurcy();

			callBack?.Invoke();
		}

		private void ResetSpawnGridCorotine() {
			if (_spawnGridCorotine != null) {
				StopCoroutine(_spawnGridCorotine);
			}
		}

		private void ResetRecurcy() {
			_tempRecurcyCalculationLand = 500;
			_tempRecurcyCalculationSea = 500;
		}

		public List<Vector3> GetListAllGridVectors() {
			List<Vector3> tempList = new List<Vector3>();

			foreach (var sea in _unitsSeaDictionary) {
				tempList.Add(sea.Key);
			}

			foreach (var land in _unitsLandDictionary) {
				tempList.Add(land.Key);
			}

			return tempList;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			if (_onHandles) {
				if (_unitsLandDictionary != null && _unitsLandDictionary.Count > 0) {
					foreach (var land in _unitsLandDictionary) {
						Handles.color = Color.blue;
						Handles.Label(land.Key, land.Value.gameObject.name);
					}
				}

				if (_unitsSeaDictionary != null && _unitsSeaDictionary.Count > 0) {
					foreach (var sea in _unitsSeaDictionary) {
						Handles.color = Color.green;
						Handles.Label(sea.Key, sea.Value.gameObject.name);
					}
				}
			}
		}
#endif
	}
}
