using System;
using System.Collections.Generic;
using System.Security;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.SilentRegistrationCommandParameters;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{

	public class SilentRegistrationParameter
	{



		public bool IsSet { get; set; }




		public string Name { get; set; }




		public SilentRegistrationParameterType Type { get; set; }




		public SilentRegistrationOperationType OperationType { get; set; }




		public int RequiredValuesCount { get; set; }




		public List<string> Values { get; set; } = new List<string>();




		public List<SecureString> SecureValues { get; set; } = new List<SecureString>();




		public bool IsSecure { get; set; }
	}
}
