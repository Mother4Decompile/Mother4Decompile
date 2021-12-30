using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Mother4.GUI;
using Mother4.Scenes;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000145 RID: 325
	internal class IrisOverlayAction : RufiniAction
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x0002F178 File Offset: 0x0002D378
		public IrisOverlayAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "prg",
					Type = typeof(float)
				},
				new ActionParam
				{
					Name = "spd",
					Type = typeof(float)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0002F210 File Offset: 0x0002D410
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			float value = base.GetValue<float>("prg");
			float value2 = base.GetValue<float>("spd");
			this.isBlocking = base.GetValue<bool>("blk");
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				OverworldScene overworldScene = (OverworldScene)scene;
				IrisOverlay irisOverlay = overworldScene.IrisOverlay;
				if (value2 > 0f)
				{
					irisOverlay.Speed = value2;
					irisOverlay.OnAnimationComplete += this.OnAnimationComplete;
				}
				else
				{
					this.isBlocking = false;
				}
				irisOverlay.Progress = value;
				irisOverlay.Visible = true;
			}
			this.context = context;
			return new ActionReturnContext
			{
				Wait = (this.isBlocking ? ScriptExecutor.WaitType.Event : ScriptExecutor.WaitType.None)
			};
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0002F2C8 File Offset: 0x0002D4C8
		private void OnAnimationComplete(IrisOverlay sender)
		{
			sender.OnAnimationComplete -= this.OnAnimationComplete;
			if (this.isBlocking)
			{
				this.context.Executor.Continue();
			}
		}

		// Token: 0x04000942 RID: 2370
		private ExecutionContext context;

		// Token: 0x04000943 RID: 2371
		private bool isBlocking;
	}
}
