using System;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x0200011E RID: 286
	internal static class ActionCodes
	{
		// Token: 0x040008D0 RID: 2256
		public const string NOOP = "NOOP";

		// Token: 0x040008D1 RID: 2257
		public const string PrintLnAction = "PRLN";

		// Token: 0x040008D2 RID: 2258
		public const string TextboxAction = "TXBX";

		// Token: 0x040008D3 RID: 2259
		public const string SetNpcAction = "SNPC";

		// Token: 0x040008D4 RID: 2260
		public const string SetNametagAction = "SNTG";

		// Token: 0x040008D5 RID: 2261
		public const string QuestionAction = "QSTN";

		// Token: 0x040008D6 RID: 2262
		public const string CameraMoveAction = "CMOV";

		// Token: 0x040008D7 RID: 2263
		public const string CameraPlayerAction = "CFLP";

		// Token: 0x040008D8 RID: 2264
		public const string CameraNPCAction = "CFLN";

		// Token: 0x040008D9 RID: 2265
		public const string WaitAction = "WAIT";

		// Token: 0x040008DA RID: 2266
		public const string ChangeSpritePlayerAction = "CSPP";

		// Token: 0x040008DB RID: 2267
		public const string ChangeSpriteNPCAction = "CSPN";

		// Token: 0x040008DC RID: 2268
		public const string ChangeSubspritePlayerAction = "CSSP";

		// Token: 0x040008DD RID: 2269
		public const string ChangeSubspriteNPCAction = "CSSN";

		// Token: 0x040008DE RID: 2270
		public const string EntityAddAction = "EADD";

		// Token: 0x040008DF RID: 2271
		public const string EntityDeleteAction = "EDEL";

		// Token: 0x040008E0 RID: 2272
		public const string EntityDirectionAction = "EDIR";

		// Token: 0x040008E1 RID: 2273
		public const string EntityMoveAction = "EMOV";

		// Token: 0x040008E2 RID: 2274
		public const string ItemAddAction = "IADD";

		// Token: 0x040008E3 RID: 2275
		public const string ItemRemoveAction = "IREM";

		// Token: 0x040008E4 RID: 2276
		public const string MapMarkSetAction = "MPMK";

		// Token: 0x040008E5 RID: 2277
		public const string MapMarkClearAction = "MPCL";

		// Token: 0x040008E6 RID: 2278
		public const string SetFlagAction = "SFLG";

		// Token: 0x040008E7 RID: 2279
		public const string AnimationAction = "ANIM";

		// Token: 0x040008E8 RID: 2280
		public const string ScreenShakeAction = "SSHK";

		// Token: 0x040008E9 RID: 2281
		public const string StatModifyAction = "SMOD";

		// Token: 0x040008EA RID: 2282
		public const string StatSetAction = "SSET";

		// Token: 0x040008EB RID: 2283
		public const string AddMoneyAction = "AMNY";

		// Token: 0x040008EC RID: 2284
		public const string SetMoneyAction = "SMNY";

		// Token: 0x040008ED RID: 2285
		public const string ValueSetAction = "SGVL";

		// Token: 0x040008EE RID: 2286
		public const string ValueAddAction = "AGVL";

		// Token: 0x040008EF RID: 2287
		public const string IfFlagAction = "IFFL";

		// Token: 0x040008F0 RID: 2288
		public const string IfValueAction = "IFVL";

		// Token: 0x040008F1 RID: 2289
		public const string EndIfAction = "IFEN";

		// Token: 0x040008F2 RID: 2290
		public const string ElseAction = "ELSE";

		// Token: 0x040008F3 RID: 2291
		public const string CallAction = "CALL";

		// Token: 0x040008F4 RID: 2292
		public const string WeatherAction = "WTHR";

		// Token: 0x040008F5 RID: 2293
		public const string ScreenEffectAction = "SCEF";

		// Token: 0x040008F6 RID: 2294
		public const string ScreenFadeAction = "SCFD";

		// Token: 0x040008F7 RID: 2295
		public const string TimeAction = "TIME";

		// Token: 0x040008F8 RID: 2296
		public const string AddExpAction = "AEXP";

		// Token: 0x040008F9 RID: 2297
		public const string ScreenFlashAction = "SCFL";

		// Token: 0x040008FA RID: 2298
		public const string EmoticonNPCAction = "EMNP";

		// Token: 0x040008FB RID: 2299
		public const string EmoticonPlayerAction = "EMPL";

		// Token: 0x040008FC RID: 2300
		public const string SetBGMAction = "SBGM";

		// Token: 0x040008FD RID: 2301
		public const string PlaySFXAction = "PSFX";

		// Token: 0x040008FE RID: 2302
		public const string EntityMoveModeAction = "ENMM";

		// Token: 0x040008FF RID: 2303
		public const string HopNPCAction = "HNPC";

		// Token: 0x04000900 RID: 2304
		public const string HopPlayerAction = "HPLR";

		// Token: 0x04000901 RID: 2305
		public const string AddPartyMemberAction = "APRT";

		// Token: 0x04000902 RID: 2306
		public const string RemovePartyMemberAction = "RPRT";

		// Token: 0x04000903 RID: 2307
		public const string StartBattleAction = "SBTL";

		// Token: 0x04000904 RID: 2308
		public const string IrisOverlayAction = "IRIS";

		// Token: 0x04000905 RID: 2309
		public const string GoToMapAction = "GOMP";

		// Token: 0x04000906 RID: 2310
		public const string PlayerPathMoveAction = "PPMV";

		// Token: 0x04000907 RID: 2311
		public const string PlayerPositionAction = "MVPL";

		// Token: 0x04000908 RID: 2312
		public const string FlyoverTextAction = "FOTX";

		// Token: 0x04000909 RID: 2313
		public const string EntityDepthAction = "EDPT";

		// Token: 0x0400090A RID: 2314
		public const string PlayerMoveAction = "PMOV";

		// Token: 0x0400090B RID: 2315
		public const string PlayerShadowAction = "TSPL";

		// Token: 0x0400090C RID: 2316
		public const string SetTilesetPaletteAction = "STSP";

		// Token: 0x0400090D RID: 2317
		public const string SetStatusEffectAction = "STEF";

		// Token: 0x0400090E RID: 2318
		public const string ResetSubspritePlayerAction = "RSSP";

		// Token: 0x0400090F RID: 2319
		public const string ResetSubspriteNPCAction = "RSSN";

		// Token: 0x04000910 RID: 2320
		public const string SetControlAction = "CTRL";

		// Token: 0x04000911 RID: 2321
		public const string SetPlayerAnimationLoopCountAction = "PALC";

		// Token: 0x04000912 RID: 2322
		public const string SetNPCAnimationLoopCountAction = "NALC";

		// Token: 0x04000913 RID: 2323
		public const string ToggleLetterboxingAction = "LTBX";

		// Token: 0x04000914 RID: 2324
		public const string SetPlayerDirectionAction = "PDIR";

		// Token: 0x04000915 RID: 2325
		public const string ToggleTextboxAction = "STBX";
	}
}
