using System;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x0200006F RID: 111
	internal class BattleSmash
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000EDE9 File Offset: 0x0000CFE9
		public bool Done
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000EDF4 File Offset: 0x0000CFF4
		public BattleSmash(RenderPipeline pipeline, Vector2f position)
		{
			this.pipeline = pipeline;
			this.smashGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "smash.dat", "smash", position, 32767);
			this.smashGraphic.OnAnimationComplete += this.SmashgraphicAnimationComplete;
			this.pipeline.Add(this.smashGraphic);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000EE5B File Offset: 0x0000D05B
		private void SmashgraphicAnimationComplete(AnimatedRenderable graphic)
		{
			this.pipeline.Remove(this.smashGraphic);
			this.done = true;
		}

		// Token: 0x040003B2 RID: 946
		private RenderPipeline pipeline;

		// Token: 0x040003B3 RID: 947
		private Graphic smashGraphic;

		// Token: 0x040003B4 RID: 948
		private bool done;
	}
}
