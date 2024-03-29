﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gamelogic.Core {

	public class WallSnapper : MonoBehaviour {

		public GameObject corner;
		public GameObject vertical;
		public GameObject horizontal;
		public bool endcap = true;
		public int snapId = 0;
		public string debug = "";

		private int gridX;
		private int gridZ;

		void OnEnable() {
			StartCoroutine(Refresh (true));
		}

		public IEnumerator Refresh(bool onStart) {

			yield return null;


			gridX = Mathf.RoundToInt (transform.position.x);
			gridZ = Mathf.RoundToInt (transform.position.z);

			Collider[] cols = Physics.OverlapBox (transform.position, new Vector3 (8, 8, 8));
			bool horiz = false;
			bool vert = false;
			int count = 0;
			foreach (Collider c in cols) {
				WallSnapper snap = c.GetComponent<WallSnapper> ();

				if (snap == null || snap == this)
					continue;

				if (snap.snapId != snapId)
					continue;

				bool h = false;
				bool v = false;

				if (IsLeft (snap)) {
					h = true;
					count++;
				}
				if (IsRight (snap)) {
					h = true;
					count++;
				}
				if (IsUp (snap)) {
					v = true;
					count++;
				}
				if (IsDown (snap)) {
					v = true;
					count++;
				}

				if ((h || v) && onStart) {
					snap.StartCoroutine(snap.Refresh (false));
				}

				if (h)
					horiz = true;
				if (v)
					vert = true;
			}

			if ((endcap && count < 2) || (horiz && vert)) {
				corner.SetActive (true);
				vertical.SetActive (false);
				horizontal.SetActive (false);
			} else if (vert) {
				vertical.SetActive (true);
				corner.SetActive (false);
				horizontal.SetActive (false);
			} else if (horiz) {
				horizontal.SetActive (true);
				vertical.SetActive (false);
				corner.SetActive (false);
			} else {
				corner.SetActive (true);
				vertical.SetActive (false);
				horizontal.SetActive (false);
			}

		}

		private bool IsLeft(WallSnapper s) {
			int x = Mathf.RoundToInt(s.transform.position.x);
			int z = Mathf.RoundToInt(s.transform.position.z);

			return IsOffset (-8, 0, x, z);
		}

		private bool IsRight(WallSnapper s) {
			int x = Mathf.RoundToInt(s.transform.position.x);
			int z = Mathf.RoundToInt(s.transform.position.z);

			return IsOffset (8, 0, x, z);
		}

		private bool IsUp(WallSnapper s) {
			int x = Mathf.RoundToInt(s.transform.position.x);
			int z = Mathf.RoundToInt(s.transform.position.z);

			return IsOffset (0, 8, x, z);
		}

		private bool IsDown(WallSnapper s) {
			int x = Mathf.RoundToInt(s.transform.position.x);
			int z = Mathf.RoundToInt(s.transform.position.z);

			return IsOffset (0, -8, x, z);
		}

		private bool IsOffset(int xOff, int zOff, int x, int z) {
			return (x == gridX + xOff && z == gridZ + zOff);
		}

	}

}