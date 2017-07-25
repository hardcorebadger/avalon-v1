// Copyright (c) Improbable Worlds Ltd, All Rights Reserved
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// =========== 

using System;

namespace Improbable.Unity.Configuration
{
    /// <summary>
    /// Configuration options that are available for the users to edit from within Unity.
    /// </summary>
    public static class EditableConfigNames
    {
        public const string AssemblyName = "assemblyName";
        public const string DeploymentId = "deploymentId";
        public const string DeploymentTag = "deploymentTag";

        [Obsolete("Please use WorkerId instead.")]
        public const string EngineId = "engineId";
        [Obsolete("Please use WorkerType instead.")]
        public const string EngineType = "engineType";

        public const string EntityCreationLimitPerFrame = "entityCreationLimitPerFrame";
        public const string InfraServiceUrl = "infraServicesUrl";
        public const string LinkProtocol = "linkProtocol";
        public const string LogDebugToSpatialOs = "logDebugToSpatialOs";
        public const string LogAssertToSpatialOs = "logAssertToSpatialOs";
        public const string LogWarningToSpatialOs = "logWarningToSpatialOs";
        public const string LogErrorToSpatialOs = "logErrorToSpatialOs";
        public const string LogExceptionToSpatialOs = "logExceptionToSpatialOs";
        public const string LoginToken = "loginToken";
        public const string TcpMultiplexLevel = "tcpMultiplexLevel";
        public const string ProtocolLogPrefix = "protocolLogPrefix";
        public const string ProtocolLoggingOnStartup = "protocolLoggingOnStartup";
        public const string ProtocolLogMaxFileBytes = "protocolLogMaxFileBytes";
        public const string RaknetHeartbeatTimeoutMillis = "raknetHeartbeatTimeoutMs";
        public const string ReceptionistPort = "receptionistPort";
        public const string ReceptionistIp = "receptionistIp";
        public const string ShowDebugTraces = "showDebugTraces";
        public const string SteamToken = "steamToken";
        public const string UsePrefabPooling = "usePrefabPooling";
        public const string UseInstrumentation = "useInstrumentation";
        public const string UsePerInstanceAssetCache = "usePerInstanceAssetCache";
        public const string WorkerId = "workerId";
        public const string WorkerType = "workerType";
    }
}
