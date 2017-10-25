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


namespace Assets.Gamelogic.Core {

	public class CharacterController : MonoBehaviour {

		[Require] public Character.Writer characterWriter;
		[Require] public Position.Writer positionWriter;
		[Require] public Rotation.Writer rotationWriter;

		[HideInInspector]
		public Rigidbody rigidBody;

		public float speed = 5f;
		public float range = 5f;
		public float maxRotation = 60f;
		public float interpolation = 1f;
		public float width = 1f;
		public float approachRadius = 4f;
		public float arrivalRadius = 2f;
		public Quaternion facing = Quaternion.identity;
		public Animator anim;
		public OwnedController owned;
		public bool indoors = false;


		private ActionQueue actionQueue;
		private AIAction currentAction;
		private float velocity;
		public CharacterState state;
		private int itemInHand = -1;
		public float health;
		public float hunger;
		public Improbable.Collections.Option<EntityId> district;

		private void OnEnable() {
			anim = GetComponent<Animator> ();
			owned = GetComponent<OwnedController> ();

			characterWriter.CommandReceiver.OnPositionTarget.RegisterResponse(OnPositionTarget);
			characterWriter.CommandReceiver.OnEntityTarget.RegisterResponse(OnEntityTarget);

			characterWriter.CommandReceiver.OnFire.RegisterResponse(OnFire);
			characterWriter.CommandReceiver.OnReceiveHit.RegisterResponse(OnReceiveHit);
			characterWriter.CommandReceiver.OnHostileAlert.RegisterResponse(OnHostileAlert);
			characterWriter.CommandReceiver.OnSetDistrict.RegisterResponse(OnSetDistrict);
			transform.position = positionWriter.Data.coords.ToVector3();
			transform.eulerAngles = new Vector3 (0, 0, rotationWriter.Data.rotation);
			state = characterWriter.Data.state;
			itemInHand = characterWriter.Data.itemInHand;
			health = characterWriter.Data.health;
			hunger = characterWriter.Data.hunger; 
			StartCoroutine ("UpdateTransform");

			rigidBody = GetComponent<Rigidbody> ();
			district = characterWriter.Data.district;
		
			indoors = characterWriter.Data.isIndoors;
		}

		private void OnDisable() {
			characterWriter.CommandReceiver.OnPositionTarget.DeregisterResponse();
			characterWriter.CommandReceiver.OnEntityTarget.DeregisterResponse();

			characterWriter.CommandReceiver.OnFire.DeregisterResponse();
			characterWriter.CommandReceiver.OnReceiveHit.DeregisterResponse();
			characterWriter.CommandReceiver.OnHostileAlert.DeregisterResponse();
			characterWriter.CommandReceiver.OnSetDistrict.DeregisterResponse();

		}

		IEnumerator UpdateTransform() {
			while (true) {
				yield return new WaitForSeconds (0.1f);
				positionWriter.Send (new Position.Update ().SetCoords (transform.position.ToCoordinates ()));
				rotationWriter.Send (new Rotation.Update ().SetRotation(facing.eulerAngles.y));
				characterWriter.Send (new Character.Update ().SetVelocity (velocity));
			}
		}

		private void Update() {
			if (health <= 0F)
				DestroyCharacter ();

			UpdateAI ();
		}

		private void UpdateAI() {
			if (currentAction == null)
				return;
			
			if (AIAction.OnTermination (currentAction.Update ()))
				currentAction = actionQueue.Dequeue();
		}

		public void QueueAction(int priority, AIAction a) {
			actionQueue.Enqueue (priority, a);
		}

		public void QueueActionImmediate(AIAction a) {
			if (currentAction != null) {
				currentAction.OnKill ();
				currentAction = a;
			} else {
				currentAction = a;
			}
		}

		private Nothing OnPositionTarget(PositionTargetRequest request, ICommandCallerInfo callerinfo) {
			if (request.command == "goto")
				QueueActionImmediate (new AIActionGoTo (this, new Vector3 ((float)request.targetPosition.x, (float)request.targetPosition.y, (float)request.targetPosition.z)));
			return new Nothing ();
		}

		private Nothing OnEntityTarget(EntityTargetRequest request, ICommandCallerInfo callerinfo) {
			if (request.command == "gather") 
				QueueActionImmediate (new AIActionGather (this, request.target));
//			else if (request.command == "work")
//				QueueActionImmediate (new ActionWork (this, request.target));
			else if (request.command == "attack")
				QueueActionImmediate (new AIActionAttack (this, request.target));
			else if (request.command == "damage")
				QueueActionImmediate (new AIActionDamage (this, request.target));
			return new Nothing ();
		}

		private Nothing OnFire(Nothing request, ICommandCallerInfo callerinfo) {
			actionQueue.CancelAllJobActions ();
			if (currentAction is AIActionJob)
				currentAction.OnKill ();
			currentAction = actionQueue.Dequeue ();
			return new Nothing ();
		}

