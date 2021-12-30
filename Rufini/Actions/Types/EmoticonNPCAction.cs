using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Actors.NPCs;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000134 RID: 308
	internal class EmoticonNPCAction : RufiniAction
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x0002D404 File Offset: 0x0002B604
		public EmoticonNPCAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "name",
					Type = typeof(string)
				},
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

		// Token: 0x06000717 RID: 1815 RVA: 0x0002D49C File Offset: 0x0002B69C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("name");
			RufiniOption value2 = base.GetValue<RufiniOption>("emt");
			this.isBlocking = base.GetValue<bool>("blk");
			NPC npcByName = context.GetNpcByName(value);
			if (npcByName != null)
			{
				string spriteName = EmoticonNPCAction.EMOTE_TYPE_SUBSPRITE_MAP[0];
				EmoticonNPCAction.EMOTE_TYPE_SUBSPRITE_MAP.TryGetValue(value2.Option, out spriteName);
				IndexedColorGraphic indexedColorGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "emote.dat", spriteName, npcByName.EmoticonPoint, npcByName.Depth);
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

		// Token: 0x06000718 RID: 1816 RVA: 0x0002D56D File Offset: 0x0002B76D
		private void OnAnimationComplete(AnimatedRenderable graphic)
		{
			this.context.Pipeline.Remove(graphic);
			graphic.Dispose();
			if (this.isBlocking)
			{
				this.context.Executor.Continue();
			}
		}

		// Token: 0x0400092E RID: 2350
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

		// Token: 0x0400092F RID: 2351
		private ExecutionContext context;

		// Token: 0x04000930 RID: 2352
		private bool isBlocking;
	}
}
