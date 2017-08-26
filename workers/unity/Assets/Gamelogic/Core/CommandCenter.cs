﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Configuration;
using Improbable.Unity.Core;
using Improbable.Unity.Core.EntityQueries;
using Improbable;
using Improbable.Core;
using Improbable.Entity.Component;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using Improbable.Worker.Query;
using Improbable.Worker;
using Improbable.Entity;
using Improbable.Unity.Core.EntityQueries;

namespace Assets.Gamelogic.Core {

	public class CommandCenter : MonoBehaviour {

		private static  List<EntityId> agents;
		private static RaycastHit2D hit;
		private static Vector3 position;
		private static float radius;
		private static GameObject target;
		private static bool radial;

		// called from selection manager
		public static void InterpretRadialCommand(List<Selectable> s, Vector3 p, float r) {
			agents = ParseControllableEntities (s);
			if (agents.Count == 0)
				return;
			
			radius = r;
			position = p;
			radial = true;

			List<string> options = new List<string> ();
			Collider2D[] colliders = Physics2D.OverlapCircleAll (p, r);
			foreach (Collider2D c in colliders) {
				ParseOptions (ref options, c.gameObject);
			}
			if (options.Count == 1) {
				OnCommandSelected (options.ToArray () [0]);
			} else {
				UIManager.OpenCommandPicker (options);
			}
		}

		// called from selection manager
		public static void InterpretClickCommand(List<Selectable> s, RaycastHit2D h, Vector3 p) {
			agents = ParseControllableEntities (s);
			if (agents.Count == 0)
				return;

			hit = h;
			position = p;
			radial = false;

			List<string> options = new List<string> ();
			if (hit.collider != null) {
				target = hit.collider.gameObject;
				ParseOptions (ref options, target);
			} else {
				// prematurely assume walk
				ExecutePositionTargetedCommand ("goto");
				return;
			}
			if (options.Count == 1) {
				OnCommandSelected (options.ToArray () [0]);
			} else {
				UIManager.OpenCommandPicker (options);
			}
		}

		// called from ui picker
		public static void OnCommandSelected(string command) {
			if (radial) {
				ExecuteRadialTargetedCommand (command);
			} if (target != null) {
				ExecuteEntityTargetedCommand (command);
			} else {
				ExecutePositionTargetedCommand (command);
			}
		}

		// called from ui picker
		public static void OnCommandCancelled() {
			hit = new RaycastHit2D();
			position = Vector3.zero;
			agents = null;
			radial = false;
			target = null;
		}

		// depricated
		private static void ExecuteRadialTargetedCommand(string command) {
			foreach (EntityId id in agents) {
				SpatialOS.Commands.SendCommand (PlayerController.instance.playerWriter, Character.Commands.RadiusTarget.Descriptor, new RadiusTargetRequest (new Vector3d (position.x, 0, position.y), radius, command), id);
			}
		}

		private static void ExecuteEntityTargetedCommand(string command) {
			if (command == "gather")
				ExecuteGatherableTargetedCommand ();
			else {
				foreach (EntityId id in agents) {
					SpatialOS.Commands.SendCommand (PlayerController.instance.playerWriter, Character.Commands.EntityTarget.Descriptor, new EntityTargetRequest (target.EntityId(), command), id);
				}
			}
		}

		private static void ExecuteGatherableTargetedCommand() {
			string command = "gather";
			if (agents.Count < 1)
				return;
			
			SpatialOS.Commands.SendCommand (PlayerController.instance.playerWriter, Character.Commands.EntityTarget.Descriptor, new EntityTargetRequest (target.EntityId(), command), agents[0]);
			agents.RemoveAt (0);

			if (agents.Count < 1)
				return;

			Collider2D[] cols = Physics2D.OverlapCircleAll (position, UIManager.instance.coOpRadius);
			WorkType t = target.GetComponent<GatherableVisualizer> ().gatherableReader.Data.workType;
			List<GameObject> used = new List<GameObject> ();
			used.Add (target);

			foreach (Collider2D c in cols) {
				if (used.Contains (c.gameObject))
					continue;
				GatherableVisualizer gatherable = c.gameObject.GetComponent<GatherableVisualizer> ();
				if (gatherable == null)
					continue;
				if (gatherable.gatherableReader.Data.workType == t) {
					used.Add (c.gameObject);
					SpatialOS.Commands.SendCommand (PlayerController.instance.playerWriter, Character.Commands.EntityTarget.Descriptor, new EntityTargetRequest (c.gameObject.EntityId(), command), agents [0]);
					agents.RemoveAt (0);
					if (agents.Count < 1)
						return;
				}
			}

		}

		private static void ExecutePositionTargetedCommand(string command) {
			foreach (EntityId id in agents) {
				SpatialOS.Commands.SendCommand (PlayerController.instance.playerWriter, Character.Commands.PositionTarget.Descriptor, new PositionTargetRequest (new Vector3d (position.x, 0, position.y), command), id);
			}
		}

		private static List<EntityId> ParseControllableEntities(List<Selectable> selected) {
			List<EntityId> ids = new List<EntityId>();
			foreach (Selectable s in selected) {
				CharacterVisualizer cv = s.GetComponent<CharacterVisualizer> ();
				if (cv != null && cv.CanControl ()) {
					ids.Add (cv.gameObject.EntityId());
				}
			}
			return ids;
		}

		//TODO this is the function the parses options from targets
		private static void ParseOptions(ref List<string> options, GameObject g) {
			GatherableVisualizer gatherable = g.GetComponent<GatherableVisualizer> ();
			if (gatherable != null) {
				//TODO actually add the options here
				if (!options.Contains ("gather"))
					options.Add ("gather");
			}
			WorkSiteVisualizer worksite = g.GetComponent<WorkSiteVisualizer> ();
			if (worksite != null) {
				if (!options.Contains ("work"))
					options.Add ("work");
			}
			StorageVisualizer storage = g.GetComponent<StorageVisualizer> ();
			if (storage != null) {
				if (!options.Contains ("store"))
					options.Add ("store");
			}
		}
			
	}

}