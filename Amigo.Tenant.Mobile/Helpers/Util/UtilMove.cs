using System.Text.RegularExpressions;
using XPO.ShuttleTracking.Mobile.Common.Constants;

namespace XPO.ShuttleTracking.Mobile.Helpers.Util
{
    public class UtilMove
    {
        public const int MINLENGTH = 8;
        private const string pipe = "|";
        public bool isShipmentIdValid(string request)
        {
            string[] limit;
           switch (validateMessageShipmentId(request, out limit))
            {
                case ShipmentCode.Ok: return true;
                default: return false;
            }
        }

        public ShipmentCode validateMessageShipmentId(string request, out string[] limits)
        {
            var regex = @Parameters.Get(ParameterCode.ShipmentNumFormat, RegularExpression.ShipmentIDFormat);
            var checkDigitRegex = new Regex(regex);
            string[] limit = {"0","9"};
            if (regex.Contains(pipe))
            {
                var start = regex.IndexOf('(');
                limit = regex.Substring(start + 1, 3).Split(pipe.ToCharArray()[0]);
            }
            limits = limit;
            if (checkDigitRegex.IsMatch(request))
                return ShipmentCode.Ok;
            else
            {
                if (request.Length > MINLENGTH)
                    return ShipmentCode.TooLarge;
                else if (request.Length < MINLENGTH)
                    return ShipmentCode.TooShort;
                else
                {
                    if (!new Regex(RegularExpression.OnlyNumber).IsMatch(request))
                        return ShipmentCode.WithABC;
                    if (!request.StartsWith(limit[0]) && !request.StartsWith(limit[1]))
                        return ShipmentCode.BadInitNumbers;
                }
            }
            return ShipmentCode.Ok;
        }
    }
    public enum ShipmentCode
    {
        TooShort,
        TooLarge,
        WithABC,
        BadInitNumbers,
        Ok
    }
}
