using System;
using Carbine.Graphics;
using SFML.System;

namespace Mother4.GUI.Modifiers
{
	// Token: 0x02000090 RID: 144
	internal class GraphicBouncer : IGraphicModifier
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00012B20 File Offset: 0x00010D20
		public bool Done
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00012B28 File Offset: 0x00010D28
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00012B30 File Offset: 0x00010D30
		public GraphicBouncer(Graphic graphic, GraphicBouncer.SpringMode mode, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.graphic = graphic;
			this.position = graphic.Position;
			this.SetSpring(mode, amplitude, speed, decay);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00012B58 File Offset: 0x00010D58
		public void SetSpring(GraphicBouncer.SpringMode mode, Vector2f amplitude, Vector2f speed, Vector2f decay)
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

		// Token: 0x060002F2 RID: 754 RVA: 0x00012BD8 File Offset: 0x00010DD8
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
			if (this.springMode == GraphicBouncer.SpringMode.BounceUp)
			{
				this.offset.Y = -Math.Abs(this.offset.Y);
				return;
			}
			if (this.springMode == GraphicBouncer.SpringMode.BounceDown)
			{
				this.offset.Y = Math.Abs(this.offset.Y);
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00012E20 File Offset: 0x00011020
		public void Update()
		{
			if (!this.done)
			{
				this.UpdateSpring();
				if (this.xDamp > 0.5f || this.yDamp > 0.5f)
				{
					this.graphic.Position = this.position + this.offset;
					return;
				}
				this.done = true;
				this.graphic.Position = this.position;
			}
		}

		// Token: 0x04000442 RID: 1090
		private const float DAMP_HIGHPASS = 0.5f;

		// Token: 0x04000443 RID: 1091
		private bool done;

		// Token: 0x04000444 RID: 1092
		private Graphic graphic;

		// Token: 0x04000445 RID: 1093
		private GraphicBouncer.SpringMode springMode;

		// Token: 0x04000446 RID: 1094
		private Vector2f position;

		// Token: 0x04000447 RID: 1095
		private Vector2f offset;

		// Token: 0x04000448 RID: 1096
		private float xSpring;

		// Token: 0x04000449 RID: 1097
		private float ySpring;

		// Token: 0x0400044A RID: 1098
		private float xSpeed;

		// Token: 0x0400044B RID: 1099
		private float xSpeedTarget;

		// Token: 0x0400044C RID: 1100
		private float ySpeed;

		// Token: 0x0400044D RID: 1101
		private float ySpeedTarget;

		// Token: 0x0400044E RID: 1102
		private float xDamp;

		// Token: 0x0400044F RID: 1103
		private float xDampTarget;

		// Token: 0x04000450 RID: 1104
		private float yDamp;

		// Token: 0x04000451 RID: 1105
		private float yDampTarget;

		// Token: 0x04000452 RID: 1106
		private float xDecay;

		// Token: 0x04000453 RID: 1107
		private float xDecayTarget;

		// Token: 0x04000454 RID: 1108
		private float yDecay;

		// Token: 0x04000455 RID: 1109
		private float yDecayTarget;

		// Token: 0x04000456 RID: 1110
		private bool ramping;

		// Token: 0x02000091 RID: 145
		public enum SpringMode
		{
			// Token: 0x04000458 RID: 1112
			Normal,
			// Token: 0x04000459 RID: 1113
			BounceUp,
			// Token: 0x0400045A RID: 1114
			BounceDown
		}
	}
}
