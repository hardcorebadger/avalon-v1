using Improbable.Worker;

namespace Improbable.Unity.Core
{
    /// <summary>
    /// Object that can receive authority change notifications.
    /// </summary>
    public interface IAuthorityChangedReceiver
    {
        /// <summary>
        /// Called when authority over a component of an entity changes.
        /// </summary>
        void AuthorityChanged(EntityId entityId, IComponentMetaclass componentId, bool hasAuthority, object component);
    }
}