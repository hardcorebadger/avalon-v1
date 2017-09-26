﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Gamelogic.Core {
	
	public class UIPreviewWidget : MonoBehaviour {

		public string type;
		public string title;
		protected GameObject targetObject;

		public virtual void Load(UIPreviewWindow window, GameObject target) {
			window.title.text = title;
			targetObject = target;
		}

	}

}