		private Nothing OnReceiveHit(ReceiveHitRequest request, ICommandCallerInfo callerinfo) {

			Debug.LogWarning ("Received Hit!");
			health -= Random.Range(3.0f, 6.0f);
			characterWriter.Send (new Character.Update ()
				.SetHealth (health)
				.AddShowHurt(new Nothing())
			);
			if (!(currentAction is AIActionAttack) && !(currentAction is AIActionDamage)) {
				QueueActionImmediate (new AIActionAttack (this, request.source));
			}
			Collider[] cols = Physics.OverlapSphere (transform.position, 50);
			System.Collections.Generic.List<CharacterController> enemies = new System.Collections.Generic.List<CharacterController>();
			System.Collections.Generic.List<CharacterController> friends = new System.Collections.Generic.List<CharacterController>();

			for (int x = 0; x < cols.Length; x++) {
				GameObject g = cols [x].gameObject;
				CharacterController c = g.GetComponent<CharacterController> ();
				if (c != null) {
					if (c.characterWriter.Data.playerId == characterWriter.Data.playerId) {
						//my character found 
						friends.Add(c);
					} else if (c.characterWriter.Data.playerId == request.playerId) {
						//other HOSTILE character found
						enemies.Add(c);
					} else {
						//other NEUTRAL/HOSTILE character found
					}
				}

			}
			int i = -1; 
			for (int y = 0; y < friends.Count; y++) {
				i++;
				SpatialOS.Commands.SendCommand (characterWriter, Character.Commands.HostileAlert.Descriptor, new HostileAlertRequest(enemies[i].characterWriter.EntityId), friends[y].characterWriter.EntityId);
				if (i >= (enemies.Count - 1)) {
					i = -1;
				}

			}

			return new Nothing ();
		}

		private Nothing OnHostileAlert(HostileAlertRequest request, ICommandCallerInfo callerinfo) {

			if (!(currentAction is AIActionAttack) && !(currentAction is AIActionDamage)) {
				QueueActionImmediate(new AIActionAttack (this, request.target));
			}

			return new Nothing ();
		}

		private Nothing OnSetDistrict(SetCharacterDistrictRequest request, ICommandCallerInfo callerinfo) {

			district = request.districtId;
			characterWriter.Send (new Character.Update ()
				.SetDistrict (district)
			);


			return new Nothing ();
		}

		public void SetState(CharacterState s) {
			state = s;
			characterWriter.Send (new Character.Update ()
				.SetState (s)
			);
		}

		public void SetVelocity(float f) {
			velocity = f;
			// preserve gravitational force
			rigidBody.velocity = new Vector3(0f, rigidBody.velocity.y, 0f) + (facing * new Vector3 (0, 0, velocity));
		}

		public Vector3 GetFacingDirection() {
			return facing*Vector3.forward;
		}

		public bool HasApplicableItem(ConstructionData c) {
			if (!c.requirements.ContainsKey(itemInHand))
				return false;
			if (c.requirements [itemInHand].required - c.requirements [itemInHand].amount > 0)
				return true;

			return false;
		}

		public void Eat(float amount) {
			hunger -= amount;
			characterWriter.Send (new Character.Update ()
				.SetHunger (hunger)
			);
		}

		public void DropItem() {
			itemInHand = -1;
			characterWriter.Send (new Character.Update ()
				.SetItemInHand (itemInHand)
			);
		}

		public int GetItemInHand() {
			return itemInHand;
		}

		public bool EmptyHanded() {
			return (itemInHand == -1);
		}

		public bool SetInHandItem(int id) {
			if (itemInHand != -1)
				return false;

			itemInHand = id;
			characterWriter.Send (new Character.Update ()
				.SetItemInHand (itemInHand)
			);

			return true;
		}

		public void SetDebugMetadata(int i) {
			characterWriter.Send (new Character.Update ()
				.SetDebugMetadata (i)
			);
		}

		public void OnDealHit() {
//			if (characterWriter != null && characterWriter.HasAuthority &&  currentAction != null) {
////				currentAction.OnDealHit ();
//			}
		}

		public void DestroyCharacter() {
			if (district.HasValue) {
				// deregiste the construction site

				Improbable.Collections.List<EntityId> l = new Improbable.Collections.List<EntityId> ();
				l.Add (gameObject.EntityId());
				SpatialOS.Commands.SendCommand (
					characterWriter, 
					District.Commands.DeregisterCharacter.Descriptor, 
					new CharacterDeregistrationRequest (l), 
				district.Value
				).OnSuccess (OnDeregisteredSelfDistrict);
			} else {
				// settlement construction is not registered, so no deregistration
				OnDeregisteredSelfDistrict (new Nothing ());
			}
		}

		private void OnDeregisteredSelfDistrict(Nothing n) {

			SpatialOS.Commands.SendCommand (
				characterWriter, 
				PlayerOnline.Commands.DeregisterCharacter.Descriptor, 
				new CharacterPlayerDeregisterRequest (gameObject.EntityId()), 
				owned.getOwnerObject()
			).OnSuccess (OnDeregisteredSelf);

		}

		private void OnDeregisteredSelf(Nothing n) {
			// finally delete yourself

			SpatialOS.WorkerCommands.DeleteEntity (gameObject.EntityId ());
		}

		public void SetIndoors(bool b) {
			indoors = b;
			characterWriter.Send (new Character.Update ()
				.SetIsIndoors (indoors)
			);
			if (indoors) {
				GetComponent<Collider> ().enabled = false;
				GetComponent<Rigidbody> ().isKinematic = true;
			} else {
				GetComponent<Collider> ().enabled = true;
				GetComponent<Rigidbody> ().isKinematic = false;
			}
		}

		public void Hit(EntityId other) {
			anim.SetTrigger ("attack");
			characterWriter.Send (new Character.Update ()
				.AddShowHit(new Nothing())
			);
		}

	}

}
