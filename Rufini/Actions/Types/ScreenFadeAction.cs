using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Mother4.Scenes;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Utility;
using SFML.Graphics;

namespace Rufini.Actions.Types
{
	// Token: 0x02000154 RID: 340
	internal class ScreenFadeAction : RufiniAction
	{
		// Token: 0x06000765 RID: 1893 RVA: 0x00030440 File Offset: 0x0002E640
		public ScreenFadeAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "col",
					Type = typeof(Color)
				},
				new ActionParam
				{
					Name = "dur",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000304D8 File Offset: 0x0002E6D8
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			Color value = base.GetValue<Color>("col");
			int value2 = base.GetValue<int>("dur");
			bool value3 = base.GetValue<bool>("blk");
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				this.dimmer = ((OverworldScene)scene).Dimmer;
				this.dimmer.ChangeColor(value, value2);
				if (value3)
				{
					this.dimmer.OnFadeComplete += this.OnFadeComplete;
					result.Wait = ScriptExecutor.WaitType.Event;
					this.context = context;
				}
			}
			return result;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0003056F File Offset: 0x0002E76F
		private void OnFadeComplete(ScreenDimmer sender)
		{
			this.dimmer.OnFadeComplete -= this.OnFadeComplete;
			this.context.Executor.Continue();
		}

		// Token: 0x0400094C RID: 2380
		private ExecutionContext context;

		// Token: 0x0400094D RID: 2381
		private ScreenDimmer dimmer;
	}
}
