// Decompiled with JetBrains decompiler
// Type: Mother4.Battle.BattleInterfaceController
// Assembly: Mother4, Version=0.7.6122.42121, Culture=neutral, PublicKeyToken=null
// MVID: FECD8919-57FF-4485-92CA-DA4098284AB3
// Assembly location: D:\OddityPrototypes\Mother 4 -- 2018\Mother4.exe

using Carbine.Actors;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Battle.Combatants;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Battle.UI.Modifiers;
using Mother4.Data;
using Mother4.Data.Psi;
using Mother4.GUI;
using Mother4.GUI.Modifiers;
using Mother4.GUI.Text;
using Mother4.GUI.Text.PrintActions;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Mother4.Battle
{
    internal class BattleInterfaceController : IDisposable
    {
        private const int TOP_LETTERBOX_HEIGHT = 14;
        private const int BOTTOM_LETTERBOX_HEIGHT = 35;
        private const float LETTERBOX_SPEED_FACTOR = 10f;
        private const int ENEMY_SPACING = 10;
        private const int ENEMY_DEPTH = 0;
        private const int ENEMY_TRANSLATE_FRAMES = 10;
        private const int ENEMY_DEATH_FRAMES = 40;
        public const int ENEMY_MIDLINE = 78;
        public const int ENEMY_OFFSET = 12;
        private bool disposed;
        private RenderPipeline pipeline;
        private ActorManager actorManager;
        private CombatantController combatantController;
        private Shape topLetterbox;
        private Shape bottomLetterbox;
        private float topLetterboxY;
        private float bottomLetterboxY;
        private float topLetterboxTargetY;
        private float bottomLetterboxTargetY;
        private ButtonBar buttonBar;
        private BattlePsiBox psiMenu;
        private CardBar cardBar;
        private Dictionary<int, IndexedColorGraphic> enemyGraphics;
        private BattleTextBox textbox;
        private ScreenDimmer dimmer;
        private ComboAnimator comboCircle;
        private int selectedTargetId;
        private int enemySelectIndex;
        private int partySelectIndex;
        private List<int> enemyIDs;
        private List<int> partyIDs;
        private List<IGraphicModifier> graphicModifiers;
        private List<PsiAnimator> psiAnimators;
        private BattleInterfaceController.State state;
        private SelectionState selectionState;
        private Groovy groovy;
        private CarbineSound moveBeepX;
        private CarbineSound moveBeepY;
        private CarbineSound selectBeep;
        private CarbineSound cancelBeep;
        private CarbineSound prePlayerAttack;
        private CarbineSound preEnemyAttack;
        private CarbineSound prePsiSound;
        private CarbineSound talkSound;
        private CarbineSound enemyDeathSound;
        private CarbineSound smashSound;
        private Dictionary<CharacterType, List<CarbineSound>> comboSoundMap;
        private CarbineSound comboHitA;
        private CarbineSound comboHitB;
        private CarbineSound hitSound;
        private CarbineSound comboSuccess;
        private CarbineSound groovySound;
        private CarbineSound reflectSound;
        private Dictionary<int, CarbineSound> winSounds;
        private YouWon youWon;
        private LevelUpJingler jingler;
        private List<DamageNumber> damageNumbers;
        private Dictionary<Graphic, Graphic> selectionMarkers;
        private bool textboxHideFlag;
        private bool isUndoAllowed;
        private int activeCharacter;

        public bool AllowUndo
        {
            get => this.isUndoAllowed;
            set => this.isUndoAllowed = value;
        }

        public int ActiveCharacter
        {
            get => this.activeCharacter;
            set => this.activeCharacter = value;
        }

        public bool RunAttempted { get; set; }

        public CarbineSound PrePlayerAttack => this.prePlayerAttack;

        public CarbineSound PreEnemyAttack => this.preEnemyAttack;

        public CarbineSound PrePsiSound => this.prePsiSound;

        public CarbineSound TalkSound => this.talkSound;

        public CarbineSound EnemyDeathSound => this.enemyDeathSound;

        public CarbineSound GroovySound => this.groovySound;

        public CarbineSound ReflectSound => this.reflectSound;

        public event BattleInterfaceController.InteractionCompletionHandler OnInteractionComplete;

        public event BattleInterfaceController.TextboxCompletionHandler OnTextboxComplete;

        public event BattleInterfaceController.TextTriggerHandler OnTextTrigger;

        public BattleInterfaceController(
          RenderPipeline pipeline,
          ActorManager actorManager,
          CombatantController combatantController,
          bool letterboxing)
        {
            this.pipeline = pipeline;
            this.actorManager = actorManager;
            this.combatantController = combatantController;
            this.topLetterbox = (Shape)new RectangleShape(new Vector2f(320f, 14f));
            this.topLetterbox.FillColor = Color.Black;
            this.topLetterbox.Position = new Vector2f(0.0f, -14f);
            this.topLetterboxY = this.topLetterbox.Position.Y;
            this.topLetterboxTargetY = letterboxing ? 0.0f : -14f;
            this.bottomLetterbox = (Shape)new RectangleShape(new Vector2f(320f, 35f));
            this.bottomLetterbox.FillColor = Color.Black;
            this.bottomLetterbox.Position = new Vector2f(0.0f, 180f);
            this.bottomLetterboxY = this.bottomLetterbox.Position.Y;
            this.bottomLetterboxTargetY = (float)(180L + (letterboxing ? -35L : 0L));
            this.buttonBar = new ButtonBar(pipeline);
            actorManager.Add((Actor)this.buttonBar);
            Combatant[] factionCombatants = combatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
            CharacterType[] characterTypeArray = new CharacterType[factionCombatants.Length];
            for (int index = 0; index < factionCombatants.Length; ++index)
                characterTypeArray[index] = ((PlayerCombatant)factionCombatants[index]).Character;
            this.cardBar = new CardBar(pipeline, characterTypeArray);
            actorManager.Add((Actor)this.cardBar);
            this.psiMenu = new BattlePsiBox(characterTypeArray);
            this.pipeline.Add((Renderable)this.psiMenu);
            this.selectionMarkers = new Dictionary<Graphic, Graphic>();
            for (int index = 0; index < characterTypeArray.Length; ++index)
            {
                Graphic cardGraphic = this.cardBar.GetCardGraphic(index);
                Graphic graphic = (Graphic)new IndexedColorGraphic(Paths.GRAPHICS + "cursor.dat", "down", VectorMath.Truncate(cardGraphic.Position - cardGraphic.Origin + new Vector2f(cardGraphic.Size.X / 2f, 4f)), cardGraphic.Depth + 10);
                graphic.Visible = false;
                this.pipeline.Add((Renderable)graphic);
                this.selectionMarkers.Add(cardGraphic, graphic);
            }
            this.enemyGraphics = new Dictionary<int, IndexedColorGraphic>();
            this.enemyIDs = new List<int>();
            this.partyIDs = new List<int>();
            foreach (Combatant combatant in combatantController.CombatantList)
            {
                switch (combatant.Faction)
                {
                    case BattleFaction.PlayerTeam:
                        PlayerCombatant playerCombatant = (PlayerCombatant)combatant;
                        playerCombatant.OnStatChange += new Combatant.StatChangeHandler(this.OnPlayerStatChange);
                        playerCombatant.OnStatusEffectChange += new Combatant.StatusEffectChangeHandler(this.OnPlayerStatusEffectChange);
                        this.partyIDs.Add(playerCombatant.ID);
                        continue;
                    case BattleFaction.EnemyTeam:
                        EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
                        enemyCombatant.OnStatusEffectChange += new Combatant.StatusEffectChangeHandler(this.OnEnemyStatusEffectChange);
                        IndexedColorGraphic key = new IndexedColorGraphic(EnemyGraphics.GetFilename(enemyCombatant.Enemy), "front", new Vector2f(), 0)
                        {
                            CurrentPalette = uint.MaxValue
                        };
                        key.CurrentPalette = 0U;
                        this.enemyGraphics.Add(enemyCombatant.ID, key);
                        pipeline.Add((Renderable)key);
                        this.enemyIDs.Add(enemyCombatant.ID);
                        Graphic graphic = (Graphic)new IndexedColorGraphic(Paths.GRAPHICS + "cursor.dat", "down", VectorMath.Truncate(key.Position - key.Origin + new Vector2f(key.Size.X / 2f, 4f)), key.Depth + 10);
                        graphic.Visible = false;
                        this.pipeline.Add((Renderable)graphic);
                        this.selectionMarkers.Add((Graphic)key, graphic);
                        continue;
                    default:
                        continue;
                }
            }
            this.AlignEnemyGraphics();
            this.textbox = new BattleTextBox();
            this.textbox.OnTextboxComplete += new TextBox.CompletionHandler(this.TextboxComplete);
            this.textbox.OnTextTrigger += new TextBox.TextTriggerHandler(this.TextTrigger);
            pipeline.Add((Renderable)this.textbox);
            this.dimmer = new ScreenDimmer(pipeline, Color.Transparent, 0, 15);
            this.state = BattleInterfaceController.State.Waiting;
            this.selectionState = new SelectionState();
            this.selectedTargetId = -1;
            this.comboCircle = new ComboAnimator(pipeline, 0);
            this.moveBeepX = AudioManager.Instance.Use(Paths.AUDIO + "cursorx.wav", AudioType.Sound);
            this.moveBeepY = AudioManager.Instance.Use(Paths.AUDIO + "cursory.wav", AudioType.Sound);
            this.selectBeep = AudioManager.Instance.Use(Paths.AUDIO + "confirm.wav", AudioType.Sound);
            this.cancelBeep = AudioManager.Instance.Use(Paths.AUDIO + "cancel.wav", AudioType.Sound);
            this.prePlayerAttack = AudioManager.Instance.Use(Paths.AUDIO + "prePlayerAttack.wav", AudioType.Sound);
            this.preEnemyAttack = AudioManager.Instance.Use(Paths.AUDIO + "preEnemyAttack.wav", AudioType.Sound);
            this.prePsiSound = AudioManager.Instance.Use(Paths.AUDIO + "prePsi.wav", AudioType.Sound);
            this.talkSound = AudioManager.Instance.Use(Paths.AUDIO + "floydTalk.wav", AudioType.Sound);
            this.enemyDeathSound = AudioManager.Instance.Use(Paths.AUDIO + "enemyDeath.wav", AudioType.Sound);
            this.smashSound = AudioManager.Instance.Use(Paths.AUDIO + "smaaash.wav", AudioType.Sound);
            this.comboHitA = AudioManager.Instance.Use(Paths.AUDIO + "hitA.wav", AudioType.Sound);
            this.comboHitB = AudioManager.Instance.Use(Paths.AUDIO + "hitB.wav", AudioType.Sound);
            this.comboSuccess = AudioManager.Instance.Use(Paths.AUDIO + "Combo16.wav", AudioType.Sound);
            this.comboSoundMap = new Dictionary<CharacterType, List<CarbineSound>>();
            for (int index1 = 0; index1 < characterTypeArray.Length; ++index1)
            {
                List<CarbineSound> carbineSoundList;
                if (this.comboSoundMap.ContainsKey(characterTypeArray[index1]))
                {
                    carbineSoundList = this.comboSoundMap[characterTypeArray[index1]];
                }
                else
                {
                    carbineSoundList = new List<CarbineSound>();
                    this.comboSoundMap.Add(characterTypeArray[index1], carbineSoundList);
                }
                for (int index2 = 0; index2 < 3; ++index2)
                {
                    string str = CharacterComboSounds.Get(characterTypeArray[index1], 0, index2, 120);
                    CarbineSound carbineSound = AudioManager.Instance.Use(Paths.AUDIO + str, AudioType.Sound);
                    carbineSoundList.Add(carbineSound);
                }
            }
            this.winSounds = new Dictionary<int, CarbineSound>();
            this.winSounds.Add(0, AudioManager.Instance.Use(Paths.AUDIO + "win1.wav", AudioType.Stream));
            this.winSounds.Add(1, AudioManager.Instance.Use(Paths.AUDIO + "win2.wav", AudioType.Stream));
            this.winSounds.Add(2, AudioManager.Instance.Use(Paths.AUDIO + "win3.wav", AudioType.Stream));
            this.winSounds.Add(3, AudioManager.Instance.Use(Paths.AUDIO + "win4.wav", AudioType.Stream));
            this.groovySound = AudioManager.Instance.Use(Paths.AUDIO + "Groovy.wav", AudioType.Sound);
            this.reflectSound = AudioManager.Instance.Use(Paths.AUDIO + "homerun.wav", AudioType.Sound);
            this.jingler = new LevelUpJingler(characterTypeArray, true);
            this.graphicModifiers = new List<IGraphicModifier>();
            this.damageNumbers = new List<DamageNumber>();
            this.psiAnimators = new List<PsiAnimator>();
            InputManager.Instance.AxisPressed += new InputManager.AxisPressedHandler(this.AxisPressed);
            InputManager.Instance.ButtonPressed += new InputManager.ButtonPressedHandler(this.ButtonPressed);
        }

        ~BattleInterfaceController() => this.Dispose(false);

        private void TextTrigger(int type, string[] args)
        {
            switch (type)
            {
                case 0:
                    this.youWon = new YouWon(this.pipeline);
                    break;
                case 1:
                    CharacterType result1;
                    if (!Enum.TryParse<CharacterType>(args[0], true, out result1))
                        break;
                    this.jingler.Play(result1);
                    break;
                case 2:
                    int result2 = 0;
                    int result3 = 0;
                    int.TryParse(args[0], out result2);
                    int.TryParse(args[1], out result3);
                    StatSet statChange1 = new StatSet()
                    {
                        HP = result3
                    };
                    this.combatantController[result2].AlterStats(statChange1);
                    break;
                case 3:
                    int result4 = 0;
                    int result5 = 0;
                    int.TryParse(args[0], out result4);
                    int.TryParse(args[1], out result5);
                    StatSet statChange2 = new StatSet()
                    {
                        PP = result5
                    };
                    this.combatantController[result4].AlterStats(statChange2);
                    break;
                default:
                    if (this.OnTextTrigger == null)
                        break;
                    this.OnTextTrigger(type, args);
                    break;
            }
        }

        public void PlayWinBGM(int type)
        {
            if (!this.winSounds.ContainsKey(type))
                return;
            this.winSounds[type].Play();
        }

        public void StopWinBGM()
        {
            foreach (CarbineSound carbineSound in this.winSounds.Values)
                carbineSound.Stop();
        }

        public void PlayLevelUpBGM() => this.jingler.Play();

        public void EndLevelUpBGM() => this.jingler.End();

        public void StopLevelUpBGM() => this.jingler.Stop();

        private CarbineSound GetComboSound(CharacterType character, int index)
        {
            CarbineSound comboSound = (CarbineSound)null;
            if (this.comboSoundMap.ContainsKey(character))
                comboSound = this.comboSoundMap[character][index % this.comboSoundMap[character].Count];
            return comboSound;
        }

        private void OnPlayerStatChange(Combatant sender, StatSet change)
        {
            PlayerCombatant playerCombatant = (PlayerCombatant)sender;
            this.UpdatePlayerCard(playerCombatant.ID, playerCombatant.Stats.HP, playerCombatant.Stats.PP, playerCombatant.Stats.Meter);
        }

        private void OnPlayerStatusEffectChange(
          Combatant sender,
          StatusEffect statusEffect,
          bool added)
        {
            if (added)
            {
                switch (statusEffect)
                {
                    case StatusEffect.Talking:
                        this.TalkifyPlayer(sender as PlayerCombatant);
                        this.SetCardSpring(sender.ID, BattleCard.SpringMode.BounceUp, new Vector2f(0.0f, 8f), new Vector2f(0.0f, 0.1f), new Vector2f(0.0f, 1f));
                        break;
                    case StatusEffect.Shield:
                        this.SetCardGlow(sender.ID, BattleCard.GlowType.Shield);
                        break;
                    case StatusEffect.PsiShield:
                        this.SetCardGlow(sender.ID, BattleCard.GlowType.PsiSheild);
                        break;
                    case StatusEffect.Counter:
                        this.SetCardGlow(sender.ID, BattleCard.GlowType.Counter);
                        break;
                    case StatusEffect.PsiCounter:
                        this.SetCardGlow(sender.ID, BattleCard.GlowType.PsiCounter);
                        break;
                    case StatusEffect.Eraser:
                        this.SetCardGlow(sender.ID, BattleCard.GlowType.Eraser);
                        break;
                }
            }
            else
            {
                switch (statusEffect)
                {
                    case StatusEffect.Talking:
                        this.RemoveTalker(this.cardBar.GetCardGraphic(sender.ID));
                        this.SetCardSpring(sender.ID, BattleCard.SpringMode.Normal, new Vector2f(0.0f, 0.0f), new Vector2f(0.0f, 0.0f), new Vector2f(0.0f, 0.0f));
                        break;
                    case StatusEffect.Shield:
                    case StatusEffect.PsiShield:
                    case StatusEffect.Counter:
                    case StatusEffect.PsiCounter:
                    case StatusEffect.Eraser:
                        this.SetCardGlow(sender.ID, BattleCard.GlowType.None);
                        break;
                }
            }
        }

        private void OnEnemyStatusEffectChange(Combatant sender, StatusEffect statusEffect, bool added)
        {
            if (added)
            {
                if (statusEffect != StatusEffect.Talking)
                    return;
                this.TalkifyEnemy(sender as EnemyCombatant);
            }
            else
            {
                if (statusEffect != StatusEffect.Talking)
                    return;
                this.RemoveTalker((Graphic)this.enemyGraphics[sender.ID]);
            }
        }

        public PsiAnimator AddPsiAnimation(
          PsiElementList animation,
          Combatant sender,
          Combatant[] targets)
        {
            Graphic senderGraphic = (Graphic)null;
            if (sender.Faction == BattleFaction.EnemyTeam)
                senderGraphic = (Graphic)this.enemyGraphics[sender.ID];
            else if (sender.Faction == BattleFaction.PlayerTeam)
                senderGraphic = this.cardBar.GetCardGraphic(sender.ID);
            int[] targetCardIds = new int[targets.Length];
            Graphic[] targetGraphics = new Graphic[targets.Length];
            for (int index = 0; index < targets.Length; ++index)
            {
                if (targets[index].Faction == BattleFaction.EnemyTeam)
                {
                    targetGraphics[index] = (Graphic)this.enemyGraphics[targets[index].ID];
                    targetCardIds[index] = -1;
                }
                else if (targets[index].Faction == BattleFaction.PlayerTeam)
                {
                    targetGraphics[index] = this.cardBar.GetCardGraphic(targets[index].ID);
                    targetCardIds[index] = targets[index].ID;
                }
            }
            PsiAnimator psiAnimator = new PsiAnimator(this.pipeline, this.graphicModifiers, animation, senderGraphic, targetGraphics, this.cardBar, targetCardIds);
            this.psiAnimators.Add(psiAnimator);
            return psiAnimator;
        }

        public DamageNumber AddDamageNumber(Combatant combatant, int number)
        {
            Vector2f offset = new Vector2f();
            Vector2f position;
            if (combatant.Faction == BattleFaction.PlayerTeam)
            {
                Graphic cardGraphic = this.cardBar.GetCardGraphic(combatant.ID);
                position = new Vector2f((float)(int)cardGraphic.Position.X, (float)(int)cardGraphic.Position.Y) + new Vector2f((float)(int)((double)cardGraphic.Size.X / 2.0), 2f);
                offset.Y = -10f;
            }
            else if (combatant.Faction == BattleFaction.EnemyTeam)
            {
                Graphic enemyGraphic = (Graphic)this.enemyGraphics[combatant.ID];
                position = new Vector2f((float)(int)enemyGraphic.Position.X, (float)(int)enemyGraphic.Position.Y);
                offset.Y = (float)(int)(-(double)enemyGraphic.Size.Y / 3.0);
            }
            else
                position = new Vector2f(-320f, -180f);
            DamageNumber damageNumber = new DamageNumber(this.pipeline, position, offset, 30, number);
            damageNumber.SetVisibility(true);
            this.damageNumbers.Add(damageNumber);
            damageNumber.Start();
            return damageNumber;
        }

        public void StartComboCircle(EnemyCombatant enemy) => this.comboCircle.Setup((Graphic)this.enemyGraphics[enemy.ID]);

        public void StopComboCircle(bool explode)
        {
            this.comboCircle.Stop(explode);
            if (!explode)
                return;
            this.comboSuccess.Stop();
            this.comboSuccess.Play();
        }

        public void AddComboHit(
          int damage,
          int comboCount,
          CharacterType character,
          Combatant target,
          bool smash)
        {
            this.comboCircle.AddHit(damage, smash);
            if ((comboCount + 1) % 4 != 0)
                this.comboHitA.Play();
            else
                this.comboHitB.Play();
            if (this.hitSound != null)
                this.hitSound.Stop();
            this.hitSound = this.GetComboSound(character, comboCount);
            if (this.hitSound != null)
                this.hitSound.Play();
            if (!smash)
                return;
            this.smashSound.Stop();
            this.smashSound.Play();
            BattleSmash battleSmash = new BattleSmash(this.pipeline, this.enemyGraphics[target.ID].Position);
        }

        public bool IsComboCircleDone() => this.comboCircle.Stopped;

        public void FlashEnemy(EnemyCombatant combatant, Color color, int duration, int count) => this.FlashEnemy(combatant, color, ColorBlendMode.Multiply, duration, count);

        public void FlashEnemy(
          EnemyCombatant combatant,
          Color color,
          ColorBlendMode blendMode,
          int duration,
          int count)
        {
            this.graphicModifiers.Add((IGraphicModifier)new GraphicFader(this.enemyGraphics[combatant.ID], color, blendMode, duration, count));
        }

        public void BlinkEnemy(EnemyCombatant combatant, int duration, int count) => this.graphicModifiers.Add((IGraphicModifier)new GraphicBlinker((Graphic)this.enemyGraphics[combatant.ID], duration, count));

        public void TalkifyPlayer(PlayerCombatant combatant) => this.graphicModifiers.Add((IGraphicModifier)new GraphicTalker(this.pipeline, this.cardBar.GetCardGraphic(combatant.ID)));

        public void TalkifyEnemy(EnemyCombatant combatant)
        {
            this.graphicModifiers.Add((IGraphicModifier)new GraphicTalker(this.pipeline, (Graphic)this.enemyGraphics[combatant.ID]));
            this.graphicModifiers.Add((IGraphicModifier)new GraphicBouncer((Graphic)this.enemyGraphics[combatant.ID], GraphicBouncer.SpringMode.BounceUp, new Vector2f(0.0f, 4f), new Vector2f(0.0f, 0.1f), new Vector2f(0.0f, 1f)));
        }

        private void RemoveTalker(Graphic graphic)
        {
            foreach (IGraphicModifier graphicModifier in this.graphicModifiers)
            {
                if (graphicModifier is GraphicTalker && graphicModifier.Graphic == graphic)
                    (graphicModifier as GraphicTalker).Dispose();
            }
            this.graphicModifiers.RemoveAll((Predicate<IGraphicModifier>)(x => x is GraphicTalker && x.Graphic == graphic));
            this.graphicModifiers.RemoveAll((Predicate<IGraphicModifier>)(x => x is GraphicBouncer && x.Graphic == graphic));
        }

        public void RemoveTalkers()
        {
            foreach (IGraphicModifier graphicModifier in this.graphicModifiers)
            {
                if (graphicModifier is GraphicTalker)
                    (graphicModifier as GraphicTalker).Dispose();
            }
            this.graphicModifiers.RemoveAll((Predicate<IGraphicModifier>)(x => x is GraphicTalker));
        }

        public void AddShieldAnimation(Combatant combatant)
        {
            Graphic graphic = (Graphic)null;
            if (combatant is PlayerCombatant)
                graphic = this.cardBar.GetCardGraphic(combatant.ID);
            else if (combatant is EnemyCombatant)
                graphic = (Graphic)this.enemyGraphics[combatant.ID];
            if (graphic == null)
                return;
            this.graphicModifiers.Add((IGraphicModifier)new GraphicShielder(this.pipeline, graphic));
        }

        private void SetSelectionMarkerVisibility(Graphic graphic, bool visible)
        {
            Graphic selectionMarker = this.selectionMarkers[graphic];
            if (visible)
            {
                selectionMarker.Position = VectorMath.Truncate(graphic.Position - graphic.Origin + new Vector2f(graphic.Size.X / 2f, 4f));
                selectionMarker.Depth = (int)short.MaxValue;
                this.pipeline.Update((Renderable)selectionMarker);
            }
            selectionMarker.Visible = visible;
        }

        private void ResetTargetingSelection()
        {
            foreach (KeyValuePair<int, IndexedColorGraphic> enemyGraphic in this.enemyGraphics)
            {
                KeyValuePair<int, IndexedColorGraphic> kvp = enemyGraphic;
                this.graphicModifiers.RemoveAll((Predicate<IGraphicModifier>)(x => x.Graphic == kvp.Value && x is GraphicFader));
                if (this.selectionState.TargetingMode == TargetingMode.Enemy)
                {
                    if (kvp.Key == this.selectedTargetId)
                    {
                        kvp.Value.Color = Color.White;
                        this.graphicModifiers.Add((IGraphicModifier)new GraphicFader(kvp.Value, new Color((byte)64, (byte)64, (byte)64), ColorBlendMode.Screen, 30, -1));
                        this.SetSelectionMarkerVisibility((Graphic)kvp.Value, true);
                    }
                    else
                    {
                        kvp.Value.ColorBlendMode = ColorBlendMode.Multiply;
                        kvp.Value.Color = new Color((byte)128, (byte)128, (byte)128);
                        this.SetSelectionMarkerVisibility((Graphic)kvp.Value, false);
                    }
                }
                else if (this.selectionState.TargetingMode == TargetingMode.AllEnemies)
                {
                    kvp.Value.Color = Color.White;
                    this.graphicModifiers.Add((IGraphicModifier)new GraphicFader(kvp.Value, new Color((byte)64, (byte)64, (byte)64), ColorBlendMode.Screen, 30, -1));
                    this.SetSelectionMarkerVisibility((Graphic)kvp.Value, true);
                }
                else if (this.selectionState.TargetingMode == TargetingMode.PartyMember || this.selectionState.TargetingMode == TargetingMode.AllPartyMembers)
                {
                    kvp.Value.ColorBlendMode = ColorBlendMode.Multiply;
                    kvp.Value.Color = new Color((byte)128, (byte)128, (byte)128);
                    this.SetSelectionMarkerVisibility((Graphic)kvp.Value, false);
                }
                else
                {
                    kvp.Value.ColorBlendMode = ColorBlendMode.Multiply;
                    kvp.Value.Color = Color.White;
                    this.SetSelectionMarkerVisibility((Graphic)kvp.Value, false);
                }
            }
            for (int index = 0; index < this.partyIDs.Count; ++index)
            {
                Graphic cardGraphic = this.cardBar.GetCardGraphic(index);
                if (this.selectionState.TargetingMode == TargetingMode.PartyMember)
                    this.SetSelectionMarkerVisibility(cardGraphic, this.partySelectIndex == index);
                else if (this.selectionState.TargetingMode == TargetingMode.AllPartyMembers)
                    this.SetSelectionMarkerVisibility(cardGraphic, true);
                else
                    this.SetSelectionMarkerVisibility(cardGraphic, false);
            }
        }

        private void AlignEnemyGraphics()
        {
            int x1 = 0;
            int num = 320 / (this.enemyGraphics.Count + 1);
            for (int index1 = 0; index1 < this.enemyIDs.Count; ++index1)
            {
                int id = this.enemyIDs[index1];
                x1 += num;
                Vector2f target = new Vector2f((float)x1, (float)(78 + (index1 % 2 == 0 ? 0 : 12)));
                this.enemyGraphics[id].Depth = (int)target.Y - 78;
                if (this.graphicModifiers != null)
                {
                    Console.WriteLine("old:({0},{1}) new:({2},{3})", new object[4]
                    {
            (object) this.enemyGraphics[id].Position.X,
            (object) this.enemyGraphics[id].Position.Y,
            (object) target.X,
            (object) target.Y
                    });
                    for (int index2 = 0; index2 < this.graphicModifiers.Count; ++index2)
                    {
                        if (this.graphicModifiers[index2].Graphic == this.enemyGraphics[id])
                        {
                            this.graphicModifiers.Remove(this.graphicModifiers[index2]);
                            Console.WriteLine("removed");
                        }
                    }
                    this.graphicModifiers.RemoveAll((Predicate<IGraphicModifier>)(x => x.Graphic == this.enemyGraphics[id] && x is GraphicTranslator));
                    this.graphicModifiers.Add((IGraphicModifier)new GraphicTranslator((Graphic)this.enemyGraphics[id], target, 10));
                }
                else
                    this.enemyGraphics[id].Position = target;
            }
        }

        public void DoGroovy(int id)
        {
            if (this.groovy != null)
                this.groovy.Dispose();
            this.groovy = new Groovy(this.pipeline, this.cardBar.GetCardTopMiddle(id));
            this.groovySound.Play();
        }

        private void TextboxComplete()
        {
            if (this.youWon != null)
            {
                this.youWon.Remove();
                this.youWon.Dispose();
                this.youWon = (YouWon)null;
            }
            if (this.OnTextboxComplete == null)
                return;
            this.OnTextboxComplete();
        }

        private void AxisPressed(InputManager sender, Vector2f axis)
        {
            bool flag1 = (double)axis.X < 0.0;
            bool flag2 = (double)axis.X > 0.0;
            bool flag3 = (double)axis.Y < 0.0;
            bool flag4 = (double)axis.Y > 0.0;
            if (this.state != BattleInterfaceController.State.Waiting)
            {
                if (flag1 || flag2)
                    this.moveBeepX.Play();
                if (flag3 || flag4)
                    this.moveBeepY.Play();
            }
            switch (this.state)
            {
                case BattleInterfaceController.State.TopLevelSelection:
                    if (flag1)
                    {
                        this.buttonBar.SelectLeft();
                        break;
                    }
                    if (!flag2)
                        break;
                    this.buttonBar.SelectRight();
                    break;
                case BattleInterfaceController.State.PsiTypeSelection:
                    if (flag3)
                    {
                        this.psiMenu.SelectUp();
                        break;
                    }
                    if (flag4)
                    {
                        this.psiMenu.SelectDown();
                        break;
                    }
                    if (flag1)
                    {
                        this.psiMenu.SelectLeft();
                        break;
                    }
                    if (!flag2)
                        break;
                    this.psiMenu.SelectRight();
                    break;
                case BattleInterfaceController.State.EnemySelection:
                    if (flag1)
                    {
                        --this.enemySelectIndex;
                        if (this.enemySelectIndex < 0)
                            this.enemySelectIndex = this.enemyIDs.Count - 1;
                        this.selectedTargetId = this.enemyIDs[this.enemySelectIndex];
                        this.ResetTargetingSelection();
                        break;
                    }
                    if (!flag2)
                        break;
                    ++this.enemySelectIndex;
                    if (this.enemySelectIndex >= this.enemyIDs.Count)
                        this.enemySelectIndex = 0;
                    this.selectedTargetId = this.enemyIDs[this.enemySelectIndex];
                    this.ResetTargetingSelection();
                    break;
                case BattleInterfaceController.State.AllySelection:
                    if (flag1)
                    {
                        --this.partySelectIndex;
                        if (this.partySelectIndex < 0)
                            this.partySelectIndex = this.partyIDs.Count - 1;
                        this.selectedTargetId = this.partyIDs[this.partySelectIndex];
                        this.ResetTargetingSelection();
                        break;
                    }
                    if (!flag2)
                        break;
                    ++this.partySelectIndex;
                    if (this.partySelectIndex >= this.partyIDs.Count)
                        this.partySelectIndex = 0;
                    this.selectedTargetId = this.partyIDs[this.partySelectIndex];
                    this.ResetTargetingSelection();
                    break;
            }
        }

        private void ButtonPressed(InputManager sender, Button b)
        {
            if (this.state != BattleInterfaceController.State.Waiting)
            {
                if (b == Button.A)
                    this.selectBeep.Play();
                if (b == Button.B)
                    this.cancelBeep.Play();
            }
            switch (this.state)
            {
                case BattleInterfaceController.State.TopLevelSelection:
                    this.TopLevelSelection(b);
                    break;
                case BattleInterfaceController.State.PsiTypeSelection:
                    this.PsiTypeSelection(b);
                    break;
                case BattleInterfaceController.State.SpecialSelection:
                    this.SpecialSelection(b);
                    break;
                case BattleInterfaceController.State.ItemSelection:
                    this.ItemSelection(b);
                    break;
                case BattleInterfaceController.State.EnemySelection:
                case BattleInterfaceController.State.AllySelection:
                    this.TargetSelection(b);
                    break;
            }
        }

        private PlayerCombatant CurrentPlayerCombatant() => (PlayerCombatant)this.combatantController.GetFactionCombatants(BattleFaction.PlayerTeam)[this.cardBar.SelectedIndex];

        private void StartTargetSelection()
        {
            if (this.selectionState.TargetingMode == TargetingMode.None)
            {
                this.CompleteTargetSelection(this.buttonBar.SelectedAction);
            }
            else
            {
                if (this.selectionState.TargetingMode == TargetingMode.Enemy)
                {
                    this.state = BattleInterfaceController.State.EnemySelection;
                    this.selectedTargetId = this.enemyIDs[this.enemySelectIndex % this.enemyIDs.Count];
                }
                else if (this.selectionState.TargetingMode == TargetingMode.AllEnemies)
                {
                    this.state = BattleInterfaceController.State.EnemySelection;
                    this.selectedTargetId = -1;
                }
                else if (this.selectionState.TargetingMode == TargetingMode.PartyMember)
                {
                    this.state = BattleInterfaceController.State.AllySelection;
                    this.selectedTargetId = this.partyIDs[this.partySelectIndex % this.partyIDs.Count];
                }
                else if (this.selectionState.TargetingMode == TargetingMode.AllPartyMembers)
                {
                    this.state = BattleInterfaceController.State.AllySelection;
                    this.selectedTargetId = -1;
                }
                this.buttonBar.Hide();
                this.ResetTargetingSelection();
            }
        }

        private void TopLevelSelection(Button b)
        {
            switch (b)
            {
                case Button.A:
                    switch (this.buttonBar.SelectedAction)
                    {
                        case ButtonBar.Action.Bash:
                            this.selectionState.TargetingMode = TargetingMode.Enemy;
                            this.StartTargetSelection();
                            return;
                        case ButtonBar.Action.Psi:
                            PlayerCombatant playerCombatant = this.CurrentPlayerCombatant();
                            this.state = BattleInterfaceController.State.PsiTypeSelection;
                            this.buttonBar.Hide();
                            this.psiMenu.Show(playerCombatant.Character);
                            return;
                        case ButtonBar.Action.Items:
                            this.state = BattleInterfaceController.State.ItemSelection;
                            this.buttonBar.Hide();
                            return;
                        case ButtonBar.Action.Talk:
                            this.selectionState.TargetingMode = TargetingMode.Enemy;
                            this.state = BattleInterfaceController.State.EnemySelection;
                            this.buttonBar.Hide();
                            this.selectedTargetId = this.enemyIDs[this.enemySelectIndex % this.enemyIDs.Count];
                            this.ResetTargetingSelection();
                            return;
                        case ButtonBar.Action.Guard:
                            this.CompleteMenuGuard();
                            return;
                        case ButtonBar.Action.Run:
                            this.buttonBar.Hide();
                            this.RunAttempted = true;
                            this.CompleteMenuRun();
                            this.state = BattleInterfaceController.State.Waiting;
                            return;
                        default:
                            throw new NotImplementedException("Tried to use unimplemented button action.");
                    }
                case Button.B:
                    if (!this.isUndoAllowed)
                        break;
                    this.CompleteMenuUndo();
                    break;
            }
        }

        private void PsiTypeSelection(Button b)
        {
            switch (b)
            {
                case Button.A:
                    if (this.psiMenu.HasSelection)
                    {
                        PsiLevel psiLevel = this.psiMenu.SelectedPsi.Value;
                        PsiData data = PsiFile.Instance.GetData(psiLevel.PsiType);
                        if ((int)data.PP[psiLevel.Level] <= this.CurrentPlayerCombatant().Stats.PP)
                        {
                            this.psiMenu.Hide();
                            this.selectionState.Psi = psiLevel;
                            this.selectionState.TargetingMode = (TargetingMode)data.Targets[psiLevel.Level];
                            this.StartTargetSelection();
                            break;
                        }
                        this.ShowTextBox("Not enough PP!", false);
                        break;
                    }
                    this.psiMenu.Accept();
                    break;
                case Button.B:
                    if (this.psiMenu.HasSelection)
                    {
                        this.psiMenu.Cancel();
                        break;
                    }
                    this.psiMenu.Hide();
                    this.state = BattleInterfaceController.State.TopLevelSelection;
                    this.ShowButtonBar();
                    break;
            }
        }

        private void SpecialSelection(Button b)
        {
            switch (b)
            {
                case Button.B:
                    this.state = BattleInterfaceController.State.TopLevelSelection;
                    this.ShowButtonBar();
                    break;
            }
        }

        private void ItemSelection(Button b)
        {
            switch (b)
            {
                case Button.B:
                    this.state = BattleInterfaceController.State.TopLevelSelection;
                    this.ShowButtonBar();
                    break;
            }
        }

        private void TargetSelection(Button b)
        {
            switch (b)
            {
                case Button.A:
                    this.CompleteTargetSelection(this.buttonBar.SelectedAction);
                    break;
                case Button.B:
                    this.selectedTargetId = -1;
                    this.selectionState.TargetingMode = TargetingMode.None;
                    this.ResetTargetingSelection();
                    this.state = BattleInterfaceController.State.TopLevelSelection;
                    this.ShowButtonBar();
                    break;
            }
        }

        private void CompleteMenuUndo()
        {
            if (this.OnInteractionComplete == null)
                return;
            this.selectionState.Type = SelectionState.SelectionType.Undo;
            this.OnInteractionComplete(this.selectionState);
        }

        private void CompleteTargetSelection(ButtonBar.Action buttonAction)
        {
            if (this.OnInteractionComplete != null)
            {
                switch (buttonAction)
                {
                    case ButtonBar.Action.Bash:
                        this.selectionState.Type = SelectionState.SelectionType.Bash;
                        break;
                    case ButtonBar.Action.Psi:
                        this.selectionState.Type = SelectionState.SelectionType.PSI;
                        break;
                    case ButtonBar.Action.Talk:
                        this.selectionState.Type = SelectionState.SelectionType.Talk;
                        break;
                }
                switch (this.selectionState.TargetingMode)
                {
                    case TargetingMode.PartyMember:
                    case TargetingMode.Enemy:
                        this.selectionState.Targets = new Combatant[1]
                        {
              this.combatantController[this.selectedTargetId]
                        };
                        break;
                    case TargetingMode.AllPartyMembers:
                        this.selectionState.Targets = this.combatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
                        break;
                    case TargetingMode.AllEnemies:
                        this.selectionState.Targets = this.combatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
                        break;
                }
                this.selectionState.AttackIndex = 0;
                this.selectionState.ItemIndex = -1;
                this.state = BattleInterfaceController.State.Waiting;
                if (this.OnInteractionComplete != null)
                    this.OnInteractionComplete(this.selectionState);
            }
            this.selectedTargetId = -1;
            this.selectionState.TargetingMode = TargetingMode.None;
            this.ResetTargetingSelection();
        }

        private void CompleteMenuGuard()
        {
            if (this.OnInteractionComplete == null)
                return;
            this.selectionState.Type = SelectionState.SelectionType.Guard;
            this.selectionState.Targets = (Combatant[])null;
            this.selectionState.AttackIndex = -1;
            this.selectionState.ItemIndex = -1;
            this.state = BattleInterfaceController.State.Waiting;
            this.OnInteractionComplete(this.selectionState);
        }

        private void CompleteMenuRun()
        {
            if (this.OnInteractionComplete == null)
                return;
            this.selectionState.Type = SelectionState.SelectionType.Run;
            this.selectionState.Targets = (Combatant[])null;
            this.selectionState.AttackIndex = -1;
            this.selectionState.ItemIndex = -1;
            this.state = BattleInterfaceController.State.Waiting;
            this.OnInteractionComplete(this.selectionState);
        }

        public void BeginPlayerInteraction(CharacterType character)
        {
            int num = 0;
            PlayerCombatant playerCombatant = (PlayerCombatant)null;
            foreach (PlayerCombatant factionCombatant in this.combatantController.GetFactionCombatants(BattleFaction.PlayerTeam))
            {
                playerCombatant = factionCombatant;
                if (!(playerCombatant.Character == character))
                    ++num;
                else
                    break;
            }
            Combatant firstLiveCombatant = this.combatantController.GetFirstLiveCombatant(BattleFaction.PlayerTeam);
            bool showRun = firstLiveCombatant != null && firstLiveCombatant.ID == playerCombatant.ID;
            this.state = BattleInterfaceController.State.TopLevelSelection;
            this.buttonBar.SetActions(BattleButtonBars.GetActions(character, showRun));
            this.buttonBar.Show(0);
            this.textbox.Hide();
            this.cardBar.SelectedIndex = num;
        }

        public void EndPlayerInteraction() => this.cardBar.SelectedIndex = -1;

        public void SetActiveCard(int index) => this.cardBar.SelectedIndex = index;

        public void PopCard(int index, int height) => this.cardBar.PopCard(index, height);

        public void SetCardSpring(
          int index,
          BattleCard.SpringMode mode,
          Vector2f amplitude,
          Vector2f speed,
          Vector2f decay)
        {
            this.cardBar.SetSpring(index, mode, amplitude, speed, decay);
        }

        public void SetCardGroovy(int index, bool groovy) => this.cardBar.SetGroovy(index, groovy);

        public void AddCardSpring(int index, Vector2f amplitude, Vector2f speed, Vector2f decay) => this.cardBar.AddSpring(index, amplitude, speed, decay);

        public void SetCardGlow(int index, BattleCard.GlowType type) => this.cardBar.SetGlow(index, type);

        public void HideButtonBar() => this.buttonBar.Hide();

        public void ShowButtonBar()
        {
            this.textbox.Hide();
            this.buttonBar.Show();
        }

        public void ShowTextBox(string message, bool useButton)
        {
            this.textbox.AutoScroll = !useButton;
            if (this.textbox.HasPrinted)
                this.textbox.Enqueue(new PrintAction(PrintActionType.LineBreak, new object[0]));
            this.textbox.EnqueueAll((IEnumerable<PrintAction>)new TextProcessor(message).Actions);
            this.textbox.Enqueue(new PrintAction(PrintActionType.Prompt, new object[0]));
            this.textbox.Show();
            this.buttonBar.Hide();
        }

        public void ClearTextBox() => this.textbox.Clear();

        public void HideTextBox() => this.textbox.Hide();

        public void SetLetterboxing(float letterboxing)
        {
            this.topLetterboxTargetY = (float)-(int)(14.0 * (1.0 - (double)letterboxing));
            this.bottomLetterboxTargetY = (float)(180L - (long)(int)(35.0 * (double)letterboxing));
        }

        public void AddEnemy(int id)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)this.combatantController[id];
            this.enemyIDs.Add(id);
            IndexedColorGraphic indexedColorGraphic = new IndexedColorGraphic(EnemyGraphics.GetFilename(enemyCombatant.Enemy), "front", new Vector2f(), 0);
            this.AlignEnemyGraphics();
        }

        public void DoEnemyDeathAnimation(int id)
        {
            this.enemyDeathSound.Play();
            this.graphicModifiers.Add((IGraphicModifier)new GraphicDeathFader(this.enemyGraphics[id], 40));
        }

        public void RemoveAllModifiers() => this.graphicModifiers.Clear();

        public void RemoveEnemy(int id)
        {
            this.RemoveTalker((Graphic)this.enemyGraphics[id]);
            this.graphicModifiers.RemoveAll((Predicate<IGraphicModifier>)(x => x.Graphic == this.enemyGraphics[id]));
            this.pipeline.Remove((Renderable)this.enemyGraphics[id]);
            this.enemyGraphics[id].Dispose();
            this.enemyGraphics.Remove(id);
            this.enemyIDs.Remove(id);
            this.AlignEnemyGraphics();
        }

        public void UpdatePlayerCard(int id, int hp, int pp, float meter)
        {
            PlayerCombatant playerCombatant = (PlayerCombatant)this.combatantController[id];
            this.cardBar.SetHP(playerCombatant.PartyIndex, hp);
            this.cardBar.SetPP(playerCombatant.PartyIndex, pp);
            this.cardBar.SetMeter(playerCombatant.PartyIndex, meter);
        }

        public void Update()
        {
            this.textbox.Update();
            foreach (IGraphicModifier graphicModifier in this.graphicModifiers)
                graphicModifier.Update();
            this.graphicModifiers.RemoveAll((Predicate<IGraphicModifier>)(x => x.Done));
            foreach (PsiAnimator psiAnimator in this.psiAnimators)
                psiAnimator.Update();
            this.psiAnimators.RemoveAll((Predicate<PsiAnimator>)(x => x.Complete));
            foreach (DamageNumber damageNumber in this.damageNumbers)
                damageNumber.Update();
            if (this.youWon != null)
                this.youWon.Update();
            if (this.groovy != null)
                this.groovy.Update();
            this.comboCircle.Update();
            this.dimmer.Update();
            if ((double)this.topLetterboxY < (double)this.topLetterboxTargetY - 0.5 || (double)this.topLetterboxY > (double)this.topLetterboxTargetY + 0.5)
            {
                this.topLetterboxY += (this.topLetterboxTargetY - this.topLetterboxY) / 10f;
                this.topLetterbox.Position = new Vector2f(this.topLetterbox.Position.X, (float)(int)this.topLetterboxY);
            }
            else if ((int)this.topLetterboxY != (int)this.topLetterboxTargetY)
            {
                this.topLetterboxY = this.topLetterboxTargetY;
                this.topLetterbox.Position = new Vector2f(this.topLetterbox.Position.X, (float)(int)this.topLetterboxY);
            }
            if ((double)this.bottomLetterboxY > (double)this.bottomLetterboxTargetY + 0.5 || (double)this.bottomLetterboxY < (double)this.bottomLetterboxTargetY - 0.5)
            {
                this.bottomLetterboxY += (this.bottomLetterboxTargetY - this.bottomLetterboxY) / 10f;
                this.bottomLetterbox.Position = new Vector2f(this.bottomLetterbox.Position.X, (float)(int)this.bottomLetterboxY);
            }
            else if ((int)this.bottomLetterboxY != (int)this.bottomLetterboxTargetY)
            {
                this.bottomLetterboxY = this.bottomLetterboxTargetY;
                this.bottomLetterbox.Position = new Vector2f(this.bottomLetterbox.Position.X, (float)(int)this.bottomLetterboxY);
            }
            if (!this.textboxHideFlag)
                return;
            this.textbox.Hide();
            this.textboxHideFlag = false;
        }

        public void Draw(RenderTarget target)
        {
            target.Draw((Drawable)this.topLetterbox);
            target.Draw((Drawable)this.bottomLetterbox);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    foreach (Renderable renderable in this.enemyGraphics.Values)
                        renderable.Dispose();
                    foreach (Renderable renderable in this.selectionMarkers.Values)
                        renderable.Dispose();
                    foreach (IGraphicModifier graphicModifier in this.graphicModifiers)
                    {
                        if (graphicModifier is IDisposable)
                            ((IDisposable)graphicModifier).Dispose();
                    }
                    this.jingler.Stop();
                    this.jingler.Dispose();
                    this.cardBar.Dispose();
                }
                AudioManager.Instance.Unuse(this.moveBeepX);
                AudioManager.Instance.Unuse(this.moveBeepY);
                AudioManager.Instance.Unuse(this.selectBeep);
                AudioManager.Instance.Unuse(this.cancelBeep);
                AudioManager.Instance.Unuse(this.prePlayerAttack);
                AudioManager.Instance.Unuse(this.preEnemyAttack);
                AudioManager.Instance.Unuse(this.prePsiSound);
                AudioManager.Instance.Unuse(this.talkSound);
                AudioManager.Instance.Unuse(this.enemyDeathSound);
                foreach (KeyValuePair<CharacterType, List<CarbineSound>> comboSound in this.comboSoundMap)
                {
                    foreach (CarbineSound sound in comboSound.Value)
                        AudioManager.Instance.Unuse(sound);
                }
                AudioManager.Instance.Unuse(this.smashSound);
                AudioManager.Instance.Unuse(this.comboHitA);
                AudioManager.Instance.Unuse(this.comboHitB);
                AudioManager.Instance.Unuse(this.comboSuccess);
                AudioManager.Instance.Unuse(this.groovySound);
                AudioManager.Instance.Unuse(this.reflectSound);
                foreach (CarbineSound sound in this.winSounds.Values)
                    AudioManager.Instance.Unuse(sound);
                this.textbox.OnTextboxComplete -= new TextBox.CompletionHandler(this.TextboxComplete);
                this.textbox.OnTextTrigger -= new TextBox.TextTriggerHandler(this.TextTrigger);
                InputManager.Instance.AxisPressed -= new InputManager.AxisPressedHandler(this.AxisPressed);
                InputManager.Instance.ButtonPressed -= new InputManager.ButtonPressedHandler(this.ButtonPressed);
            }
            this.disposed = true;
        }

        private enum State
        {
            Waiting,
            TopLevelSelection,
            PsiTypeSelection,
            SpecialSelection,
            ItemSelection,
            EnemySelection,
            AllySelection,
        }

        public delegate void InteractionCompletionHandler(SelectionState state);

        public delegate void TextboxCompletionHandler();

        public delegate void TextTriggerHandler(int type, string[] args);
    }
}
