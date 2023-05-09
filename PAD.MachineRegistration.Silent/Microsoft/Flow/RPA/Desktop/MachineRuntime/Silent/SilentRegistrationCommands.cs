using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using Microsoft.Flow.RPA.Desktop.Common.Structures.Extensions;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Authentication;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.SilentRegistrationCommandParameters;
using Microsoft.Flow.RPA.Desktop.Shared.Common.Enums;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{

	public class SilentRegistrationCommands
	{



		public SilentRegistrationOperationType OperationToExecute { get; private set; }


		public static void DisplayUsage(bool isError)
		{
			TextWriter textWriter = (isError ? Console.Error : Console.Out);
			textWriter.WriteLine("Power Automate - Machine Registration Silent allow you to deploy PAD easily on multiple machines (Preview).");
			textWriter.WriteLine("Scenarios:");
			foreach (ValueTuple<string, string> valueTuple in new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("Register machine with username", "$ <exe> -register -machinename mymachine -username john@contoso.com"),
				new ValueTuple<string, string>("Register machine with application id", "$ <exe> -register -force -applicationid 654b31ae-d941-4e22-8798-7add8fdf049f -clientsecret -tenantid bc301e21-fe79-4572-9768-ded9fc364da3"),
				new ValueTuple<string, string>("Join machine group", "$ <exe> -joinmachinegroup -groupid 524acc79-c2ae-43f0-a68a-47319e62a1eb -grouppassword")
			})
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string text = item + ": " + item2;
				textWriter.WriteLine(text.PadLeft(text.Length + 3));
			}
			textWriter.WriteLine();
			textWriter.WriteLine("Options:");
			foreach (ValueTuple<string, string> valueTuple2 in new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("-help", "Display the usage guide."),
				new ValueTuple<string, string>("-username <string>", "The username to use."),
				new ValueTuple<string, string>("-applicationid <string> [-clientsecret][-certificatethumbprint <string>]", "The application to use."),
				new ValueTuple<string, string>("-cloudtype <string>", "The cloud type to use e.g. public (default), gcc, gcchigh, dod."),
				new ValueTuple<string, string>("-environmentid <string>", "The environment identifier to use."),
				new ValueTuple<string, string>("-tenantid <string>", "The tenant identifier to use. This parameter is required when using an application."),
				new ValueTuple<string, string>("-authenticationfallback <string>", "The registration authentication to use e.g. interactive or devicecode."),
				new ValueTuple<string, string>("-register [-machinename <string>][-machinedescription <string>][-force]", "Perform the registration with the given parameter."),
				new ValueTuple<string, string>("-joinmachinegroup -groupid <string> -grouppassword", "Perform joining the current machine to the given group."),
				new ValueTuple<string, string>("-getstatus [-format <string>]", "Get the current status of the machine. Format can be default or json."),
				new ValueTuple<string, string>("-recover", "For security if the machine is not online for more than 90days, the connection will expire. You can use this command to recover it.")
			})
			{
				string item3 = valueTuple2.Item1;
				string item4 = valueTuple2.Item2;
				textWriter.WriteLine("{0,-40} {1,5:N1}", item3.PadLeft(item3.Length + 3), item4);
			}
			textWriter.WriteLine();
			textWriter.WriteLine("Secure element as grouppassword and clientsecret will required to be typed and will be kept confidential.");
			textWriter.WriteLine("Current version doesn't allow to perform register and join machine group on the same command.");
			textWriter.WriteLine();
			textWriter.WriteLine("Learn more about machine registration: https://go.microsoft.com/fwlink/?linkid=2165141");
			textWriter.WriteLine();
		}


		public SilentRegistrationCommands(IEnumerable<string> parameters)
		{
			if (parameters == null || parameters.Count<string>() == 0)
			{
				this.OperationToExecute = SilentRegistrationOperationType.Help;
				return;
			}
			this.OperationToExecute = SilentRegistrationOperationType.None;
			try
			{
				this.ParseParameters(parameters.ToList<string>());
			}
			catch
			{
				SilentRegistrationCommands.DisplayUsage(true);
				throw;
			}
		}


		public SilentRegistrationFormatType GetFormatType()
		{
			SilentRegistrationFormatType silentRegistrationFormatType;
			this.TryGetEnumValue<SilentRegistrationFormatType>(SilentRegistrationParameterType.FormatType, SilentRegistrationErrorCodes.InvalidFormatType, out silentRegistrationFormatType);
			return silentRegistrationFormatType;
		}


		public bool TryGetUsernameParameter(out string username)
		{
			return this.TryGetValue(SilentRegistrationParameterType.Username, out username);
		}


		public bool NeedForceRegistration()
		{
			return this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.ForceRegistration).IsSet;
		}


		public string GetRegistrationMachineNameParameter()
		{
			return this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.RegistrationMachineName).Values.FirstOrDefault<string>();
		}


		public string GetB64SerializedRegisterRpaBoxRequest()
		{
			return this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.RegisterRpaBoxB64SerializedRequest).Values.FirstOrDefault<string>();
		}


		public string GetRegistrationMachineDescriptionParameter()
		{
			return this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.RegistrationMachineDescription).Values.FirstOrDefault<string>();
		}


		public string GetEnvironmentIdentifier()
		{
			return this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.EnvironmentIdentifier).Values.FirstOrDefault<string>();
		}


		public string GetTenantIdentifier()
		{
			return this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.TenantIdentifier).Values.FirstOrDefault<string>();
		}


		public bool TryGetApplicationIdentifier(out string appId)
		{
			return this.TryGetValue(SilentRegistrationParameterType.ApplicationIdentifier, out appId);
		}


		public bool TryGetApplicationSecret(out SecureString appSecret)
		{
			return this.TryGetSecureValue(SilentRegistrationParameterType.ApplicationSecret, out appSecret);
		}


		public bool TryGetApplicationCertificateThumbprint(out string appCertThumbprint)
		{
			return this.TryGetValue(SilentRegistrationParameterType.ApplicationCertificateThumbprint, out appCertThumbprint);
		}


		public SilentRegistrationAuthenticationFallbackType GetAuthenticationFallback()
		{
			SilentRegistrationAuthenticationFallbackType silentRegistrationAuthenticationFallbackType;
			this.TryGetEnumValue<SilentRegistrationAuthenticationFallbackType>(SilentRegistrationParameterType.AuthenticationFallback, SilentRegistrationErrorCodes.InvalidAuthenticationFallbackType, out silentRegistrationAuthenticationFallbackType);
			return silentRegistrationAuthenticationFallbackType;
		}


		public bool TryGetCloudType(out CloudType cloudType)
		{
			return this.TryGetEnumValue<CloudType>(SilentRegistrationParameterType.CloudType, SilentRegistrationErrorCodes.InvalidCloudType, out cloudType);
		}


		public bool TryGetJoinGroupParameters(out Guid groupId, out SecureString password)
		{
			groupId = default(Guid);
			password = null;
			if (!this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.JoinGroup).IsSet)
			{
				return false;
			}
			SilentRegistrationParameter silentRegistrationParameter = this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.GroupId);
			SilentRegistrationParameter silentRegistrationParameter2 = this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.GroupPassword);
			if (!silentRegistrationParameter.IsSet || !silentRegistrationParameter2.IsSet)
			{
				return false;
			}
			string text = silentRegistrationParameter.Values.First<string>();
			if (!Guid.TryParse(text, out groupId))
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.InvalidGuid.ToString(), "Error: Invalid group id guid '" + text + "'.", null);
			}
			password = silentRegistrationParameter2.SecureValues.First<SecureString>();
			return true;
		}


		private bool TryGetCommonHostedParameters(out Uri serviceUri, out string vmResourceId, out SecureString miAuthToken)
		{
			serviceUri = null;
			vmResourceId = null;
			miAuthToken = null;
			if (!this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.ServiceUri).IsSet || !this.TryGetUriValue(SilentRegistrationParameterType.ServiceUri, out serviceUri))
			{
				return false;
			}
			if (!this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.VmResourceId).IsSet || !this.TryGetValue(SilentRegistrationParameterType.VmResourceId, out vmResourceId))
			{
				return false;
			}
			SilentRegistrationParameter silentRegistrationParameter = this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.ManagedIdentityAuthToken);
			if (!silentRegistrationParameter.IsSet)
			{
				return false;
			}
			miAuthToken = silentRegistrationParameter.SecureValues.First<SecureString>();
			return true;
		}


		public bool TryGetAADJoinParameters(out Uri serviceUri, out string vmResourceId, out SecureString miAuthToken, out string tenantId)
		{
			serviceUri = null;
			vmResourceId = null;
			miAuthToken = null;
			tenantId = null;
			if (!this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.AADJoin).IsSet)
			{
				return false;
			}
			if (!this.TryGetCommonHostedParameters(out serviceUri, out vmResourceId, out miAuthToken))
			{
				return false;
			}
			return this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.TenantIdentifier).IsSet && this.TryGetValue(SilentRegistrationParameterType.TenantIdentifier, out tenantId);
		}


		public bool TryGetRegisterHostedParameters(out Uri serviceUri, out string vmResourceId, out SecureString miAuthToken, out SecureString groupPassword)
		{
			serviceUri = null;
			vmResourceId = null;
			miAuthToken = null;
			groupPassword = null;
			if (!this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.RegisterHosted).IsSet)
			{
				return false;
			}
			if (!this.TryGetCommonHostedParameters(out serviceUri, out vmResourceId, out miAuthToken))
			{
				return false;
			}
			SilentRegistrationParameter silentRegistrationParameter = this._parameters.First((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.GroupPassword);
			if (!silentRegistrationParameter.IsSet)
			{
				return false;
			}
			groupPassword = silentRegistrationParameter.SecureValues.First<SecureString>();
			return true;
		}


		public bool TryGetClientSessionId(out string clientSessionId)
		{
			SilentRegistrationParameter silentRegistrationParameter = this._parameters.FirstOrDefault((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.ClientSessionId);
			if (!silentRegistrationParameter.IsSet)
			{
				clientSessionId = null;
				return false;
			}
			clientSessionId = silentRegistrationParameter.Values.First<string>();
			Guid guid;
			if (!Guid.TryParse(clientSessionId, out guid))
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.InvalidGuid.ToString(), string.Format("Error: Invalid client session id guid '{0}'.", silentRegistrationParameter), null);
			}
			return true;
		}


		public bool TryGetCorrelationId(out string correlationId)
		{
			SilentRegistrationParameter silentRegistrationParameter = this._parameters.FirstOrDefault((SilentRegistrationParameter s) => s.Type == SilentRegistrationParameterType.CorrelationId);
			if (!silentRegistrationParameter.IsSet)
			{
				correlationId = null;
				return false;
			}
			correlationId = silentRegistrationParameter.Values.First<string>();
			Guid guid;
			if (!Guid.TryParse(correlationId, out guid))
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.InvalidGuid.ToString(), string.Format("Error: Invalid correlation id guid '{0}'.", silentRegistrationParameter), null);
			}
			return true;
		}


		private static SecureString ReadSecureInput(string commandName)
		{
			if (Console.IsInputRedirected)
			{
				return Console.ReadLine().ToSecureString();
			}
			Console.WriteLine("Please input '" + commandName + "' value:");
			SecureString secureString = new SecureString();
			ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
			while (consoleKeyInfo.Key != ConsoleKey.Enter)
			{
				if (consoleKeyInfo.Key == ConsoleKey.Backspace)
				{
					if (secureString.Length > 0)
					{
						secureString.RemoveAt(secureString.Length - 1);
						Console.Write(consoleKeyInfo.KeyChar);
						Console.Write(" ");
						Console.Write(consoleKeyInfo.KeyChar);
					}
				}
				else
				{
					secureString.AppendChar(consoleKeyInfo.KeyChar);
					Console.Write("*");
				}
				consoleKeyInfo = Console.ReadKey(true);
			}
			Console.WriteLine();
			return secureString;
		}


		private void ParseParameters(List<string> args)
		{
			SilentRegistrationParameter silentRegistrationParameter = null;
			using (List<string>.Enumerator enumerator = args.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string arg = enumerator.Current;
					if (SilentRegistrationCommands.PrefixesArg.Any((string prefixeArg) => arg.StartsWith(prefixeArg)))
					{
						string currentArg = arg.Substring(1);
						if (silentRegistrationParameter != null)
						{
							this.ValidateAndCompleteCommand(silentRegistrationParameter);
						}
						silentRegistrationParameter = this._parameters.FirstOrDefault((SilentRegistrationParameter p) => string.Equals(p.Name, currentArg, StringComparison.InvariantCultureIgnoreCase));
						if (silentRegistrationParameter == null)
						{
							this._errors.Add("Error: parameter '" + currentArg + "' doesn't exist.");
						}
						else
						{
							if (this._parsedParameters.Contains(silentRegistrationParameter.Type))
							{
								this._errors.Add("Error: '" + silentRegistrationParameter.Name + "' is specified more than once.");
							}
							this._parsedParameters.Add(silentRegistrationParameter.Type);
							if (silentRegistrationParameter.OperationType != SilentRegistrationOperationType.None)
							{
								if (this.OperationToExecute != SilentRegistrationOperationType.None)
								{
									this._errors.Add("Error: only one operation should be specified.");
								}
								this.OperationToExecute = silentRegistrationParameter.OperationType;
							}
						}
					}
					else if (silentRegistrationParameter != null)
					{
						if (silentRegistrationParameter.IsSecure)
						{
							throw new SilentRegistrationException(SilentRegistrationErrorCodes.SecureInputAsParameterIsForbidden.ToString(), "Error: secure input '" + silentRegistrationParameter.Name + "' value has been specified as an input parameter.", null);
						}
						silentRegistrationParameter.Values.Add(arg.Trim(new char[] { '"' }));
					}
					else
					{
						this._errors.Add("Error: missing command before '" + arg + "' parameter.");
					}
				}
			}
			this.ValidateAndCompleteCommand(silentRegistrationParameter);
			if (this.OperationToExecute == SilentRegistrationOperationType.None)
			{
				this._errors.Add("Error: missing operation to execute.");
			}
			if (this._errors.Count != 0)
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.InvalidArguments.ToString(), string.Join(Environment.NewLine, this._errors), null);
			}
		}


		private void ValidateAndCompleteCommand(SilentRegistrationParameter currentCommand)
		{
			if (currentCommand == null)
			{
				return;
			}
			if (currentCommand.IsSecure)
			{
				currentCommand.SecureValues.Add(SilentRegistrationCommands.ReadSecureInput(currentCommand.Name));
			}
			if (currentCommand.SecureValues.Count + currentCommand.Values.Count != currentCommand.RequiredValuesCount)
			{
				this._errors.Add(string.Format("Error: '{0}' requires {1} arguments.", currentCommand.Name, currentCommand.RequiredValuesCount) + string.Format(" Got {0} values and {1} secure values.", currentCommand.Values.Count, currentCommand.SecureValues.Count));
				return;
			}
			currentCommand.IsSet = true;
		}


		private bool TryGetEnumValue<T>(SilentRegistrationParameterType parameterType, SilentRegistrationErrorCodes errorCode, out T enumValue) where T : struct
		{
			enumValue = default(T);
			SilentRegistrationParameter silentRegistrationParameter = this._parameters.FirstOrDefault((SilentRegistrationParameter parameter) => parameter.Type == parameterType);
			if (silentRegistrationParameter == null || !silentRegistrationParameter.IsSet)
			{
				return false;
			}
			string text = silentRegistrationParameter.Values.FirstOrDefault<string>();
			int num;
			if (int.TryParse(text, out num) || !Enum.TryParse<T>(text, true, out enumValue))
			{
				SilentRegistrationCommands.DisplayUsage(true);
				throw new SilentRegistrationException(errorCode.ToString(), string.Concat(new string[] { "Error: invalid value '", text, "' for '", silentRegistrationParameter.Name, "' parameter. See -help above." }), null);
			}
			return true;
		}


		private bool TryGetUriValue(SilentRegistrationParameterType parameterType, out Uri value)
		{
			value = null;
			string text;
			if (!this.TryGetValue(parameterType, out text))
			{
				return false;
			}
			if (!Uri.TryCreate(text, UriKind.Absolute, out value))
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.MissingParameters.ToString(), string.Format("Error: '{0}' has invalid uri, value '{1}'.", parameterType, text), null);
			}
			return true;
		}


		private bool TryGetValue(SilentRegistrationParameterType parameterType, out string value)
		{
			value = null;
			SilentRegistrationParameter silentRegistrationParameter = this._parameters.First((SilentRegistrationParameter parameter) => parameter.Type == parameterType);
			if (!silentRegistrationParameter.IsSet)
			{
				return false;
			}
			value = silentRegistrationParameter.Values.FirstOrDefault<string>();
			if (value == null)
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.MissingParameters.ToString(), "Error: missing '" + silentRegistrationParameter.Name + "' value.", null);
			}
			return true;
		}


		private bool TryGetSecureValue(SilentRegistrationParameterType parameterType, out SecureString value)
		{
			value = null;
			SilentRegistrationParameter silentRegistrationParameter = this._parameters.First((SilentRegistrationParameter parameter) => parameter.Type == parameterType);
			if (!silentRegistrationParameter.IsSet)
			{
				return false;
			}
			value = silentRegistrationParameter.SecureValues.FirstOrDefault<SecureString>();
			if (value == null)
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.MissingParameters.ToString(), "Error: missing '" + silentRegistrationParameter.Name + "' secure value.", null);
			}
			return true;
		}


		private const string MachineRegistrationFwdLink = "https://go.microsoft.com/fwlink/?linkid=2165141";


		private const char ValueDelimiter = '"';


		private HashSet<SilentRegistrationParameterType> _parsedParameters = new HashSet<SilentRegistrationParameterType>();


		private static readonly string[] PrefixesArg = new string[] { "-", "/" };


		private IList<string> _errors = new List<string>();


		private List<SilentRegistrationParameter> _parameters = new List<SilentRegistrationParameter>
		{
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.Usage,
				OperationType = SilentRegistrationOperationType.Help,
				Name = "help"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.Username,
				Name = "username",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.ApplicationIdentifier,
				Name = "applicationid",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.ApplicationSecret,
				Name = "clientsecret",
				RequiredValuesCount = 1,
				IsSecure = true
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.ApplicationCertificateThumbprint,
				Name = "certificatethumbprint",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.CloudType,
				Name = "cloudtype",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.EnvironmentIdentifier,
				Name = "environmentid",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.TenantIdentifier,
				Name = "tenantid",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.Register,
				OperationType = SilentRegistrationOperationType.Register,
				Name = "register"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.ForceRegistration,
				Name = "force"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.RegistrationMachineName,
				Name = "machinename",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.RegistrationMachineDescription,
				Name = "machinedescription",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.JoinGroup,
				OperationType = SilentRegistrationOperationType.JoinGroup,
				Name = "joinmachinegroup"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.GroupId,
				Name = "groupid",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.GroupPassword,
				Name = "grouppassword",
				RequiredValuesCount = 1,
				IsSecure = true
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.AuthenticationFallback,
				Name = "authenticationfallback",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.GetStatus,
				OperationType = SilentRegistrationOperationType.GetStatus,
				Name = "getstatus"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.GetRegistrationState,
				OperationType = SilentRegistrationOperationType.GetRegistrationState,
				Name = "getregistrationstate"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.FormatType,
				Name = "format",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.CorrelationId,
				Name = "correlationid",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.ClientSessionId,
				Name = "clientsessionid",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.RegisterHosted,
				OperationType = SilentRegistrationOperationType.RegisterHosted,
				Name = "registerhosted"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.ServiceUri,
				Name = "serviceuri",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.ManagedIdentityAuthToken,
				Name = "miauthtoken",
				RequiredValuesCount = 1,
				IsSecure = true
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.VmResourceId,
				Name = "vmresourceid",
				RequiredValuesCount = 1
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.AADJoin,
				OperationType = SilentRegistrationOperationType.AADJoin,
				Name = "aadjoin"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.Recover,
				OperationType = SilentRegistrationOperationType.Recover,
				Name = "recover"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.RegisterRpaBox,
				OperationType = SilentRegistrationOperationType.RegisterRpaBox,
				Name = "registerRpaBox"
			},
			new SilentRegistrationParameter
			{
				Type = SilentRegistrationParameterType.RegisterRpaBoxB64SerializedRequest,
				Name = "registerRpaBoxPayload",
				RequiredValuesCount = 1
			}
		};
	}
}
