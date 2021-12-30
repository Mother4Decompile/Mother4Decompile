using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000135 RID: 309
	internal class EmoticonPlayerAction : RufiniAction
	{
		// Token: 0x0600071A RID: 1818 RVA: 0x0002D610 File Offset: 0x0002B810
		public EmoticonPlayerAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "emt",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002D680 File Offset: 0x0002B880
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			RufiniOption value = base.GetValue<RufiniOption>("emt");
			this.isBlocking = base.GetValue<bool>("blk");
			if (context.Player != null)
			{
				string spriteName = EmoticonPlayerAction.EMOTE_TYPE_SUBSPRITE_MAP[0];
				EmoticonPlayerAction.EMOTE_TYPE_SUBSPRITE_MAP.TryGetValue(value.Option, out spriteName);
				IndexedColorGraphic indexedColorGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "emote.dat", spriteName, context.Player.EmoticonPoint, context.Player.Depth);
				indexedColorGraphic.OnAnimationComplete += this.OnAnimationComplete;
				context.Pipeline.Add(indexedColorGraphic);
				this.context = context;
			}
			else
			{
				this.isBlocking = false;
			}
			return new ActionReturnContext
			{
				Wait = (this.isBlocking ? ScriptExecutor.WaitType.Event : ScriptExecutor.WaitType.None)
			};
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0002D748 File Offset: 0x0002B948
		private void OnAnimationComplete(AnimatedRenderable graphic)
		{
			this.context.Pipeline.Remove(graphic);
			graphic.Dispose();
			if (this.isBlocking)
			{
				this.context.Executor.Continue();
			}
		}

		// Token: 0x04000931 RID: 2353
		private static readonly Dictionary<int, string> EMOTE_TYPE_SUBSPRITE_MAP = new Dictionary<int, string>
		{
			{
				0,
				"surprise"
			},
			{
				1,
				"question"
			},
			{
				2,
				"ellipses"
			},
			{
				3,
				"frustration"
			},
			{
				4,
				"vein"
			},
			{
				5,
				"idea"
			},
			{
				6,
				"music"
			}
		};

		// Token: 0x04000932 RID: 2354
		private ExecutionContext context;

		// Token: 0x04000933 RID: 2355
		private bool isBlocking;
	}
}
