using System;
using Carbine.Graphics;

namespace Mother4.GUI.Modifiers
{
	// Token: 0x0200001F RID: 31
	internal interface IGraphicModifier
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000068 RID: 104
		bool Done { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000069 RID: 105
		Graphic Graphic { get; }

		// Token: 0x0600006A RID: 106
		void Update();
	}
}
