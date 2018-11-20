using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.PureObject;

namespace PureActive.Network.Devices.PureObject
{
    public abstract class PureObjectBase : PureLoggableBase<PureObjectBase>, IPureObject, IEquatable<PureObjectBase>
    {
        protected static readonly ulong ObjectVersionStart = 1;
        protected static readonly ulong ObjectVersionDefaultIncrement = 1;

        protected PureObjectBase(IPureLoggerFactory loggerFactory, IPureLogger logger = null) :
            base(loggerFactory, logger)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            CreatedTimestamp = ModifiedTimestamp = DateTimeOffset.Now;
            ObjectId = Guid.NewGuid();
            ObjectVersion = ObjectVersionStart;

            Logger = logger ?? LoggerFactory.CreatePureLogger<PureObjectBase>();
        }

        public bool Equals(PureObjectBase other)
        {
            return other != null &&
                   ObjectId.Equals(other.ObjectId) &&
                   CreatedTimestamp == other.CreatedTimestamp &&
                   ModifiedTimestamp == other.ModifiedTimestamp &&
                   ObjectVersion == other.ObjectVersion;
        }

        public Guid ObjectId { get; set; }

        public DateTimeOffset CreatedTimestamp { get; set; }

        public DateTimeOffset ModifiedTimestamp { get; set; }

        public ulong ObjectVersion { get; set; }

        public bool IsSameObjectId(IPureObject objectOther) =>
            objectOther != null && ObjectId.Equals(objectOther.ObjectId);

        public bool IsSameObjectVersion(IPureObject objectOther) =>
            objectOther != null && ObjectVersion == objectOther.ObjectVersion;

        public virtual IPureObject CopyInstance()
        {
            return MemberwiseClone() as IPureObject;
        }

        public virtual IPureObject CloneInstance()
        {
            var objectClone = (PureObjectBase) MemberwiseClone();

            // Establishes new ObjectId, CreatedTimestamp and ModifiedTimestamp
            objectClone.ObjectId = Guid.NewGuid();
            CreatedTimestamp = ModifiedTimestamp = DateTimeOffset.Now;

            return objectClone;
        }

        public virtual IPureObject UpdateInstance(IPureObject objectUpdate)
        {
            if (ObjectId.Equals(objectUpdate.ObjectId))
            {
                if (objectUpdate.ObjectVersion > ObjectVersion)
                {
                    Logger?.LogTrace("IPureObject:UpdateInstance: ");
                    ObjectVersion = objectUpdate.ObjectVersion;
                }

                IncrementObjectVersion();
                ModifiedTimestamp = DateTimeOffset.Now;
            }
            else
            {
                Logger?.LogDebug("UpdateInstance called when {CurObjectId} != {UpdateObjectId}", ObjectId,
                    objectUpdate.ObjectId);
            }

            return this;
        }

        public virtual int CompareTo(IPureObject other)
        {
            return other == null ? 1 : ObjectId.CompareTo(other.ObjectId);
        }


        public override IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel,
            LoggableFormat loggableFormat)
        {
            var logPropertyLevels = loggableFormat.IsWithParents()
                ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
                : new List<IPureLogPropertyLevel>();

            if (logLevel <= LogLevel.Information)
            {
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(ObjectId), ObjectId, LogLevel.Information));
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(ObjectVersion), ObjectVersion,
                    LogLevel.Information));
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(CreatedTimestamp), CreatedTimestamp,
                    LogLevel.Information));
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(ModifiedTimestamp), ModifiedTimestamp,
                    LogLevel.Information));
            }

            return logPropertyLevels?.Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        }

        protected ulong IncrementObjectVersion() => IncrementObjectVersion(ObjectVersionDefaultIncrement);
        protected ulong IncrementObjectVersion(ulong incAmt) => ObjectVersion = ObjectVersion + incAmt;

        public override bool Equals(object obj)
        {
            return Equals(obj as PureObjectBase);
        }

        public override int GetHashCode()
        {
            var hashCode = 1614018472;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(ObjectId);
            hashCode = hashCode * -1521134295 + CreatedTimestamp.GetHashCode();
            hashCode = hashCode * -1521134295 + ModifiedTimestamp.GetHashCode();
            hashCode = hashCode * -1521134295 + ObjectVersion.GetHashCode();
            return hashCode;
        }
    }
}