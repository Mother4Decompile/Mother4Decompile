using System;
using System.Collections.Generic;
using Carbine.Audio;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x0200014F RID: 335
	internal class PlaySFXAction : RufiniAction
	{
		// Token: 0x06000758 RID: 1880 RVA: 0x0002FE70 File Offset: 0x0002E070
		public PlaySFXAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "sfx",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "loop",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "bal",
					Type = typeof(float)
				}
			};
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0002FF08 File Offset: 0x0002E108
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("sfx");
			bool value2 = base.GetValue<bool>("loop");
			base.GetValue<float>("bal");
			CarbineSound carbineSound = AudioManager.Instance.Use(value, AudioType.Sound);
			carbineSound.LoopCount = (value2 ? -1 : 0);
			carbineSound.OnComplete += this.SoundComplete;
			carbineSound.Play();
			return default(ActionReturnContext);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0002FF74 File Offset: 0x0002E174
		private void SoundComplete(CarbineSound sender)
		{
			AudioManager.Instance.Unuse(sender);
		}
	}
}
