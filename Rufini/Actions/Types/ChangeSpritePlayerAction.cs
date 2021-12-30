using System;
using System.Collections.Generic;
using System.IO;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000130 RID: 304
	internal class ChangeSpritePlayerAction : RufiniAction
	{
		// Token: 0x0600070E RID: 1806 RVA: 0x0002D14C File Offset: 0x0002B34C
		public ChangeSpritePlayerAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "spr",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "sub",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002D1BC File Offset: 0x0002B3BC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("spr");
			string value2 = base.GetValue<string>("sub");
			if (context.Player != null && value.Length > 0)
			{
				string text = Paths.GRAPHICS + value + ".dat";
				if (File.Exists(text))
				{
					context.Player.ChangeSprite(text, value2);
				}
				else
				{
					Console.WriteLine("Sprite file \"{0}\" does not exist.", text);
				}
			}
			return default(ActionReturnContext);
		}
	}
}
