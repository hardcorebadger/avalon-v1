﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gamelogic.Core {
	
	public class SelectionManager : MonoBehaviour {

		public RectTransform dragSelector;
		public GameObject tagPrefab;
		public GameObject canvas;
		public double downDelay = 0.2;
		public double upDelay = 0.1;

		[HideInInspector]
		public static SelectionManager instance;

		private double downTime = 0;
		private double upTime = 0;
		private bool potentialDouble = false;
		private bool hasTriggered = true;
		private Vector3 startPos;
		private Vector3 downPos;
		private bool dragging = false;


		private List<CharacterVisualizer> engaged;
		private List<Selectable> selected;
		private List<Selectable> currentDragSelection;

		void OnEnable() {
			instance = this;
			engaged = new List<CharacterVisualizer> ();
			selected = new List<Selectable> ();
			currentDragSelection = new List<Selectable> ();
		}

		void Update() {

			if (Input.GetKeyDown (KeyCode.Escape)) {
				ClearEngaged ();
			}

			if (dragging)
				UpdateDrag ();
			
			if (Input.GetMouseButtonDown (0)) {
				downPos = Input.mousePosition;
				hasTriggered = false;
				downTime = Time.time;
				if (Time.time - upTime < upDelay) {
					potentialDouble = true;
				}
			}

			if (Input.GetMouseButtonUp (0)) {
				if (dragging)
					StopDrag ();
				
				if (Time.time - downTime < downDelay) {
					if (potentialDouble) {
						TriggerWipe ();
						DoubleClick ();
					} else 
						upTime = Time.time;
				}
			}

			if (Input.GetMouseButton (0) && !hasTriggered) {
				if (Time.time - downTime > downDelay) {
					TriggerWipe ();
					Drag ();
				}
			} else if (!hasTriggered) {
				if (Time.time - upTime > upDelay) {
					TriggerWipe ();
					SingleClick ();
				}
			}
		}

		private void TriggerWipe() {
			hasTriggered = true;
			upTime = 0;
			downTime = 0;
			potentialDouble = false;
		}

		private RaycastHit2D GetHit() {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			return Physics2D.GetRayIntersection(ray,Mathf.Infinity);
		}

		void DoubleClick() {
			RaycastHit2D hit = GetHit();
			if (hit.collider != null) {
				Selectable s = hit.transform.GetComponent<Selectable> ();
				if (s != null) {
					if (!IsSelected (s)) {
						SetSelected (s);
					}
					DefaultAction ();
				}
			} else {
				Debug.Log ("goto position");
				// goto
				GoTo (hit.point);
			}
		}

		void SingleClick() {
			RaycastHit2D hit = GetHit();
			if (hit.collider != null) {
				Selectable s = hit.transform.GetComponent<Selectable> ();
				if (s != null) {
					if (Input.GetKey (KeyCode.LeftShift)) {
						AddSelected (s);
					} else {
						SetSelected (s);
					}
				}
			} else {
				// deselect
				ClearSelected();
			}
		}

		void Drag() {
			// begin drag select
			StartDrag();
		}

		private void StartDrag() {
			if (!Input.GetKey (KeyCode.LeftShift)) {
				ClearSelected ();
			}
			startPos = downPos;
			dragSelector.position = startPos;
			dragSelector.sizeDelta = Vector2.zero;
			dragSelector.gameObject.SetActive (true);
			dragging = true;
		}

		private void UpdateDrag() {
			//draw rect between start and cur pos
			Vector3 cur = Input.mousePosition;
			Vector3 pos = dragSelector.position;

			float width = cur.x - startPos.x;
			if (cur.x < startPos.x) {
				pos.x = cur.x;
				width *= -1;
			} else {
				pos.x = startPos.x;
			}
			float height = cur.y - startPos.y;
			if (cur.y > startPos.y) {
				pos.y = cur.y;
			} else {
				pos.y = startPos.y;
				height *= -1;
			}
			dragSelector.position = pos;
			dragSelector.sizeDelta = new Vector2 (width, height);

			foreach (Selectable s in currentDragSelection) {
				s.SetHighlighted (false);
			}
			currentDragSelection.Clear ();

			Vector3 pt1 = Camera.main.ScreenToWorldPoint (dragSelector.position + new Vector3 (0, 0, 30));
			Vector3 pt2 = Camera.main.ScreenToWorldPoint (dragSelector.position + new Vector3 (width, -1 * height, 30));

			Collider2D[] colliders = Physics2D.OverlapAreaAll (pt1, pt2);
			foreach (Collider2D c in colliders) {
				Selectable s = c.GetComponent<Selectable> ();
				if (s && s.IsSelectable()) {
					s.SetHighlighted (true);
					currentDragSelection.Add (s);
				}
			}
		}

		private void StopDrag() {
			dragging = false;
			dragSelector.sizeDelta = Vector2.zero;
			dragSelector.gameObject.SetActive (false);

			foreach (Selectable s in currentDragSelection) {
				if (!selected.Contains (s))
					AddSelected (s);
			}
			currentDragSelection.Clear ();
		}

		private void DefaultAction() {
			if (IsEngaged()) {
				Debug.Log ("Action");
			} else {
				// engage attempt
				Debug.Log ("Engage");
				foreach (Selectable s in selected) {
					CharacterVisualizer cv = s.GetComponent<CharacterVisualizer> ();
					if (cv != null && cv.CanEngage ()) {
						AddEngaged (cv);
					}
				}
			}
			ClearSelected ();
		}

		private void GoTo(Vector2 point) {
			Debug.Log ("GoTo");
		}

		public void SetSelected(Selectable s) {
			ClearSelected ();
			if (s.IsSelectable ()) {
				s.SetHighlighted (true);
				selected.Add (s);
			}
		}

		public void AddSelected(Selectable s) {
			if (s.IsSelectable ()) {
				s.SetHighlighted (true);
				selected.Add (s);
			}
		}

		public void RemoveSelected(Selectable s) {
			s.SetHighlighted (false);
			selected.Remove (s);
		}

		public void ClearSelected() {
			foreach (Selectable s in selected) {
				s.SetHighlighted (false);
			}
			selected.Clear ();
		}

		public bool IsSelected(Selectable s) {
			return selected.Contains (s);
		}

		public void AddEngaged(CharacterVisualizer cv) {
			cv.GetComponent<Selectable>().SetEngaged (true);
			engaged.Add (cv);
		}

		public bool IsEngaged() {
			return engaged.Count != 0;
		}

		public void ClearEngaged() {
			foreach (CharacterVisualizer cv in engaged) {
				cv.GetComponent<Selectable>().SetEngaged (false);
			}
			engaged.Clear ();
		}

	}

}