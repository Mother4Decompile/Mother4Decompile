using System;
using Carbine.Actors;
using Carbine.Graphics;
using SFML.Graphics;

namespace Mother4.Battle.UI
{
	// Token: 0x020000DC RID: 220
	internal class Blinker : Actor
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060004F9 RID: 1273 RVA: 0x0001F988 File Offset: 0x0001DB88
		// (remove) Token: 0x060004FA RID: 1274 RVA: 0x0001F9C0 File Offset: 0x0001DBC0
		public event Blinker.CompletionHandler OnComplete;

		// Token: 0x060004FB RID: 1275 RVA: 0x0001F9F5 File Offset: 0x0001DBF5
		public Blinker(Graphic graphic, int blinks)
		{
			this.graphic = graphic;
			graphic.Color = new Color(0, 0, 0, 0);
			this.blinkCap = blinks;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		public override void Update()
		{
			base.Update();
			this.graphic.Color = new Color(0, 0, 0, (byte)((Math.Sin((double)this.timer) + 1.0) / 2.0 * 255.0));
			if ((double)this.timer > 6.283185307179586 * (double)this.blinkCap)
			{
				if (this.OnComplete != null)
				{
					this.OnComplete();
					return;
				}
			}
			else
			{
				this.timer += 0.3926991f;
			}
		}

		// Token: 0x040006E0 RID: 1760
		private Graphic graphic;

		// Token: 0x040006E1 RID: 1761
		private int blinkCap;

		// Token: 0x040006E2 RID: 1762
		private float timer;

		// Token: 0x020000DD RID: 221
		// (Invoke) Token: 0x060004FE RID: 1278
		public delegate void CompletionHandler();
	}
}
