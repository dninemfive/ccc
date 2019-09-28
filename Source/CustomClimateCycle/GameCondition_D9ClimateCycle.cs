using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace D9CCC
{
    class GameCondition_D9ClimateCycle : GameCondition
    {
        private int ticksOffset = 0;

        private float PeriodYears = 4f;

        private float tempOffsetFactor = 20f;

        private float tempOffsetOffset = 0f;

        private float xCoeff = 0f; //x coefficient, increasing the temperature linearly for runaway scenarios

        private const float tau = 3.14159274f * 2.0f;

        public override void Init()
        {
            ticksOffset = ((!(Rand.Value < 0.5f)) ? 7200000 : 0);
        }

        public void setup(float p, float tf, float to, float x)
        {            
            PeriodYears = p;
            ticksOffset = (int)(UnityEngine.Random.Range(0f, PeriodYears * GenDate.TicksPerYear));
            tempOffsetFactor = tf;
            tempOffsetOffset = to;
            xCoeff = x;
            make();
        }

        public void setup(int o, float p, float tf, float to, float x)
        {
            setup(p, tf, to, x);
            ticksOffset = o;
            make();
        }

        public void make() // homologous to GameConditionManager.MakePermanent(this)
        {
            startTick = Find.TickManager.TicksGame - 180000;
            Permanent = true;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksOffset, "ticksOffset", 0, false);
            Scribe_Values.Look(ref PeriodYears, "PeriodYears", 4f, false);
            Scribe_Values.Look(ref tempOffsetFactor, "tempOffsetFactor", 20f, false);
            Scribe_Values.Look(ref tempOffsetOffset, "tempOffsetOffset", 0f, false);
            Scribe_Values.Look(ref xCoeff, "xCoeff", 0f, false);
        }

        public override float TemperatureOffset()
        {
            return (Mathf.Sin((GenDate.YearsPassedFloat + (float)ticksOffset / (float)GenDate.TicksPerYear) / PeriodYears * tau) * tempOffsetFactor) + tempOffsetOffset + (xCoeff * GenDate.YearsPassedFloat);
        }
    }
}
