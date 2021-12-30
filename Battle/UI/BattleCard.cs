using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Utility;
using Mother4.Data;
using Mother4.GUI;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x020000D6 RID: 214
	internal class BattleCard : IDisposable
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001EB3B File Offset: 0x0001CD3B
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0001EB43 File Offset: 0x0001CD43
		public Graphic CardGraphic
		{
			get
			{
				return this.card;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001EB4B File Offset: 0x0001CD4B
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0001EB53 File Offset: 0x0001CD53
		public BattleCard.GlowType Glow
		{
			get
			{
				return this.glowType;
			}
			set
			{
				this.SetGlow(value);
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001EB5C File Offset: 0x0001CD5C
		public BattleCard(RenderPipeline pipeline, Vector2f position, int depth, string name, int hp, int maxHp, int pp, int maxPp, float meterFill)
		{
			this.position = position;
			this.card = new IndexedColorGraphic(BattleCard.BATTLEUI_DAT, "card", position, depth);
			this.card.CurrentPalette = Settings.WindowFlavor;
			this.hpLabel = new IndexedColorGraphic(BattleCard.BATTLEUI_DAT, "hp", position + BattleCard.HPLABEL_POSITION, depth + 2);
			this.hpLabel.CurrentPalette = Settings.WindowFlavor;
			this.ppLabel = new IndexedColorGraphic(BattleCard.BATTLEUI_DAT, "pp", position + BattleCard.PPLABEL_POSITION, depth + 2);
			this.ppLabel.CurrentPalette = Settings.WindowFlavor;
			this.nameTag = new TextRegion(position, depth + 2, Fonts.Main, name);
			this.nameTag.Color = Color.Black;
			this.nametagX = (int)((float)(this.card.TextureRect.Width / 2) - this.nameTag.Size.X / 2f);
			this.nameTag.Position = position + new Vector2f((float)this.nametagX, 6f) + BattleCard.NAME_POSITION;
			pipeline.Add(this.card);
			pipeline.Add(this.hpLabel);
			pipeline.Add(this.ppLabel);
			pipeline.Add(this.nameTag);
			this.meter = new BattleMeter(pipeline, position + BattleCard.METER_OFFSET, meterFill, depth + 1);
			this.odoHP = new Odometer(pipeline, position + BattleCard.HPODO_POSITION, depth + 2, 3, hp, maxHp);
			this.odoPP = new Odometer(pipeline, position + BattleCard.PPODO_POSITION, depth + 2, 3, pp, maxPp);
			this.springMode = BattleCard.SpringMode.Normal;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001ED18 File Offset: 0x0001CF18
		~BattleCard()
		{
			this.Dispose(false);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001ED48 File Offset: 0x0001CF48
		public void SetHP(int newHP)
		{
			this.odoHP.SetValue(newHP);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001ED56 File Offset: 0x0001CF56
		public void SetPP(int newPP)
		{
			this.odoPP.SetValue(newPP);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001ED64 File Offset: 0x0001CF64
		public void SetMeter(float newFill)
		{
			this.meter.SetFill(newFill);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001ED72 File Offset: 0x0001CF72
		public void SetGroovy(bool groovy)
		{
			this.meter.SetGroovy(groovy);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001ED80 File Offset: 0x0001CF80
		public void SetTargetY(float newTargetY)
		{
			this.SetTargetY(newTargetY, false);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001ED8A File Offset: 0x0001CF8A
		public void SetTargetY(float newTargetY, bool instant)
		{
			this.targetY = newTargetY;
			if (instant)
			{
				this.position.Y = this.targetY;
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001EDA8 File Offset: 0x0001CFA8
		public void SetSpring(BattleCard.SpringMode mode, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.springMode = mode;
			this.xSpring = 0f;
			this.xDampTarget = amplitude.X;
			this.xSpeedTarget = speed.X;
			this.xDecayTarget = decay.X;
			this.ySpring = 0f;
			this.yDampTarget = amplitude.Y;
			this.ySpeedTarget = speed.Y;
			this.yDecayTarget = decay.Y;
			this.ramping = true;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001EE28 File Offset: 0x0001D028
		public void AddSpring(Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.xDampTarget += amplitude.X;
			this.xSpeedTarget += speed.X;
			this.xDecayTarget += decay.X;
			this.yDampTarget += amplitude.Y;
			this.ySpeedTarget += speed.Y;
			this.yDecayTarget += decay.Y;
			this.ramping = true;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001EEB4 File Offset: 0x0001D0B4
		private void UpdateSpring()
		{
			if (this.ramping)
			{
				this.xDamp += (this.xDampTarget - this.xDamp) / 2f;
				this.xSpeed += (this.xSpeedTarget - this.xSpeed) / 2f;
				this.xDecay += (this.xDecayTarget - this.xDecay) / 2f;
				this.yDamp += (this.yDampTarget - this.yDamp) / 2f;
				this.ySpeed += (this.ySpeedTarget - this.ySpeed) / 2f;
				this.yDecay += (this.yDecayTarget - this.yDecay) / 2f;
				if ((int)this.xDamp == (int)this.xDampTarget && (int)this.xSpeed == (int)this.xSpeedTarget && (int)this.xDecay == (int)this.xDecayTarget && (int)this.yDamp == (int)this.yDampTarget && (int)this.ySpeed == (int)this.ySpeedTarget && (int)this.yDecay == (int)this.yDecayTarget)
				{
					this.ramping = false;
				}
			}
			else
			{
				this.xDamp = ((this.xDamp > 0.5f) ? (this.xDamp * this.xDecay) : 0f);
				this.yDamp = ((this.yDamp > 0.5f) ? (this.yDamp * this.yDecay) : 0f);
			}
			this.xSpring += this.xSpeed;
			this.ySpring += this.ySpeed;
			this.offset.X = (float)Math.Sin((double)this.xSpring) * this.xDamp;
			this.offset.Y = (float)Math.Sin((double)this.ySpring) * this.yDamp;
			if (this.springMode == BattleCard.SpringMode.BounceUp)
			{
				this.offset.Y = -Math.Abs(this.offset.Y);
				return;
			}
			if (this.springMode == BattleCard.SpringMode.BounceDown)
			{
				this.offset.Y = Math.Abs(this.offset.Y);
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001F0FC File Offset: 0x0001D2FC
		public void SetGlow(BattleCard.GlowType type)
		{
			this.glowType = type;
			BattleCard.GlowSettings glowSettings = BattleCard.GLOW_COLORS[this.glowType];
			this.glowColor = ColorHelper.Blend(glowSettings.Color, Color.Black, 0.75f);
			this.glowSpeed = 0.05f + (float)(Engine.Random.Next(20) - 10) / 1000f;
			this.glowDelta = 0f;
			this.card.ColorBlendMode = glowSettings.BlendMode;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001F17C File Offset: 0x0001D37C
		private void UpdatePosition()
		{
			if (this.position.Y < this.targetY - 0.5f)
			{
				this.position.Y = this.position.Y + (this.targetY - this.position.Y) / 2f;
				return;
			}
			if (this.position.Y > this.targetY + 0.5f)
			{
				this.position.Y = this.position.Y + (this.targetY - this.position.Y) / 2f;
				return;
			}
			if ((int)this.position.Y != (int)this.targetY)
			{
				this.position.Y = this.targetY;
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001F238 File Offset: 0x0001D438
		private void MoveGraphics(Vector2f gPosition)
		{
			gPosition.X = (float)((int)gPosition.X);
			gPosition.Y = (float)((int)gPosition.Y);
			this.card.Position = gPosition;
			this.hpLabel.Position = gPosition + BattleCard.HPLABEL_POSITION;
			this.ppLabel.Position = gPosition + BattleCard.PPLABEL_POSITION;
			this.nameTag.Position = gPosition + new Vector2f((float)this.nametagX, 6f) + BattleCard.NAME_POSITION;
			this.meter.Position = gPosition + BattleCard.METER_OFFSET;
			this.odoHP.Position = gPosition + BattleCard.HPODO_POSITION;
			this.odoPP.Position = gPosition + BattleCard.PPODO_POSITION;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001F30C File Offset: 0x0001D50C
		private void UpdateGlow()
		{
			if (this.glowType != BattleCard.GlowType.None)
			{
				this.glowDelta += this.glowSpeed;
				float amount = (float)Math.Sin((double)this.glowDelta) / 2f + 0.5f;
				this.card.Color = ColorHelper.Blend(Color.Black, this.glowColor, amount);
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001F36C File Offset: 0x0001D56C
		public void Update()
		{
			this.UpdateGlow();
			this.UpdateSpring();
			this.UpdatePosition();
			this.MoveGraphics(this.position + this.offset);
			this.odoHP.Update();
			this.odoPP.Update();
			this.meter.Update();
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001F3C3 File Offset: 0x0001D5C3
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001F3D4 File Offset: 0x0001D5D4
		public void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.card.Dispose();
					this.hpLabel.Dispose();
					this.ppLabel.Dispose();
					this.odoHP.Dispose();
					this.odoPP.Dispose();
					this.meter.Dispose();
					this.nameTag.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x04000694 RID: 1684
		private const float DAMP_HIGHPASS = 0.5f;

		// Token: 0x04000695 RID: 1685
		private const float GLOW_SPEED = 0.05f;

		// Token: 0x04000696 RID: 1686
		private const float GLOW_INTENSITY = 0.75f;

		// Token: 0x04000697 RID: 1687
		private static readonly Dictionary<BattleCard.GlowType, BattleCard.GlowSettings> GLOW_COLORS = new Dictionary<BattleCard.GlowType, BattleCard.GlowSettings>
		{
			{
				BattleCard.GlowType.None,
				new BattleCard.GlowSettings(Color.White, ColorBlendMode.Multiply)
			},
			{
				BattleCard.GlowType.Shield,
				new BattleCard.GlowSettings(new Color(206, 226, 234), ColorBlendMode.Add)
			},
			{
				BattleCard.GlowType.Counter,
				new BattleCard.GlowSettings(new Color(byte.MaxValue, 249, 119), ColorBlendMode.Add)
			},
			{
				BattleCard.GlowType.PsiSheild,
				new BattleCard.GlowSettings(new Color(120, 232, 252), ColorBlendMode.Add)
			},
			{
				BattleCard.GlowType.PsiCounter,
				new BattleCard.GlowSettings(new Color(219, 121, 251), ColorBlendMode.Add)
			},
			{
				BattleCard.GlowType.Eraser,
				new BattleCard.GlowSettings(new Color(247, 136, 136), ColorBlendMode.Add)
			}
		};

		// Token: 0x04000698 RID: 1688
		private static readonly string BATTLEUI_DAT = Paths.GRAPHICS + "battleui.dat";

		// Token: 0x04000699 RID: 1689
		private static readonly Vector2f HPLABEL_POSITION = new Vector2f(10f, 23f);

		// Token: 0x0400069A RID: 1690
		private static readonly Vector2f PPLABEL_POSITION = new Vector2f(10f, 34f);

		// Token: 0x0400069B RID: 1691
		private static readonly Vector2f HPODO_POSITION = new Vector2f(28f, 22f);

		// Token: 0x0400069C RID: 1692
		private static readonly Vector2f PPODO_POSITION = new Vector2f(28f, 33f);

		// Token: 0x0400069D RID: 1693
		private static readonly Vector2f METER_OFFSET = new Vector2f(1f, 1f);

		// Token: 0x0400069E RID: 1694
		private static readonly Vector2f NAME_POSITION = new Vector2f(0f, 2f);

		// Token: 0x0400069F RID: 1695
		private bool disposed;

		// Token: 0x040006A0 RID: 1696
		private IndexedColorGraphic card;

		// Token: 0x040006A1 RID: 1697
		private IndexedColorGraphic hpLabel;

		// Token: 0x040006A2 RID: 1698
		private IndexedColorGraphic ppLabel;

		// Token: 0x040006A3 RID: 1699
		private TextRegion nameTag;

		// Token: 0x040006A4 RID: 1700
		private int nametagX;

		// Token: 0x040006A5 RID: 1701
		private BattleMeter meter;

		// Token: 0x040006A6 RID: 1702
		private Odometer odoHP;

		// Token: 0x040006A7 RID: 1703
		private Odometer odoPP;

		// Token: 0x040006A8 RID: 1704
		private BattleCard.SpringMode springMode;

		// Token: 0x040006A9 RID: 1705
		private Vector2f position;

		// Token: 0x040006AA RID: 1706
		private Vector2f offset;

		// Token: 0x040006AB RID: 1707
		private float xSpring;

		// Token: 0x040006AC RID: 1708
		private float ySpring;

		// Token: 0x040006AD RID: 1709
		private float xSpeed;

		// Token: 0x040006AE RID: 1710
		private float xSpeedTarget;

		// Token: 0x040006AF RID: 1711
		private float ySpeed;

		// Token: 0x040006B0 RID: 1712
		private float ySpeedTarget;

		// Token: 0x040006B1 RID: 1713
		private float xDamp;

		// Token: 0x040006B2 RID: 1714
		private float xDampTarget;

		// Token: 0x040006B3 RID: 1715
		private float yDamp;

		// Token: 0x040006B4 RID: 1716
		private float yDampTarget;

		// Token: 0x040006B5 RID: 1717
		private float xDecay;

		// Token: 0x040006B6 RID: 1718
		private float xDecayTarget;

		// Token: 0x040006B7 RID: 1719
		private float yDecay;

		// Token: 0x040006B8 RID: 1720
		private float yDecayTarget;

		// Token: 0x040006B9 RID: 1721
		private bool ramping;

		// Token: 0x040006BA RID: 1722
		private float targetY;

		// Token: 0x040006BB RID: 1723
		private BattleCard.GlowType glowType;

		// Token: 0x040006BC RID: 1724
		private Color glowColor;

		// Token: 0x040006BD RID: 1725
		private float glowSpeed;

		// Token: 0x040006BE RID: 1726
		private float glowDelta;

		// Token: 0x020000D7 RID: 215
		public enum SpringMode
		{
			// Token: 0x040006C0 RID: 1728
			Normal,
			// Token: 0x040006C1 RID: 1729
			BounceUp,
			// Token: 0x040006C2 RID: 1730
			BounceDown
		}

		// Token: 0x020000D8 RID: 216
		public enum GlowType
		{
			// Token: 0x040006C4 RID: 1732
			None,
			// Token: 0x040006C5 RID: 1733
			Shield,
			// Token: 0x040006C6 RID: 1734
			Counter,
			// Token: 0x040006C7 RID: 1735
			PsiSheild,
			// Token: 0x040006C8 RID: 1736
			PsiCounter,
			// Token: 0x040006C9 RID: 1737
			Eraser
		}

		// Token: 0x020000D9 RID: 217
		public enum OverlayType
		{
			// Token: 0x040006CB RID: 1739
			None,
			// Token: 0x040006CC RID: 1740
			Diamondized
		}

		// Token: 0x020000DA RID: 218
		private struct GlowSettings
		{
			// Token: 0x060004EE RID: 1262 RVA: 0x0001F593 File Offset: 0x0001D793
			public GlowSettings(Color color, ColorBlendMode mode)
			{
				this.Color = color;
				this.BlendMode = mode;
			}

			// Token: 0x040006CD RID: 1741
			public Color Color;

			// Token: 0x040006CE RID: 1742
			public ColorBlendMode BlendMode;
		}
	}
}
