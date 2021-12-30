using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.GUI;
using Mother4.GUI;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;
using SFML.Graphics;

namespace Rufini.Actions.Types
{
	// Token: 0x0200013A RID: 314
	internal class FlyoverTextAction : RufiniAction
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x0002E260 File Offset: 0x0002C460
		public FlyoverTextAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "txt",
					Type = typeof(RufiniString)
				},
				new ActionParam
				{
					Name = "loc",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "fnt",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "tcol",
					Type = typeof(Color)
				},
				new ActionParam
				{
					Name = "bcol",
					Type = typeof(Color)
				},
				new ActionParam
				{
					Name = "tdur",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "hdur",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0002E3D4 File Offset: 0x0002C5D4
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			RufiniString value = base.GetValue<RufiniString>("txt");
			string value2 = StringFile.Instance.Get(value.Names).Value;
			RufiniOption value3 = base.GetValue<RufiniOption>("loc");
			RufiniOption value4 = base.GetValue<RufiniOption>("fnt");
			Color value5 = base.GetValue<Color>("tcol");
			Color value6 = base.GetValue<Color>("bcol");
			int value7 = base.GetValue<int>("tdur");
			int value8 = base.GetValue<int>("hdur");
			this.isBlocking = base.GetValue<bool>("blk");
			this.context = context;
			this.manager = context.ActorManager;
			FontData font;
			switch (value4.Option)
			{
			case 1:
				font = Fonts.Title;
				break;
			case 2:
				font = Fonts.Saturn;
				break;
			default:
				font = Fonts.Main;
				break;
			}
			this.text = new FlyoverText(this.context.Pipeline, font, value2, (FlyoverText.TextPosition)value3.Option, value6, value5, value7, value8);
			this.text.OnCompletion += this.Continue;
			this.manager.Add(this.text);
			return new ActionReturnContext
			{
				Wait = (this.isBlocking ? ScriptExecutor.WaitType.Event : ScriptExecutor.WaitType.None)
			};
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0002E51C File Offset: 0x0002C71C
		private void Continue()
		{
			this.manager.Remove(this.text);
			this.manager = null;
			this.text.Dispose();
			this.text = null;
			if (this.isBlocking)
			{
				this.context.Executor.Continue();
			}
			this.context = null;
		}

		// Token: 0x0400093C RID: 2364
		private ExecutionContext context;

		// Token: 0x0400093D RID: 2365
		private ActorManager manager;

		// Token: 0x0400093E RID: 2366
		private bool isBlocking;

		// Token: 0x0400093F RID: 2367
		private FlyoverText text;
	}
}
