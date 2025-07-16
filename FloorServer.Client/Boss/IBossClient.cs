using System;

namespace FloorServer.Client.Boss
{
    /// <summary>
    ///  BOSS client interface.
    /// Handles single connection to BOSS server process.
    /// </summary>
    public interface IBossClient
	{
		void Start();
		void Stop();
		void Reader();
		void SetName(string newName);
		void Write(string msg);
		event EventHandler<BomEventArgs> BossMessageReceived;
		event EventHandler<BossStatusEventArgs> BossStatusChanged;
		String Name { get; }
		String Host { get; set; }
		int Port { get; set; }
		int BufferSize { get; set; }
		bool KeepAlive { get; set; }
	}
}
