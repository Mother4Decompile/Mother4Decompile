using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using SFML.System;

namespace Rufini.Actions.Types
{
	// Token: 0x02000156 RID: 342
	internal class ScreenShakeAction : RufiniAction
	{
		// Token: 0x0600076C RID: 1900 RVA: 0x000307DC File Offset: 0x0002E9DC
		public ScreenShakeAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "pow",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "dur",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "x",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "y",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000308A0 File Offset: 0x0002EAA0
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			bool value = base.GetValue<bool>("x");
			bool value2 = base.GetValue<bool>("y");
			int value3 = base.GetValue<int>("dur");
			float num = (float)base.GetValue<int>("pow");
			Vector2f intensity = new Vector2f(value ? num : 0f, value2 ? num : 0f);
			ViewManager.Instance.Shake(intensity, (float)value3 / 60f);
			return default(ActionReturnContext);
		}
	}
}
