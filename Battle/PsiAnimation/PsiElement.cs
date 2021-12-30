using System;
using Carbine.Audio;
using Carbine.Graphics;
using Mother4.Battle.UI;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.PsiAnimation
{
	// Token: 0x0200006C RID: 108
	internal struct PsiElement
	{
		// Token: 0x0400037A RID: 890
		public int Timestamp;

		// Token: 0x0400037B RID: 891
		public AnimatedRenderable Animation;

		// Token: 0x0400037C RID: 892
		public Vector2f Offset;

		// Token: 0x0400037D RID: 893
		public bool LockToTargetPosition;

		// Token: 0x0400037E RID: 894
		public int PositionIndex;

		// Token: 0x0400037F RID: 895
		public CarbineSound Sound;

		// Token: 0x04000380 RID: 896
		public int CardPop;

		// Token: 0x04000381 RID: 897
		public float CardPopSpeed;

		// Token: 0x04000382 RID: 898
		public int CardPopHangtime;

		// Token: 0x04000383 RID: 899
		public BattleCard.SpringMode CardSpringMode;

		// Token: 0x04000384 RID: 900
		public Vector2f CardSpringAmplitude;

		// Token: 0x04000385 RID: 901
		public Vector2f CardSpringSpeed;

		// Token: 0x04000386 RID: 902
		public Vector2f CardSpringDecay;

		// Token: 0x04000387 RID: 903
		public Color? TargetFlashColor;

		// Token: 0x04000388 RID: 904
		public ColorBlendMode TargetFlashBlendMode;

		// Token: 0x04000389 RID: 905
		public int TargetFlashCount;

		// Token: 0x0400038A RID: 906
		public int TargetFlashFrames;

		// Token: 0x0400038B RID: 907
		public Color? SenderFlashColor;

		// Token: 0x0400038C RID: 908
		public ColorBlendMode SenderFlashBlendMode;

		// Token: 0x0400038D RID: 909
		public int SenderFlashCount;

		// Token: 0x0400038E RID: 910
		public int SenderFlashFrames;

		// Token: 0x0400038F RID: 911
		public Color? ScreenDarkenColor;

		// Token: 0x04000390 RID: 912
		public int? ScreenDarkenDepth;

		// Token: 0x04000391 RID: 913
		public float? ScreenDarkenSpeed;
	}
}
