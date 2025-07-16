#region

using System;
using FloorServer.Client.Configuration;

#endregion

namespace FloorServer.Client.Tools.Validators
{
    public class FloorServerConfigurationValidator
    {

        #region IElementValidator<FloorServerConfiguration> Members

        public bool Validate(FloorServerConfiguration entry)
        {
            //if (entry == null)
            //{
            //    LogManager.GetLogger("FloorServerLogger").LogError("entry parameter cannot be null.");
            //    throw new ArgumentNullException("entry");
            //}

            //if (string.IsNullOrEmpty(entry.ClientName))
            //{
            //    ErrorMessage = "Invalid client name.";
            //    return false;
            //}

            //if (entry.EndPoint == null)
            //{
            //    ErrorMessage = "EndPoint not properly defined.";
            //    return false;
            //}

            //var ipValidator = new IPV4Validator();

            //if (!ipValidator.Validate(entry.EndPoint.Address.ToString()))
            //{
            //    ErrorMessage = ipValidator.ErrorMessage;
            //    return false;
            //}

            return true;
        }

        #endregion
    }
}
