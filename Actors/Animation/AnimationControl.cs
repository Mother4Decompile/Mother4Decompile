// Decompiled with JetBrains decompiler
// Type: Mother4.Actors.Animation.AnimationControl
// Assembly: Mother4, Version=0.7.6122.42121, Culture=neutral, PublicKeyToken=null
// MVID: FECD8919-57FF-4485-92CA-DA4098284AB3
// Assembly location: D:\OddityPrototypes\Mother 4 -- 2018\Mother4.exe

using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Overworld;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Mother4.Actors.Animation
{
    internal class AnimationControl
    {
        private IndexedColorGraphic graphic;
        private int previousDirection;
        private Vector2f previousVelocity;
        private AnimationType previousStance;
        private AnimationType defaultStance;
        private Dictionary<AnimationType, byte> counts;
        private bool hasStand;
        private bool hasWalk;
        private bool hasRun;
        private bool hasCrouch;
        private bool hasDead;
        private bool hasIdle;
        private bool hasTalk;
        private bool hasBlink;
        private bool isOverriden;
        private string overrideSubsprite;
        private AnimationType animationType;

        public bool Overriden => this.isOverriden;

        public AnimationControl(IndexedColorGraphic graphic, int initialDirection)
        {
            this.graphic = graphic;
            this.counts = new Dictionary<AnimationType, byte>();
            this.previousVelocity = VectorMath.ZERO_VECTOR;
            this.previousDirection = initialDirection;
            this.previousStance = AnimationType.INVALID;
            this.animationType = AnimationType.SOUTH | AnimationType.STAND;
            this.overrideSubsprite = string.Empty;
            this.Initialize();
        }

        private void Initialize()
        {
            foreach (SpriteDefinition spriteDefinition in (IEnumerable<SpriteDefinition>)((IndexedTexture)this.graphic.Texture).GetSpriteDefinitions())
            {
                AnimationType animationType = AnimationNames.GetAnimationType(spriteDefinition.Name.ToLowerInvariant());
                if (animationType != AnimationType.INVALID)
                {
                    AnimationType key1 = animationType & AnimationType.STANCE_MASK;
                    if (this.defaultStance == AnimationType.INVALID)
                        this.defaultStance = key1;
                    if (this.counts.ContainsKey(key1))
                    {
                        Dictionary<AnimationType, byte> counts;
                        AnimationType key2;
                        (counts = this.counts)[key2 = key1] = (byte)((uint)counts[key2] + 1U);
                    }
                    else
                        this.counts.Add(key1, (byte)1);
                }
            }
            this.hasStand = this.counts.ContainsKey(AnimationType.STAND);
            this.hasWalk = this.counts.ContainsKey(AnimationType.WALK);
            this.hasRun = this.counts.ContainsKey(AnimationType.RUN);
            this.hasCrouch = this.counts.ContainsKey(AnimationType.CROUCH);
            this.hasDead = this.counts.ContainsKey(AnimationType.DEAD);
            this.hasIdle = this.counts.ContainsKey(AnimationType.IDLE);
            this.hasTalk = this.counts.ContainsKey(AnimationType.TALK);
            this.hasBlink = this.counts.ContainsKey(AnimationType.BLINK);
        }

        public void ChangeGraphic(IndexedColorGraphic graphic)
        {
            this.graphic = graphic;
            this.counts.Clear();
            this.animationType = AnimationType.SOUTH | AnimationType.STAND;
            this.overrideSubsprite = string.Empty;
            this.Initialize();
        }

        public void OverrideSubsprite(string subsprite)
        {
            this.isOverriden = true;
            this.overrideSubsprite = subsprite;
            this.graphic.SpeedModifier = 1f;
        }

        public void ClearOverride()
        {
            this.isOverriden = false;
            this.overrideSubsprite = string.Empty;
        }

        private AnimationType GetDirectionPart(
          Vector2f velocity,
          int direction,
          int dirCount)
        {
            int num = direction;
            if (dirCount == 4 && num % 2 == 1)
                num = num > 4 ? 6 : 2;
            AnimationType directionPart;
            switch (num)
            {
                case 0:
                    directionPart = AnimationType.EAST;
                    break;
                case 1:
                    directionPart = AnimationType.NORTHEAST;
                    break;
                case 2:
                    directionPart = AnimationType.NORTH;
                    break;
                case 3:
                    directionPart = AnimationType.NORTHWEST;
                    break;
                case 4:
                    directionPart = AnimationType.WEST;
                    break;
                case 5:
                    directionPart = AnimationType.SOUTHWEST;
                    break;
                case 6:
                    directionPart = AnimationType.SOUTH;
                    break;
                case 7:
                    directionPart = AnimationType.SOUTHEAST;
                    break;
                default:
                    directionPart = AnimationType.SOUTH;
                    break;
            }
            return directionPart;
        }

        private void DetermineSubsprite(
          float velocityMagnitude,
          int direction,
          AnimationContext context)
        {
            AnimationType animationType = AnimationType.SOUTH;
            AnimationType key = !context.IsTalk ? (!context.IsDead ? (!context.IsCrouch ? (!context.IsNauseous ? ((double)velocityMagnitude < 2.0 ? ((double)velocityMagnitude <= 0.0 ? (context.TerrainType != TerrainType.Ocean ? AnimationType.STAND : AnimationType.FLOAT) : (context.TerrainType != TerrainType.Ocean ? AnimationType.WALK : AnimationType.SWIM)) : (context.TerrainType != TerrainType.Ocean ? (this.hasRun ? AnimationType.RUN : AnimationType.WALK) : AnimationType.SWIM)) : AnimationType.NAUSEA) : AnimationType.CROUCH) : AnimationType.DEAD) : AnimationType.TALK;
            if (!this.counts.ContainsKey(key) || this.counts[key] == (byte)0)
                key = this.defaultStance;
            if (key != AnimationType.INVALID)
                animationType = this.GetDirectionPart(context.Velocity, direction, (int)this.counts[key]);
            this.animationType = key | animationType;
        }

        public void UpdateSubsprite(AnimationContext context)
        {
            bool flag = false;
            float velocityMagnitude = VectorMath.Magnitude(context.Velocity);
            int direction;
            if ((double)velocityMagnitude > 0.0)
            {
                direction = (int)Math.Floor(Math.Atan2(-(double)context.Velocity.Y, (double)context.Velocity.X) / (Math.PI / 4.0));
                if (direction < 0)
                    direction += 8;
            }
            else
            {
                direction = context.SuggestedDirection;
                flag = !context.IsDead && !context.IsCrouch && !context.IsTalk;
            }
            bool reset = false;
            string overrideSubsprite;
            if (!this.isOverriden)
            {
                this.previousStance = this.animationType & AnimationType.STANCE_MASK;
                this.DetermineSubsprite(velocityMagnitude, direction, context);
                overrideSubsprite = AnimationNames.GetString(this.animationType);
                reset = this.previousStance != (this.animationType & AnimationType.STANCE_MASK);
            }
            else
                overrideSubsprite = this.overrideSubsprite;
            this.graphic.SetSprite(overrideSubsprite, reset);
            if (!this.isOverriden)
            {
                if (flag && (!this.hasStand || this.animationType == AnimationType.NAUSEA))
                {
                    this.graphic.SpeedModifier = 0.0f;
                    this.graphic.Frame = 0.0f;
                }
                else
                    this.graphic.SpeedModifier = 1f;
            }
            this.previousVelocity = context.Velocity;
            this.previousDirection = direction;
        }
    }
}
