using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x02000070 RID: 112
	internal class ComboAnimator : IDisposable
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600025F RID: 607 RVA: 0x0000EE78 File Offset: 0x0000D078
		// (remove) Token: 0x06000260 RID: 608 RVA: 0x0000EEB0 File Offset: 0x0000D0B0
		public event ComboAnimator.AnimationCompleteHandler OnAnimationComplete;

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000EEE5 File Offset: 0x0000D0E5
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000EEED File Offset: 0x0000D0ED
		public int Depth
		{
			get
			{
				return this.depth;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000EEF5 File Offset: 0x0000D0F5
		public bool Stopped
		{
			get
			{
				return this.state == ComboAnimator.State.Stopped;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000EF00 File Offset: 0x0000D100
		public ComboAnimator(RenderPipeline pipeline, int depth)
		{
			this.pipeline = pipeline;
			this.depth = depth;
			this.position = default(Vector2f);
			this.size = default(Vector2f);
			this.starGraphics = new IndexedColorGraphic[16];
			this.bounceFlag = new bool[16];
			this.starVelocity = new Vector2f[16];
			Vector2f vector2f = new Vector2f(-320f, -180f);
			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starGraphics[i] = new IndexedColorGraphic(ComboAnimator.HITSPARK_RESOURCE, "star", vector2f, depth);
				this.starGraphics[i].Visible = false;
				pipeline.Add(this.starGraphics[i]);
				this.starVelocity[i] = new Vector2f(0f, 0f);
			}
			this.damageNumbers = new List<DamageNumber>();
			this.damageNumbersToRemove = new List<DamageNumber>();
			this.totalDamageNumber = new TotalDamageNumber(pipeline, this.position, 0);
			this.totalDamageNumber.SetVisibility(false);
			this.hitsparks = new Graphic[2];
			for (int j = 0; j < this.hitsparks.Length; j++)
			{
				this.hitsparks[j] = new IndexedColorGraphic(ComboAnimator.HITSPARK_RESOURCE, "combohitspark", vector2f, depth + 20);
				this.hitsparks[j].Visible = false;
				pipeline.Add(this.hitsparks[j]);
			}
			this.state = ComboAnimator.State.Stopped;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000F06C File Offset: 0x0000D26C
		~ComboAnimator()
		{
			this.Dispose(false);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000F09C File Offset: 0x0000D29C
		public void Setup(Graphic graphic)
		{
			this.wobble = 0f;
			this.wobbleSpeed = 0f;
			this.wobbleDamp = 0f;
			this.enemyGraphic = graphic;
			this.initialEnemyPosition = graphic.Position;
			this.bounce = 0f;
			this.bounceSpeed = 0f;
			this.position = this.enemyGraphic.Position - this.enemyGraphic.Origin + new Vector2f((float)this.enemyGraphic.TextureRect.Width / 2f, (float)this.enemyGraphic.TextureRect.Height / 6f);
			this.position.X = (float)((int)this.position.X);
			this.position.Y = (float)((int)this.position.Y);
			this.size = new Vector2f((float)this.enemyGraphic.TextureRect.Width * 0.6f, 2f);
			this.depth = this.enemyGraphic.Depth;
			this.starCount = 0;
			this.rotAngle = 0f;
			this.modAngle = 0f;
			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starGraphics[i].Visible = false;
				this.bounceFlag[i] = false;
			}
			Vector2f v = new Vector2f(0f, (float)this.enemyGraphic.TextureRect.Height * 0.9f);
			this.totalDamageNumber.Reset(this.position + v, 0);
			this.state = ComboAnimator.State.Circling;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000F240 File Offset: 0x0000D440
		public void Stop(bool explode)
		{
			this.state = ComboAnimator.State.Falling;
			float x = explode ? 7.5f : 100f;
			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starVelocity[i] = (this.starGraphics[i].Position - this.position) / x;
			}
			this.totalDamageNumber.Start();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
		private void End()
		{
			this.state = ComboAnimator.State.Stopped;
			this.enemyGraphic.Rotation = 0f;
			this.enemyGraphic = null;
			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starGraphics[i].Position = new Vector2f(-320f, -180f);
				this.starGraphics[i].Visible = false;
			}
			if (this.OnAnimationComplete != null)
			{
				this.OnAnimationComplete(this.starCount);
			}
			this.starCount = 0;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000F33C File Offset: 0x0000D53C
		public void AddHit(int damage, bool smash)
		{
			this.AddStar();
			this.wobbleSpeed = (smash ? 1.2f : 0.7f);
			this.wobbleDamp = (smash ? 4f : 2f) * 3.14159f;
			if (smash)
			{
				this.bounceSpeed = -4f;
			}
			Vector2f vector2f = this.position - this.size / 2f + new Vector2f((float)((int)(Engine.Random.NextDouble() * (double)this.size.X)), (float)((int)(Engine.Random.NextDouble() * (double)this.size.Y)));
			DamageNumber damageNumber = new DamageNumber(this.pipeline, vector2f, new Vector2f(0f, -20f), 50, damage);
			damageNumber.SetVisibility(true);
			damageNumber.OnComplete += this.DamageNumberComplete;
			this.damageNumbers.Add(damageNumber);
			damageNumber.Start();
			this.totalDamageNumber.AddToNumber(damage);
			this.totalDamageNumber.SetVisibility(true);
			Vector2f vector2f2 = new Vector2f(vector2f.X, this.enemyGraphic.Position.Y - this.enemyGraphic.Origin.Y + (float)((int)(Engine.Random.NextDouble() * (double)this.enemyGraphic.TextureRect.Height)));
			this.hitsparks[this.hitsparkIndex].Position = vector2f2;
			this.hitsparks[this.hitsparkIndex].Visible = true;
			this.hitsparks[this.hitsparkIndex].Frame = 0f;
			this.hitsparks[this.hitsparkIndex].OnAnimationComplete += this.HitsparkAnimationComplete;
			this.hitsparkIndex = (this.hitsparkIndex + 1) % this.hitsparks.Length;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000F507 File Offset: 0x0000D707
		private void HitsparkAnimationComplete(AnimatedRenderable graphic)
		{
			graphic.Visible = false;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000F510 File Offset: 0x0000D710
		private void DamageNumberComplete(DamageNumber sender)
		{
			this.damageNumbersToRemove.Add(sender);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F520 File Offset: 0x0000D720
		private void AddStar()
		{
			if (this.starCount < 16)
			{
				this.starGraphics[this.starCount].Visible = true;
				this.starCount++;
				this.rotAngle -= (float)(6.283185307179586 / (double)this.starCount);
				this.modAngle -= (float)(6.283185307179586 / (double)this.starCount) / 2f;
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F59C File Offset: 0x0000D79C
		public void Update()
		{
			if (this.state == ComboAnimator.State.Circling)
			{
				this.rotAngle += 0.07f;
				this.modAngle += 0.035f;
				for (int i = 0; i < this.starCount; i++)
				{
					float num = (float)(6.283185307179586 / (double)this.starCount * (double)i);
					int num2 = (int)(Math.Cos((double)(this.rotAngle + num)) * (double)this.size.X);
					int num3 = (int)(Math.Sin((double)(this.rotAngle + num)) * (double)this.size.Y + Math.Sin((double)(this.modAngle + num)) * 10.0);
					Vector2f vector2f = this.position + new Vector2f((float)num2, (float)(-(float)num3));
					this.starGraphics[i].Position = vector2f;
					this.starGraphics[i].Depth = this.depth - (int)(Math.Sin((double)(this.rotAngle + num)) * (double)((float)(this.starCount + 1)));
				}
			}
			else if (this.state == ComboAnimator.State.Falling)
			{
				bool flag = true;
				for (int j = 0; j < this.starCount; j++)
				{
					if (this.starGraphics[j].Position.Y > 135f && !this.bounceFlag[j])
					{
						this.starVelocity[j].Y = -(this.starVelocity[j].Y * 0.45f);
						this.bounceFlag[j] = true;
					}
					Vector2f[] array = this.starVelocity;
					int num4 = j;
					array[num4].X = array[num4].X * 0.98f;
					Vector2f[] array2 = this.starVelocity;
					int num5 = j;
					array2[num5].Y = array2[num5].Y + 0.1f;
					this.starGraphics[j].Position += this.starVelocity[j];
					bool flag2 = this.starGraphics[j].Position.X < (float)this.starGraphics[j].TextureRect.Width || this.starGraphics[j].Position.Y < (float)this.starGraphics[j].TextureRect.Height || this.starGraphics[j].Position.X > (float)(320L + (long)this.starGraphics[j].TextureRect.Width) || this.starGraphics[j].Position.Y > (float)(180L + (long)this.starGraphics[j].TextureRect.Height);
					flag = (flag && flag2);
				}
				if (flag && this.totalDamageNumber.Done)
				{
					this.End();
				}
			}
			if (this.enemyGraphic != null && this.wobbleSpeed > 0f)
			{
				this.wobble += this.wobbleSpeed;
				this.wobbleDamp = ((this.wobbleDamp > 0.05f) ? (this.wobbleDamp * 0.9f) : 0f);
				this.enemyGraphic.Rotation = (float)(Math.Sin((double)this.wobble) * (double)this.wobbleDamp);
			}
			if (this.enemyGraphic != null)
			{
				this.bounce += this.bounceSpeed;
				if (this.initialEnemyPosition.Y + this.bounce < this.initialEnemyPosition.Y)
				{
					this.bounceSpeed += 0.5f;
					this.enemyGraphic.Position = this.initialEnemyPosition + new Vector2f(0f, this.bounce);
				}
				else
				{
					this.bounceSpeed = -this.bounceSpeed * 0.6f;
					this.enemyGraphic.Position = this.initialEnemyPosition;
				}
			}
			foreach (DamageNumber damageNumber in this.damageNumbers)
			{
				damageNumber.Update();
			}
			foreach (DamageNumber item in this.damageNumbersToRemove)
			{
				this.damageNumbers.Remove(item);
			}
			this.damageNumbersToRemove.Clear();
			this.totalDamageNumber.Update();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000FA28 File Offset: 0x0000DC28
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000FA38 File Offset: 0x0000DC38
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				for (int i = 0; i < this.starGraphics.Length; i++)
				{
					if (this.starGraphics[i] != null)
					{
						this.starGraphics[i].Dispose();
					}
				}
			}
			this.disposed = true;
		}

		// Token: 0x040003B5 RID: 949
		private const int MAX_STARS = 16;

		// Token: 0x040003B6 RID: 950
		private const int MAX_DAMAGE_NUMBERS = 3;

		// Token: 0x040003B7 RID: 951
		private const float ROT_SPEED = 0.07f;

		// Token: 0x040003B8 RID: 952
		private const float MODULATION_SPEED = 0.035f;

		// Token: 0x040003B9 RID: 953
		private const float GRAVITY = 0.1f;

		// Token: 0x040003BA RID: 954
		private const int GROUND_Y = 135;

		// Token: 0x040003BB RID: 955
		private const float EXPLODE_SUCCESS_FACTOR = 7.5f;

		// Token: 0x040003BC RID: 956
		private const float EXPLODE_FAIL_FACTOR = 100f;

		// Token: 0x040003BD RID: 957
		private static readonly string HITSPARK_RESOURCE = Paths.GRAPHICS + "hitsparks.dat";

		// Token: 0x040003BE RID: 958
		private bool disposed;

		// Token: 0x040003BF RID: 959
		private Vector2f position;

		// Token: 0x040003C0 RID: 960
		private Vector2f size;

		// Token: 0x040003C1 RID: 961
		private int depth;

		// Token: 0x040003C2 RID: 962
		private RenderPipeline pipeline;

		// Token: 0x040003C3 RID: 963
		private IndexedColorGraphic[] starGraphics;

		// Token: 0x040003C4 RID: 964
		private ComboAnimator.State state;

		// Token: 0x040003C5 RID: 965
		private int starCount;

		// Token: 0x040003C6 RID: 966
		private float rotAngle;

		// Token: 0x040003C7 RID: 967
		private float modAngle;

		// Token: 0x040003C8 RID: 968
		private bool[] bounceFlag;

		// Token: 0x040003C9 RID: 969
		private Vector2f[] starVelocity;

		// Token: 0x040003CA RID: 970
		private List<DamageNumber> damageNumbers;

		// Token: 0x040003CB RID: 971
		private List<DamageNumber> damageNumbersToRemove;

		// Token: 0x040003CC RID: 972
		private TotalDamageNumber totalDamageNumber;

		// Token: 0x040003CD RID: 973
		private Graphic[] hitsparks;

		// Token: 0x040003CE RID: 974
		private int hitsparkIndex;

		// Token: 0x040003CF RID: 975
		private Graphic enemyGraphic;

		// Token: 0x040003D0 RID: 976
		private Vector2f initialEnemyPosition;

		// Token: 0x040003D1 RID: 977
		private float wobble;

		// Token: 0x040003D2 RID: 978
		private float wobbleSpeed;

		// Token: 0x040003D3 RID: 979
		private float wobbleDamp;

		// Token: 0x040003D4 RID: 980
		private float bounce;

		// Token: 0x040003D5 RID: 981
		private float bounceSpeed;

		// Token: 0x02000071 RID: 113
		private enum State
		{
			// Token: 0x040003D8 RID: 984
			Circling,
			// Token: 0x040003D9 RID: 985
			Falling,
			// Token: 0x040003DA RID: 986
			Stopped
		}

		// Token: 0x02000072 RID: 114
		// (Invoke) Token: 0x06000272 RID: 626
		public delegate void AnimationCompleteHandler(int starCount);
	}
}
