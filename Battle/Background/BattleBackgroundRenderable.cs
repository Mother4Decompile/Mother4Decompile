using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.Background
{
	// Token: 0x02000065 RID: 101
	internal class BattleBackgroundRenderable : Renderable
	{
		// Token: 0x0600023F RID: 575 RVA: 0x0000E194 File Offset: 0x0000C394
		public BattleBackgroundRenderable(string file, int depth)
		{
			this.position = VectorMath.ZERO_VECTOR;
			this.origin = VectorMath.ZERO_VECTOR;
			this.size = new Vector2f(float.MaxValue, float.MaxValue);
			this.depth = depth;
			this.bbg = new BattleBackground(file);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000E1E5 File Offset: 0x0000C3E5
		public void AddTranslation(float x, float y, float xFactor, float yFactor)
		{
			this.bbg.AddTranslation(x, y, xFactor, yFactor);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000E1F7 File Offset: 0x0000C3F7
		public override void Draw(RenderTarget target)
		{
			this.bbg.Draw(target);
		}

		// Token: 0x04000355 RID: 853
		private BattleBackground bbg;
	}
}
