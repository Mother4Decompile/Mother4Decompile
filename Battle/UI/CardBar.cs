using System;
using Carbine.Actors;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x020000E0 RID: 224
	internal class CardBar : Actor
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001FFD6 File Offset: 0x0001E1D6
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x0001FFDE File Offset: 0x0001E1DE
		public int SelectedIndex
		{
			get
			{
				return this.selIndex;
			}
			set
			{
				this.lastSelIndex = this.selIndex;
				this.selIndex = Math.Max(-1, Math.Min(this.cards.Length - 1, value));
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00020008 File Offset: 0x0001E208
		public CardBar(RenderPipeline pipeline, CharacterType[] party)
		{
			int num = party.Length;
			this.leftMargin = 160 - 63 * num / 2;
			this.idleY = (int)ViewManager.Instance.FinalTopLeft.Y + 136;
			this.cards = new BattleCard[num];
			for (int i = 0; i < this.cards.Length; i++)
			{
				Vector2f position = ViewManager.Instance.FinalTopLeft + new Vector2f((float)(this.leftMargin + 63 * i), (float)this.idleY);
				StatSet stats = CharacterStats.GetStats(party[i]);
				this.cards[i] = new BattleCard(pipeline, position, 32697, CharacterNames.GetName(party[i]), stats.HP, stats.MaxHP, stats.PP, stats.MaxPP, stats.Meter);
				this.PopCard(i, 0);
			}
			this.selIndex = -1;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00020103 File Offset: 0x0001E303
		public void PopCard(int index, int height)
		{
			this.cards[index].SetTargetY((float)(this.idleY - height));
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0002011B File Offset: 0x0001E31B
		public void SetHP(int index, int newHP)
		{
			this.cards[index].SetHP(newHP);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0002012B File Offset: 0x0001E32B
		public void SetPP(int index, int newPP)
		{
			this.cards[index].SetPP(newPP);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0002013B File Offset: 0x0001E33B
		public void SetMeter(int index, float newFill)
		{
			this.cards[index].SetMeter(newFill);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0002014B File Offset: 0x0001E34B
		public Vector2f GetCardTopMiddle(int index)
		{
			return this.cards[index].Position + new Vector2f(31f, 0f);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0002016E File Offset: 0x0001E36E
		public Graphic GetCardGraphic(int index)
		{
			return this.cards[index].CardGraphic;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0002017D File Offset: 0x0001E37D
		public void SetGroovy(int index, bool groovy)
		{
			this.cards[index].SetGroovy(groovy);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0002018D File Offset: 0x0001E38D
		public void SetSpring(int index, BattleCard.SpringMode mode, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.cards[index].SetSpring(mode, amplitude, speed, decay);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000201A2 File Offset: 0x0001E3A2
		public void AddSpring(int index, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.cards[index].AddSpring(amplitude, speed, decay);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000201B5 File Offset: 0x0001E3B5
		public void SetGlow(int index, BattleCard.GlowType type)
		{
			this.cards[index].Glow = type;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000201C8 File Offset: 0x0001E3C8
		private void SetTargetY(float y, bool instant)
		{
			for (int i = 0; i < this.cards.Length; i++)
			{
				this.cards[i].SetTargetY(y, instant);
				if (instant)
				{
					this.cards[i].Update();
				}
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00020207 File Offset: 0x0001E407
		public void Show()
		{
			this.Show(false);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00020210 File Offset: 0x0001E410
		public void Show(bool instant)
		{
			this.idleY = (int)ViewManager.Instance.FinalTopLeft.Y + 136;
			this.SetTargetY((float)this.idleY, instant);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0002023C File Offset: 0x0001E43C
		public void Hide()
		{
			this.Hide(false);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00020245 File Offset: 0x0001E445
		public void Hide(bool instant)
		{
			this.SetTargetY(ViewManager.Instance.FinalTopLeft.Y + 180f, instant);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00020263 File Offset: 0x0001E463
		public override void Input()
		{
			base.Input();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0002026C File Offset: 0x0001E46C
		public override void Update()
		{
			base.Update();
			for (int i = 0; i < this.cards.Length; i++)
			{
				this.cards[i].Update();
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000202A0 File Offset: 0x0001E4A0
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			for (int i = 0; i < this.cards.Length; i++)
			{
				this.cards[i].Dispose();
			}
		}

		// Token: 0x040006FA RID: 1786
		private const int SPACING = 63;

		// Token: 0x040006FB RID: 1787
		private const int IDLE_Y_OFFSET = 136;

		// Token: 0x040006FC RID: 1788
		private const int DEPTH = 32697;

		// Token: 0x040006FD RID: 1789
		private BattleCard[] cards;

		// Token: 0x040006FE RID: 1790
		private readonly int leftMargin;

		// Token: 0x040006FF RID: 1791
		private int idleY;

		// Token: 0x04000700 RID: 1792
		private int selIndex;

		// Token: 0x04000701 RID: 1793
		private int lastSelIndex;
	}
}
