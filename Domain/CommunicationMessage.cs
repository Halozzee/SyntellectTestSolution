using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	public enum ResponseStatus
	{
		None,
		Success,
		Fail,
		Exception
	}

	public class CommunicationMessage
	{
		public string ExceptionMessage { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public string Content { get; set; }
	}
}
