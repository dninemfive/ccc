using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace D9CCC
{
    public class ScenPart_D9ClimateCycle : ScenPart
    {
        private float yearsOffset = 0f;
        private String yobuf;

        private bool useOffset = true;

        private float PeriodYears = 4f;
        private String pybuf; //"period years buffer"

        private float tempOffsetFactor = 20f;
        private String tofbuf;

        private float tempOffsetOffset = 0f;
        private String toobuf;

        private float xCoeff = 0f; //x coefficient, increasing the temperature linearly for runaway scenarios
        private String xbuf;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref yearsOffset, "yearsOffset", 0f, false);
            Scribe_Values.Look(ref useOffset, "useOffset", true, false);
            Scribe_Values.Look(ref PeriodYears, "PeriodYears", 4f, false);
            Scribe_Values.Look(ref tempOffsetFactor, "tempOffsetFactor", 20f, false);
            Scribe_Values.Look(ref tempOffsetOffset, "tempOffsetOffset", 0f, false);
            Scribe_Values.Look(ref xCoeff, "xCoeff", 0f, false);
        }

        public override void DoEditInterface(Listing_ScenEdit listing)
        {
            float heightFactor = 7f;
            Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * heightFactor);
            Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / heightFactor);
            Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / heightFactor, scenPartRect.width, scenPartRect.height / heightFactor);
            Rect rect3 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / heightFactor, scenPartRect.width, scenPartRect.height / heightFactor);
            Rect rect4 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 3f / heightFactor, scenPartRect.width, scenPartRect.height / heightFactor);
            Rect rect5 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 4f / heightFactor, scenPartRect.width, scenPartRect.height / heightFactor);
            Rect rect6 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 5f / heightFactor, scenPartRect.width, scenPartRect.height / heightFactor);
            Rect rect7 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 6f / heightFactor, scenPartRect.width, scenPartRect.height / heightFactor);
            Widgets.TextFieldNumericLabeled(rect2, "d9cccOffset".Translate(), ref yearsOffset, ref yobuf, 0f, PeriodYears);
            Widgets.CheckboxLabeled(rect3, "d9cccUseOffset".Translate(), ref useOffset, false, null, null, false);
            Widgets.TextFieldNumericLabeled(rect4, "d9cccPeriod".Translate(), ref PeriodYears, ref pybuf, 0f, 1000f);
            Widgets.TextFieldNumericLabeled(rect5, "d9cccTempFactor".Translate(), ref tempOffsetFactor, ref tofbuf, 0f, 1000f);
            Widgets.TextFieldNumericLabeled(rect6, "d9cccTempOffset".Translate(), ref tempOffsetOffset, ref toobuf, -1000f, 1000f);
            Widgets.TextFieldNumericLabeled(rect7, "d9cccXCoeff".Translate(), ref xCoeff, ref xbuf, -100f, 100f);
        }
        public override void PostWorldGenerate()
        {
            //GameCondition gccc = (GameCondition)Activator.CreateInstance(typeof(GameCondition_D9ClimateCycle));
            GameCondition_D9ClimateCycle gccc = new GameCondition_D9ClimateCycle();
            if(useOffset)gccc.setup((int)(yearsOffset * GenDate.TicksPerYear), PeriodYears, tempOffsetFactor, tempOffsetOffset, xCoeff);
            else gccc.setup(PeriodYears, tempOffsetFactor, tempOffsetOffset, xCoeff);
            gccc.def = defOf.D9CustomClimateCycle;
            Find.World.gameConditionManager.RegisterCondition(gccc);
        }
    }
}
