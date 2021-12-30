using System;

namespace Mother4.Items
{
	// Token: 0x020000F8 RID: 248
	internal class InvalidPropertyType : ApplicationException
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x000223AF File Offset: 0x000205AF
		public InvalidPropertyType(Type expectedType, Type valueType) : base(string.Format("Expected {0}, but found {1}.", expectedType.Name, valueType.Name))
		{
		}
	}
}
