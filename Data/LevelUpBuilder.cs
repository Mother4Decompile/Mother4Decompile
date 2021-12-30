using System;
using System.Collections.Generic;
using System.Text;
using Mother4.Battle;

namespace Mother4.Data
{
	// Token: 0x0200007A RID: 122
	internal class LevelUpBuilder
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0001009D File Offset: 0x0000E29D
		public LevelUpBuilder(CharacterType[] party)
		{
			this.increases = new Dictionary<CharacterType, StatSet>();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000100B0 File Offset: 0x0000E2B0
		public string GetLevelUpString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("@Travis[t:1,0] reached level 61!");
			stringBuilder.AppendLine("@Offense went up by 1!");
			stringBuilder.AppendLine("@Guts went up by 2!");
			stringBuilder.AppendLine("@Floyd[t:1,2] reached level 59!");
			stringBuilder.AppendLine("@Offense went up by 1!");
			stringBuilder.AppendLine("@Right on[p:15]!");
			stringBuilder.AppendLine("Speed went up by 6!");
			stringBuilder.AppendLine("@Maximum HP went up by [t:2,1,5]5!");
			stringBuilder.AppendLine("@Meryl[t:1,1] reached level 60!");
			stringBuilder.AppendLine("@Defense went up 2!");
			stringBuilder.AppendLine("@Speed went up by 1!");
			stringBuilder.AppendLine("@Oh, far out[p:15]!");
			stringBuilder.AppendLine("Guts went up by 4!");
			stringBuilder.AppendLine("@Maximum HP went up by [t:2,2,11]11!");
			stringBuilder.AppendLine("@Maximum PP Went up by [t:3,2,8]8!");
			stringBuilder.AppendLine("@Leo[t:1,3] reached level 55!");
			stringBuilder.AppendLine("@Oh, far out[p:15]!");
			stringBuilder.AppendLine("Offense went up by 5!");
			stringBuilder.AppendLine("@Defense went up by 2!");
			stringBuilder.AppendLine("@Maximum HP went up by [t:2,3,12]12!");
			stringBuilder.AppendLine("@Maximum PP went up by [t:3,3,10]10!");
			return stringBuilder.ToString();
		}

		// Token: 0x040003F2 RID: 1010
		private Dictionary<CharacterType, StatSet> increases;
	}
}
