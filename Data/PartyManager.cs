using System;
using System.Collections.Generic;

namespace Mother4.Data
{
	// Token: 0x02000085 RID: 133
	internal class PartyManager
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x000112C3 File Offset: 0x0000F4C3
		public static PartyManager Instance
		{
			get
			{
				if (PartyManager.instance == null)
				{
					PartyManager.instance = new PartyManager();
				}
				return PartyManager.instance;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x000112DB File Offset: 0x0000F4DB
		public int Count
		{
			get
			{
				return this.party.Count;
			}
		}

		// Token: 0x1700007F RID: 127
		public CharacterType this[int i]
		{
			get
			{
				return this.party[i];
			}
			set
			{
				this.party[i] = value;
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060002B8 RID: 696 RVA: 0x00011308 File Offset: 0x0000F508
		// (remove) Token: 0x060002B9 RID: 697 RVA: 0x00011340 File Offset: 0x0000F540
		public event PartyManager.OnPartyChangeHandler OnPartyChange;

		// Token: 0x060002BA RID: 698 RVA: 0x00011375 File Offset: 0x0000F575
		private PartyManager()
		{
			this.party = new List<CharacterType>();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00011388 File Offset: 0x0000F588
		public void Clear()
		{
			this.party.Clear();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00011398 File Offset: 0x0000F598
		public void Add(CharacterType character)
		{
			this.party.Add(character);
			if (this.OnPartyChange != null)
			{
				PartyManager.PartyChangeArgs args = new PartyManager.PartyChangeArgs
				{
					Added = new CharacterType?(character)
				};
				this.OnPartyChange(args);
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000113DC File Offset: 0x0000F5DC
		public void AddAll(ICollection<CharacterType> characters)
		{
			foreach (CharacterType character in characters)
			{
				this.Add(character);
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00011424 File Offset: 0x0000F624
		public void Insert(int index, CharacterType character)
		{
			this.party.Insert(index, character);
			if (this.OnPartyChange != null)
			{
				PartyManager.PartyChangeArgs args = new PartyManager.PartyChangeArgs
				{
					Added = new CharacterType?(character)
				};
				this.OnPartyChange(args);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0001146C File Offset: 0x0000F66C
		public void Remove(CharacterType character)
		{
			this.party.Remove(character);
			if (this.OnPartyChange != null)
			{
				PartyManager.PartyChangeArgs args = new PartyManager.PartyChangeArgs
				{
					Removed = new CharacterType?(character)
				};
				this.OnPartyChange(args);
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000114B1 File Offset: 0x0000F6B1
		public int IndexOf(CharacterType character)
		{
			return this.party.IndexOf(character);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000114BF File Offset: 0x0000F6BF
		public CharacterType[] ToArray()
		{
			return this.party.ToArray();
		}

		// Token: 0x0400041F RID: 1055
		private static PartyManager instance;

		// Token: 0x04000420 RID: 1056
		private List<CharacterType> party;

		// Token: 0x02000086 RID: 134
		public struct PartyChangeArgs
		{
			// Token: 0x04000422 RID: 1058
			public CharacterType? Added;

			// Token: 0x04000423 RID: 1059
			public CharacterType? Removed;
		}

		// Token: 0x02000087 RID: 135
		// (Invoke) Token: 0x060002C3 RID: 707
		public delegate void OnPartyChangeHandler(PartyManager.PartyChangeArgs args);
	}
}
