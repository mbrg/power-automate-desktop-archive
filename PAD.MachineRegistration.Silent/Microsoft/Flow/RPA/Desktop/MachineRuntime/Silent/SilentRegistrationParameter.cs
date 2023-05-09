using System;
using System.Collections.Generic;
using System.Security;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.SilentRegistrationCommandParameters;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{

	public class SilentRegistrationParameter
	{

		// (get) Token: 0x06000028 RID: 40 RVA: 0x000035ED File Offset: 0x000017ED
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000035F5 File Offset: 0x000017F5
		public bool IsSet { get; set; }


		// (get) Token: 0x0600002A RID: 42 RVA: 0x000035FE File Offset: 0x000017FE
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00003606 File Offset: 0x00001806
		public string Name { get; set; }


		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000360F File Offset: 0x0000180F
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00003617 File Offset: 0x00001817
		public SilentRegistrationParameterType Type { get; set; }


		// (get) Token: 0x0600002E RID: 46 RVA: 0x00003620 File Offset: 0x00001820
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00003628 File Offset: 0x00001828
		public SilentRegistrationOperationType OperationType { get; set; }


		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003631 File Offset: 0x00001831
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00003639 File Offset: 0x00001839
		public int RequiredValuesCount { get; set; }


		// (get) Token: 0x06000032 RID: 50 RVA: 0x00003642 File Offset: 0x00001842
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000364A File Offset: 0x0000184A
		public List<string> Values { get; set; } = new List<string>();


		// (get) Token: 0x06000034 RID: 52 RVA: 0x00003653 File Offset: 0x00001853
		// (set) Token: 0x06000035 RID: 53 RVA: 0x0000365B File Offset: 0x0000185B
		public List<SecureString> SecureValues { get; set; } = new List<SecureString>();


		// (get) Token: 0x06000036 RID: 54 RVA: 0x00003664 File Offset: 0x00001864
		// (set) Token: 0x06000037 RID: 55 RVA: 0x0000366C File Offset: 0x0000186C
		public bool IsSecure { get; set; }
	}
}
