using System;
using System.Linq;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{
	// Token: 0x02000013 RID: 19
	internal class PasswordStrengthHelper
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003C1A File Offset: 0x00001E1A
		internal static bool AllCriteriasSatisfied(string password)
		{
			return PasswordStrengthHelper.PasswordCriteriasCount(password) == 4;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003C28 File Offset: 0x00001E28
		private static int PasswordCriteriasCount(string password)
		{
			bool[] array = new bool[4];
			array[0] = password.Any((char c) => !char.IsLetterOrDigit(c));
			int num = 1;
			bool flag;
			if (password.Any((char c) => char.IsUpper(c)))
			{
				flag = password.Any((char c) => char.IsLower(c));
			}
			else
			{
				flag = false;
			}
			array[num] = flag;
			array[2] = password.Length >= 8;
			int num2 = 3;
			bool flag2;
			if (password.Any((char c) => char.IsLetter(c)))
			{
				flag2 = password.Any((char c) => char.IsNumber(c));
			}
			else
			{
				flag2 = false;
			}
			array[num2] = flag2;
			return array.Count((bool c) => c);
		}
	}
}
