#region

using System.Reflection;
using FloorServer.Client.Boss;
using Microsoft.Extensions.Logging;

#endregion

namespace FloorServer.Client.Communication.Details
{
    public class BOMVersionInfo : Bom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BOMVersionInfo"/> class.
        /// </summary>
        /// <param name="clientName">Name of the client which tries to connect to the floorServer.</param>
        public BOMVersionInfo(string clientName, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<BOMVersionInfo>();
            logger.LogDebug("Building Bom version details ...");
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();

            logger.LogDebug("Major Version : {0};Minor Version : {1};Revision : {2}", assemblyName.Version.Major, assemblyName.Version.Minor, assemblyName.Version.Revision);
            this.PutString(Boss.Tags.APPLICATION, clientName);
            this.PutString(Boss.Tags.VERSION, string.Format("{0}.{1}.{2}", assemblyName.Version.Major, assemblyName.Version.Minor, assemblyName.Version.Revision));
            this.PutLong(Boss.Tags.BUILD, (long)assemblyName.Version.Build);
        }
    }
}
