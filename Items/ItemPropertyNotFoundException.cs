using System;

namespace Mother4.Items
{
	// Token: 0x020000FA RID: 250
	internal class ItemPropertyNotFoundException : ApplicationException
	{
		// Token: 0x060005C4 RID: 1476 RVA: 0x000224A4 File Offset: 0x000206A4
		public ItemPropertyNotFoundException(string name) : base(string.Format("\"{0}\" was not found in the item's properties.", name))
		{
		}
	}
}
