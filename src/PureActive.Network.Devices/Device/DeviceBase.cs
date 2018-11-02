using System;
using PureActive.Hosting.Abstractions.System;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.Device;
using PureActive.Network.Abstractions.PureObject;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.PureObject;

namespace PureActive.Network.Devices.Device
{
    public abstract class DeviceBase : PureObjectBase, IDevice
    {
        public ICommonServices CommonServices { get; }
        public DeviceType DeviceType { get; set; }
        
        protected DeviceBase(ICommonServices commonServices, DeviceType deviceType = DeviceType.UnknownDevice, IPureLogger logger = null) : 
            base(commonServices?.LoggerFactory, logger)
        {
            DeviceType = deviceType;
            CommonServices = commonServices;
        }

        public override int CompareTo(IPureObject other)
        {
            if (!(other is DeviceBase))
                throw new ArgumentException("Object must be of type DeviceBase.");

            return CompareTo((DeviceBase)other);
        }

        public int CompareTo(DeviceBase other)
        {
            if (other == null) return 1;

            if (ObjectId.Equals(other.ObjectId))
                return 0;

            return DeviceType.CompareTo(other.DeviceType);
        }

        // TODO: ILogPropertyLevel
        //public override IEnumerable<ILogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel, LoggableFormat loggableFormat)
        //{
        //    var logPropertyLevels = loggableFormat.IsWithParents()
        //        ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
        //        : new List<ILogPropertyLevel>();

        //    if (logLevel <= LogLevel.Information)
        //    {
        //        logPropertyLevels?.Add(new LogPropertyLevel(nameof(DeviceType), DeviceType, LogLevel.Information));
        //    }

        //    return logPropertyLevels;
        //}
    }
}
