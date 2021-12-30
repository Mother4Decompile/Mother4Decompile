using System;
using Carbine;
using Carbine.Actors;
using Carbine.Graphics;
using Carbine.Scenes;

namespace Mother4.Scenes
{
	// Token: 0x02000106 RID: 262
	internal class StandardScene : Scene
	{
		// Token: 0x06000610 RID: 1552 RVA: 0x00023988 File Offset: 0x00021B88
		protected StandardScene()
		{
			this.pipeline = new RenderPipeline(Engine.FrameBuffer);
			this.actorManager = new ActorManager();
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000239AB File Offset: 0x00021BAB
		public override void Focus()
		{
			base.Focus();
			ViewManager.Instance.CancelMoveTo();
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000239BD File Offset: 0x00021BBD
		public override void Update()
		{
			base.Update();
			this.actorManager.Step();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000239D0 File Offset: 0x00021BD0
		public override void Draw()
		{
			base.Draw();
			this.pipeline.Draw();
		}

		// Token: 0x040007DF RID: 2015
		protected RenderPipeline pipeline;

		// Token: 0x040007E0 RID: 2016
		protected ActorManager actorManager;
	}
}
