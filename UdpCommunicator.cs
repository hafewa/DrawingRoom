using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleNetwork
{
	public class IncomingMessageEventArgs : EventArgs
	{
		public string Message { get; private set; }
		public IPEndPoint EndPoint { get; private set; }

		public IncomingMessageEventArgs(string message, IPEndPoint ep)
		{
			Message = message;
			EndPoint = ep;
		}
	}

	/// <summary>
	/// Summary description for Broadcast.
	/// </summary>
	public class UdpCommunicator
	{
		// Multicast IPs can be anything in the range 224.0.0.0-239.255.255.255, but...
		// --> 224.0.0.0 – 224.0.0.255 reserved for local purposes (such as administrative and maintenance tasks). Don’t use.
		// --> 224.0.1.0 – 238.255.255.255 can be used (with caution)
		// --> 239.0.0.0 – 239.255.255.255 reserved for “administrative scoping” Don’t use.
		// Nice summary at http://www.ibiblio.org/pub/Linux/docs/HOWTO/other-formats/html_single/Multicast-HOWTO.html#ss2.1
		// Exhaustive details on address assignments at http://www.iana.org/assignments/multicast-addresses
		// also see my "Simple Peer to Peer Networking.doc"
		private UdpClient client;
		private IPEndPoint multiEp;
		
		// Next two lines of code establish the event that can be subscribed to by any
		// code that wants to receive messages via this client.
		public delegate void IncomingMessageEventHandler(object sender, IncomingMessageEventArgs e);
		public event IncomingMessageEventHandler IncomingMessage;
		// above is an auto-event --> accessors are thread-safe (see below for what an explicit implementation would look like)
		//public delegate void IncomingDrawingEventHandler(object sender, IncomingDrawingEventArgs e);
		//public event IncomingDrawingEventHandler IncomingDrawing;


		// if TimeToLive is not specified, set it to 3 -- enough to bridge ethernet and wireless in NSD network
		public UdpCommunicator(IPAddress multiIP, int port)
			: this(multiIP, port, 3)
		{
		}

		// throws System.Net.Sockets.SocketException
		public UdpCommunicator(IPAddress multiIP, int port, int timeToLive)
		{
			// Set us up to transmit and receive on specified multicast channel
			client = new UdpClient(port);
			client.JoinMulticastGroup(multiIP, timeToLive);
			multiEp = new IPEndPoint(multiIP, port);

			// Turn on our receiver
			client.BeginReceive(PacketReceive, null);
		}

		private void PacketReceive(IAsyncResult ar)
		{
			IPEndPoint ep = null;
			byte[] data = client.EndReceive(ar, ref ep);
			string message = Encoding.UTF8.GetString(data);

			// make invoking thread-safe
			IncomingMessageEventHandler handler = IncomingMessage;
			if (handler != null)
				handler(this, new IncomingMessageEventArgs(message, ep));

			client.BeginReceive(PacketReceive, null);
		}

		// Make it simple & clean to send text messages...
		// Send to multi-cast group
		public void SendMessage(string message)	{this.SendMessage(message, this.multiEp);}
		// Send to specific IP address
		public void SendMessage(string message, IPEndPoint dest)
		{
			if (message.Length == 0)
				return;

			byte[] sdata = Encoding.UTF8.GetBytes(message);
			client.Send(sdata,sdata.Length,dest);
		}
	}
}
