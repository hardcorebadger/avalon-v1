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

	public class ActionGather : Action {

		public EntityId target;
		public GatherableData gatherable;
		public Vector3 position;
		private int state = -1;
		public bool failed = false;
		private bool success = false;
		public bool gatherableNotFound = false;

		public ActionSeek seek;

		private float time = -1f;


		public ActionGather(CharacterController o, EntityId t) : base(o)	{
			target = t;
		}

		public override void Log() {
			seek.Log ();
		}

		public override ActionCode Update () {

			switch (state) {
			case -1:
				if (!owner.EmptyHanded ())
					success = true;
				state = 0;
				break;
			case 0:
				var entityQuery = Query.HasEntityId(target).ReturnComponents(Position.ComponentId, Gatherable.ComponentId);
				SpatialOS.WorkerCommands.SendQuery(entityQuery)
					.OnSuccess(OnSuccessfulEntityQuery)
					.OnFailure(OnFailedEntityQuery);
				state = 1;
				break;
			case 1:
				// waiting on query
				break;
			case 2:
				// lets go over there
				if (seek == null) {
					seek = new ActionSeek (owner, target, position);
				}

				ActionCode seekProgress = seek.Update ();
				if (seekProgress == ActionCode.Success) {
					state = 3;
					owner.SetState (CharacterState.CHOPPING);
					time = 0f;
				}
				break;
			case 3:
				// we're there! Lets do this bitch
				time+= Time.deltaTime;
				if (time >= gatherable.strength) {
					//successfully chopped, send gather request
					SpatialOS.Commands.SendCommand (owner.characterWriter, Gatherable.Commands.RequestGather.Descriptor , new GatherRequest(), target)
						.OnSuccess(response => OnGatherResponse(response))
						.OnFailure(response => OnGatherFailed());
					state = 4;
				}
				break;
			case 4:
				// waiting - see callback
				break;
			}

			if (success)
				return ActionCode.Success;
			else if (failed)
				return ActionCode.Failure;
			else
				return ActionCode.Perpetual;

		}

		private void OnSuccessfulEntityQuery(EntityQueryResult queryResult) {
			Map<EntityId, Entity> resultMap = queryResult.Entities;
			if (resultMap.Count == 0) {
				Debug.Log ("ish: gatherable no longer exits");
				gatherableNotFound = true;
				success = true;
				return;
			}
			Entity e = resultMap.First.Value.Value;
			Improbable.Collections.Option<IComponentData<Position>> p = e.Get<Position>();
			Improbable.Collections.Option<IComponentData<Gatherable>> g = e.Get<Gatherable>();
			gatherable = g.Value.Get().Value;
			position = p.Value.Get().Value.coords.ToVector3();
			state = 2;
		}

		private void OnFailedEntityQuery(ICommandErrorDetails _) {
			failed = true;
		}

		public void OnGatherResponse(GatherResponse response) {
			//we got the gather response
			if (response.success) {
				int id = response.items.id;
				owner.SetInHandItem(id);
				success = true;
			} else {
				//gatherable said no!
				failed = true;
			}
			owner.SetState (CharacterState.DEFAULT);
		}

		public void OnGatherFailed() {
			Debug.Log ("gather fail fix happened");
			owner.SetState (CharacterState.DEFAULT);
			failed = true;
		}

	}
}