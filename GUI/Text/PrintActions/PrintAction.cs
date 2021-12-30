using System;

namespace Mother4.GUI.Text.PrintActions
{
	// Token: 0x0200004D RID: 77
	internal struct PrintAction
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000B539 File Offset: 0x00009739
		public PrintActionType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000B541 File Offset: 0x00009741
		public object Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000B549 File Offset: 0x00009749
		public PrintAction(PrintActionType type, params object[] data)
		{
			this.type = type;
			this.data = data;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000B559 File Offset: 0x00009759
		public PrintAction(PrintActionType type, object data)
		{
			this.type = type;
			this.data = data;
		}

		// Token: 0x040002A4 RID: 676
		private PrintActionType type;

		// Token: 0x040002A5 RID: 677
		private object data;
	}
}
