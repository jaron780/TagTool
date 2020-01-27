﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;

namespace TagTool.Common
{
    /// <summary>
    /// SH coefficients are defined using float arrays. The length depends on the order
    /// </summary>
    public static class SphericalHarmonics
    {
        public static readonly int Order0Count = 1;
        public static readonly int Order1Count = 4;
        public static readonly int Order2Count = 9;
        public static readonly int Order3Count = 16;
        public static readonly int Order4Count = 25;
        public static readonly int Order5Count = 36;

        public static int GetCoefficientCount(int order)
        {
            if (order == 0)
                return 1;
            else
                return order * order;
        }

        
    }

    [TagStructure(Size = 0x5C)]
    public class Airprobe : TagStructure
    {
        public RealPoint3d Position;
        public StringId Name;
        public AirprobeFlags Flags;
        public CompressedSH SHCoeffcients;
    }

    [Flags]
    public enum AirprobeFlags : int
    {
        None = 0
    }

    [TagStructure(Size = 0x48)]
    public class CompressedSH : TagStructure
    {
        [TagField(Length = 3)]
        public short[] DominantLightDirection = new short[3];

        public short Padding1;

        [TagField(Length = 3)]
        public short[] DominantLightIntensity = new short[3];

        public short Padding2;

        [TagField(Length = 9)]
        public short[] SHRed = new short[SphericalHarmonics.Order2Count];
        [TagField(Length = 9)]
        public short[] SHGreen = new short[SphericalHarmonics.Order2Count];
        [TagField(Length = 9)]
        public short[] SHBlue = new short[SphericalHarmonics.Order2Count];

        public short Padding3;
    }

    [TagStructure(Size = 0x50)]
    public class UnknownSHBlock2 : TagStructure
    {
        public int Unknown1;
        public short Unknown2;
        public byte Unknown3;
        public byte Unknown4;
        public CompressedSH SHCoefficients;
    }

    [TagStructure(Size = 0x2C)]
    public class UnknownSHBlock3 : TagStructure
    {
        public uint Unknown;
        public short Unknown2;
        public short Unknown3;
        public float Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public float Unknown9;
        public List<UnknownBlock> Unknown10;

        [TagStructure(Size = 0x54)]
        public class UnknownBlock : TagStructure
        {
            public RealPoint3d Position;
            public CompressedSH SHCoefficients;
        }
    }
}
