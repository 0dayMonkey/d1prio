using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Reflection;

namespace FloorServer.Client.Boss
{
    /// <summary>
    /// Class handling renaming of clients.
    /// </summary>
    public class ClientName
    {
        #region Private members

        private readonly ILogger<ClientName> _logger;
        private static readonly string UNKNOWN = "unknown";

        private readonly BossClient _client;
        private readonly BomFactory _bomFactory;
        private string _newName = String.Empty;
        private string _assignedName = String.Empty;

        #endregion

        #region Constructors

        internal ClientName(BossClient client, BomFactory bomFactory, ILoggerFactory loggerFactory)
        {
            this._client = client;
            _logger = loggerFactory.CreateLogger<ClientName>();
            _bomFactory = bomFactory;
        }

        #endregion

        #region Properties

        internal string Assigned
        {
            get { return _assignedName; }

            private set {
                if (value == _assignedName)
                    return;

                _assignedName = value;
                _logger.LogInformation("BOSS assigned client name [{0}]", _assignedName);
                _client.RaiseBossStatusChangeEvent(new BossStatusEventArgs(BossEventType.NewName, value));
            }
        }

        private bool AlreadyConnected
        {
            get { return Assigned != String.Empty; }
        }

        #endregion

        /// <summary>
        /// Checks if the received message is a logon message. If this is the case
        /// set client name provided by the logon message and if needed send rename 
        /// request to BOSS.
        /// </summary>
        /// <remarks>
        /// If the provided client name does not pipe the desired clientname
        /// (provided by the constructor) send a rename request to the BOSS.
        /// </remarks>
        /// <param name="sender">The BOSS client</param>
        /// <param name="args">Message received </param>
        internal void ConsumeLogonMessage(object sender, BomEventArgs args)
        {
            if (IsNotLogonMessage(args.Message))
            {
                return;
            }

            string dst = args.Message.GetString(Tags.DST);

            if (NameChangeRejected(dst))
            {
                _client.RaiseBossStatusChangeEvent(new BossStatusEventArgs(BossEventType.NameChangeRejected, dst));
                return;
            }

            if (NamelessClient())
            {
                _newName = dst;
            }

            Assigned = dst;

            if(RenameRequired())
            {
            	SendNameChangeRequest(sender);
            }
        }

        private bool IsNotLogonMessage(Bom message)
        {
            return message.GetString(Tags.TYP) != "1";
        }

        private bool NameChangeRejected(string dst)
        {
            return dst == Assigned;
        }

        private bool NamelessClient()
        {
            return _newName == String.Empty;
        }

        private bool RenameRequired()
        {
            return Assigned != _newName;
        }

        private void SendNameChangeRequest(object sender)
        {
            var remote = (BossClient)sender;
            Bom renameRequest = CreateRenameRequest();
            _logger.LogInformation("Sending request to rename client from [{0}] to [{1}]", Assigned, _newName);
            remote.Write(renameRequest.ToString());
        }


        private Bom CreateRenameRequest()
        {
            var renameRequest = _bomFactory.CreateBom();
            renameRequest.PutString(Tags.DST, Tags.BOSS);
            renameRequest.PutString(Tags.TYP, "2");
            renameRequest.PutString(Tags.MYNAME, _newName);

            Bom details = ProvideAssemblyDetails();
            renameRequest.PutMsg(Tags.DETAILS, details);

            return renameRequest;
        }

        private Bom ProvideAssemblyDetails()
        {
            var details = _bomFactory.CreateBom();

            string name = UNKNOWN;
            string version = UNKNOWN;
            string build = UNKNOWN;

            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                name = assembly.GetName().Name;
				Version v = GetAssemblyVersion(assembly);
				if (v != null)
				{
					version = String.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build);
					build = v.Revision.ToString(CultureInfo.InvariantCulture);
				}
            }
            details.PutString(Tags.APPLICATION, name);
            details.PutString(Tags.VERSION, version);
            details.PutString(Tags.BUILD, build);
            return details;
        }

		private static Version GetAssemblyVersion(Assembly assembly)
		{
			if (assembly == null)
			{
				return null;
			}

			var customAttributes =
				assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);

			if (customAttributes.Length == 1)
			{
				var fileVersionAttribute = customAttributes[0] as AssemblyFileVersionAttribute;
				if(fileVersionAttribute != null)
				{
					return new Version(fileVersionAttribute.Version);
				}
			}

			return assembly.GetName().Version;
		}

        /// <summary>
        /// Send rename request to BOSS
        /// </summary>
        /// <param name="remote">Remote host to which we send the request.</param>
        /// <param name="name">New name.</param>
        internal void RequestNameChange(BossClient remote, string name)
        {
            if (name == null)
                throw new ArgumentNullException();

            if (String.Empty == name)
                throw new ArgumentException();

            _newName = name;
            if (AlreadyConnected)
                SendNameChangeRequest(remote);
        }

        /// <summary>
        /// Resests client name to String.Empty.
        /// </summary>
        internal void Reset()
        {
            _assignedName = String.Empty;
        }

    }
}
