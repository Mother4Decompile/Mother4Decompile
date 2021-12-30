using System;
using System.Collections.Generic;
using Carbine.Audio;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000157 RID: 343
	internal class SetBGMAction : RufiniAction
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x0003091C File Offset: 0x0002EB1C
		public SetBGMAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "bgm",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "loop",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0003098C File Offset: 0x0002EB8C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("bgm");
			bool value2 = base.GetValue<bool>("loop");
			AudioManager.Instance.SetBGM(value);
			AudioManager.Instance.BGM.LoopCount = (value2 ? -1 : 0);
			AudioManager.Instance.BGM.Play();
			return default(ActionReturnContext);
		}
	}
}
