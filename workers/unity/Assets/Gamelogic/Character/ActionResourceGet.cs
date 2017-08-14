﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
using Improbable.Collections;

namespace Assets.Gamelogic.Core {

	// Policy: Failed = actual query failures etc, Success = using the options presented and my inv space, I got what I could
	public class ActionResourceGet : Action {

		private SourcingOption sourcing;
		private int state = 0;
		private bool failed = false;
		private bool success = false;
		private Dictionary<int,int> toGet;
		private Dictionary<int,int> canGet;
		private Dictionary<int,int> getting;
		private Vector3 targetPosition;
		private InventoryData targetInv;
		private EntityId target;
		private Action subAction;

		public ActionResourceGet(CharacterController o, SourcingOption s, Dictionary<int,int> tg) : base(o)	{
			sourcing = s;
			toGet = tg;
		}

		public override ActionCode Update () {
			if (state == 0) {
				// before actually sourcing, see if there's at least one thing we could actually fit into our inventory
				bool flag = false;
				foreach (int id in toGet.Keys) {
					if (owner.inventory.CanHold (id, 1)) {
						flag = true;
						break;
					}
				}

				// if not, just success
				if (!flag)
					return ActionCode.Success;
				else
					state = 1;

			} else if (sourcing.radius) {
				switch (state) {
				case 1:
					// call a query for storage in the area
					var entityQuery = Query.And (Query.HasComponent<Storage> (), Query.InSphere (sourcing.storageRangeCenter.x, sourcing.storageRangeCenter.y, sourcing.storageRangeCenter.z, sourcing.storageRadius)).ReturnComponents (Position.ComponentId, Inventory.ComponentId);
					SpatialOS.WorkerCommands.SendQuery (entityQuery)
						.OnSuccess (OnSuccessfulEntityQuery)
						.OnFailure (OnFailedEntityQuery);
					state = 2;
					break;
				case 2:
					//waiting... (processes request on return)
					break;
				case 3:
					// found a storage to go to, walking there...
					ActionCode seekProgress = subAction.Update ();
					if (seekProgress == ActionCode.Success) {
						state = 4;
					} if (seekProgress == ActionCode.Failure)
						failed = true;
					break;
				case 4:
					// take as much shit out of the storage as you can that fits requirements, set state back to 0
					getting = InventoryController.GetCappedOverlap (targetInv.inventory, toGet, owner.inventory.GetAvailableWeight ());
					if (getting.Count > 0) {
						SpatialOS.Commands.SendCommand (owner.characterWriter, Inventory.Commands.TakeMultiple.Descriptor, InventoryController.ToItemStackList(getting), target)
							.OnSuccess(response => OnTakeResult(response))
							.OnFailure(response => OnRequestFailed());
						state = 5;
					} else {
						// sadly, someone took the shit this guy was trying to go get and now he has to start over...
						state = 0;
					}
					break;
				case 5:
					// waiting...
					break;
				}
			} else {
				switch (state) {
				case 1:
					break;
				}
			}

			if (failed)
				return ActionCode.Failure;
			else if (success)
				return ActionCode.Success;
			else
				return ActionCode.Working;
		}

		private void OnSuccessfulEntityQuery(EntityQueryResult queryResult) {
			Map<EntityId, Entity> resultMap = queryResult.Entities;
			foreach (EntityId id in resultMap.Keys) {
				Entity e = resultMap[id];
				Improbable.Collections.Option<IComponentData<Inventory>> i = e.Get<Inventory>();
				targetInv = i.Value.Get().Value;

				getting = InventoryController.GetCappedOverlap (targetInv.inventory, toGet, owner.inventory.GetAvailableWeight());
				if (getting.Count > 0) {
					Improbable.Collections.Option<IComponentData<Position>> p = e.Get<Position>();
					targetPosition = p.Value.Get().Value.coords.ToVector3();
					target = id;
					state = 3;
					subAction = new ActionSeek (owner, targetPosition);
					return;
				}
			}
			success = true;
		}

		private void OnFailedEntityQuery(ICommandErrorDetails _) {
			failed = true;
		}

		private void OnTakeResult(TakeResponse response) {
			if (response.success) {
				if (!owner.inventory.Insert (getting))
					failed = true;
				else
					state = 0;
			} else {
				// kinda weird, we should've been able to get it...
				state = 0;
			}
		}

		private void OnRequestFailed() {
			failed = true;
		}
	}

}