using System;

namespace Carbine.Utility
{
	// Token: 0x02000064 RID: 100
	internal class Rectangle
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000EC63 File Offset: 0x0000CE63
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000EC6B File Offset: 0x0000CE6B
		public float X { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000EC74 File Offset: 0x0000CE74
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000EC7C File Offset: 0x0000CE7C
		public float Y { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000EC85 File Offset: 0x0000CE85
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000EC8D File Offset: 0x0000CE8D
		public float Width { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000EC96 File Offset: 0x0000CE96
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000EC9E File Offset: 0x0000CE9E
		public float Height { get; set; }

		// Token: 0x060002BD RID: 701 RVA: 0x0000ECA7 File Offset: 0x0000CEA7
		public Rectangle(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}
	}
}
