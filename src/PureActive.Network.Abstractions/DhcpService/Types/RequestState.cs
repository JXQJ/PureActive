namespace PureActive.Network.Abstractions.DhcpService.Types
{
    public enum RequestState
    {
        Unknown = 0,
        Selecting,
        Renewing,
        Rebinding,
        InitReboot,
        Duplicate
    }

    public static class RequestStateString
    {
        public static string GetName(RequestState requestStateEnum)
        {
            switch (requestStateEnum)
            {
                case RequestState.Selecting:
                    return "SELECTING";
                case RequestState.Renewing:
                    return "RENEWING";
                case RequestState.Rebinding:
                    return "REBINDING";
                case RequestState.InitReboot:
                    return "INIT-REBOOT";
                case RequestState.Duplicate:
                    return "DUPLICATE";
                default:
                    return "UNKNOWN";
            }
        }
    }
}