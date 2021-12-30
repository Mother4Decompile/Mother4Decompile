using System;
using Mother4.Utility;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000062 RID: 98
	internal class TalkStatusEffectAction : StatusEffectAction
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000DFAC File Offset: 0x0000C1AC
		public TalkStatusEffectAction(ActionParams aparams) : base(aparams)
		{
			string arg = string.Empty;
			object obj = null;
			this.controller.Data.TryGetValue("topicOfDiscussion", out obj);
			if (obj is string)
			{
				arg = (obj as string);
			}
			string arg2 = Capitalizer.Capitalize(base.BuildCombatantName(this.sender));
			string arg3 = Capitalizer.Capitalize(base.BuildCombatantName(this.targets[0]));
			if (this.effect.TurnsRemaining > 1)
			{
				this.message = string.Format("@{0} is busy talking about {1} with {2}.", arg2, arg, arg3);
			}
			else
			{
				this.message = string.Format("@{0} got bored of talking about {1} with {2}.", arg2, arg, arg3);
			}
			this.actionStartSound = this.controller.InterfaceController.TalkSound;
		}

		// Token: 0x0400034C RID: 844
		private const string TOPIC_OF_DISCUSSION = "topicOfDiscussion";
	}
}
