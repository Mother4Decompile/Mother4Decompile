using System;
using System.Collections.Generic;
using Carbine.Utility;
using Mother4.Battle;
using Mother4.Battle.UI;
using Mother4.Data.Character;

namespace Mother4.Data
{
	// Token: 0x02000080 RID: 128
	internal static class BattleButtonBars
	{
		// Token: 0x060002AA RID: 682 RVA: 0x00010B80 File Offset: 0x0000ED80
		public static ButtonBar.Action[] GetActions(CharacterType character, bool showRun)
		{
			List<ButtonBar.Action> list = new List<ButtonBar.Action>();
			list.Add(ButtonBar.Action.Bash);
			if (character.Identifier == Hash.Get("character.party.floyd"))
			{
				list.Add(ButtonBar.Action.Talk);
			}
			else if (character.Identifier != Hash.Get("character.party.zack"))
			{
				StatSet stats = CharacterStats.GetStats(character);
				CharacterData data = CharacterFile.Instance.GetData(character);
				if (data.HasPsi(stats.Level))
				{
					list.Add(ButtonBar.Action.Psi);
				}
			}
			list.Add(ButtonBar.Action.Items);
			list.Add(ButtonBar.Action.Guard);
			if (showRun)
			{
				list.Add(ButtonBar.Action.Run);
			}
			return list.ToArray();
		}
	}
}
