using System;
using Carbine.Graphics;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.NamingMenu
{
	// Token: 0x02000040 RID: 64
	internal class NamingCharacter : Renderable
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00008FE8 File Offset: 0x000071E8
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00008FF0 File Offset: 0x000071F0
		public CharacterType Character
		{
			get
			{
				return this.character;
			}
			set
			{
				this.character = value;
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008FFC File Offset: 0x000071FC
		public NamingCharacter(CharacterType initialCharacter, int depth)
		{
			this.visible = true;
			this.state = NamingCharacter.State.WalkIn;
			this.character = initialCharacter;
			this.depth = depth;
			this.SetSprite(this.character);
			this.shadowGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "shadow.dat", ShadowSize.GetSubsprite(this.graphic.Size), this.position, this.depth - 1);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000906F File Offset: 0x0000726F
		public void SwitchCharacters(CharacterType newCharacter)
		{
			this.nextCharacter = newCharacter;
			this.state = NamingCharacter.State.TurnToLeft;
			this.timer = 0;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009088 File Offset: 0x00007288
		private void SetSprite(CharacterType newCharacter)
		{
			this.character = newCharacter;
			this.position = new Vector2f(-30f, 46f);
			if (this.graphic != null)
			{
				this.graphic.Dispose();
			}
			this.graphic = new IndexedColorGraphic(CharacterGraphics.GetFile(this.character), "walk east", this.position, this.depth);
			this.size = this.graphic.Size;
			this.origin = this.graphic.Origin;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009110 File Offset: 0x00007310
		public void Update()
		{
			this.previousPosition = this.position;
			switch (this.state)
			{
			case NamingCharacter.State.WalkIn:
				this.WalkIn();
				break;
			case NamingCharacter.State.TurnToFront:
				this.TurnToFront();
				break;
			case NamingCharacter.State.Idle:
				this.Idle();
				break;
			case NamingCharacter.State.TurnToLeft:
				this.TurnToLeft();
				break;
			case NamingCharacter.State.WalkOut:
				this.WalkOut();
				break;
			}
			if (this.previousPosition != this.position)
			{
				this.shadowGraphic.Position = this.position;
				this.graphic.Position = this.position;
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000091A8 File Offset: 0x000073A8
		private void WalkIn()
		{
			if (this.position.X < 70f)
			{
				this.position.X = this.position.X + 1f;
				return;
			}
			this.position.X = 70f;
			this.timer = 0;
			this.state = NamingCharacter.State.TurnToFront;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009200 File Offset: 0x00007400
		private void TurnToFront()
		{
			this.timer++;
			if (this.timer == 1)
			{
				this.graphic.SetSprite("walk southeast");
				return;
			}
			if (this.timer == 5)
			{
				this.graphic.SetSprite("walk south");
				return;
			}
			if (this.timer == 10)
			{
				this.timer = 0;
				this.state = NamingCharacter.State.Idle;
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009268 File Offset: 0x00007468
		private void Idle()
		{
			if (this.timer < 180)
			{
				this.timer++;
				return;
			}
			if (this.timer == 180)
			{
				this.graphic.SetSprite("smoke", true);
				this.graphic.OnAnimationComplete += this.graphic_OnAnimationComplete;
				this.timer++;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000092D4 File Offset: 0x000074D4
		private void graphic_OnAnimationComplete(AnimatedRenderable graphic)
		{
			this.timer = 0;
			this.graphic.SetSprite("walk south", true);
			this.graphic.OnAnimationComplete -= this.graphic_OnAnimationComplete;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00009308 File Offset: 0x00007508
		private void TurnToLeft()
		{
			this.timer++;
			if (this.timer == 1)
			{
				this.graphic.SetSprite("walk southwest");
				return;
			}
			if (this.timer == 5)
			{
				this.graphic.SetSprite("walk west");
				return;
			}
			if (this.timer == 10)
			{
				this.timer = 0;
				this.state = NamingCharacter.State.WalkOut;
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009370 File Offset: 0x00007570
		private void WalkOut()
		{
			if (this.position.X > -30f)
			{
				this.position.X = this.position.X - 1f;
				return;
			}
			this.position.X = -30f;
			if (this.character != this.nextCharacter)
			{
				this.SetSprite(this.nextCharacter);
				this.state = NamingCharacter.State.WalkIn;
				return;
			}
			this.state = NamingCharacter.State.Wait;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000093E5 File Offset: 0x000075E5
		public override void Draw(RenderTarget target)
		{
			this.shadowGraphic.Draw(target);
			this.graphic.Draw(target);
		}

		// Token: 0x04000247 RID: 583
		private const int STAND_Y = 46;

		// Token: 0x04000248 RID: 584
		private const int STAND_X = 70;

		// Token: 0x04000249 RID: 585
		private const int EXIT_X = -30;

		// Token: 0x0400024A RID: 586
		private const float WALK_SPEED = 1f;

		// Token: 0x0400024B RID: 587
		private NamingCharacter.State state;

		// Token: 0x0400024C RID: 588
		private Graphic shadowGraphic;

		// Token: 0x0400024D RID: 589
		private IndexedColorGraphic graphic;

		// Token: 0x0400024E RID: 590
		private CharacterType character;

		// Token: 0x0400024F RID: 591
		private CharacterType nextCharacter;

		// Token: 0x04000250 RID: 592
		private Vector2f previousPosition;

		// Token: 0x04000251 RID: 593
		private int timer;

		// Token: 0x02000041 RID: 65
		private enum State
		{
			// Token: 0x04000253 RID: 595
			WalkIn,
			// Token: 0x04000254 RID: 596
			TurnToFront,
			// Token: 0x04000255 RID: 597
			Idle,
			// Token: 0x04000256 RID: 598
			TurnToLeft,
			// Token: 0x04000257 RID: 599
			WalkOut,
			// Token: 0x04000258 RID: 600
			Wait
		}
	}
}
