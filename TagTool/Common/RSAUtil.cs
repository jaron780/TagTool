﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace TagTool.Common
{
    class RSAUtil
    {
        /// <summary>
        /// Decodes an RSA private key from ASN.1/DER format
        /// </summary>
        /// <param name="privateKey">The private key</param>
        /// <returns>An <see cref="RSACryptoServiceProvider"/> initialized with the given <paramref name="privateKey"/></returns>
        public static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privateKey)
        {
            using (var reader = new BinaryReader(new MemoryStream(privateKey)))
            {
                ushort val = reader.ReadUInt16();
                if (val == 0x8130) // 0x30, 0x81
                    reader.ReadByte();
                else if (val == 0x8230)
                    reader.ReadInt16();
                else
                    return null;

                if (reader.ReadUInt16() != 0x0102) //version
                    return null;

                if (reader.ReadByte() != 0x00)
                    return null;

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = reader.ReadBytes(GetIntegerSize(reader));
                RSAparams.Exponent = reader.ReadBytes(GetIntegerSize(reader));
                RSAparams.D = reader.ReadBytes(GetIntegerSize(reader));
                RSAparams.P = reader.ReadBytes(GetIntegerSize(reader));
                RSAparams.Q = reader.ReadBytes(GetIntegerSize(reader));
                RSAparams.DP = reader.ReadBytes(GetIntegerSize(reader));
                RSAparams.DQ = reader.ReadBytes(GetIntegerSize(reader));
                RSAparams.InverseQ = reader.ReadBytes(GetIntegerSize(reader));
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
        }

        private static int GetIntegerSize(BinaryReader reader)
        {
            byte val = reader.ReadByte();

            // check the next value is an int
            if (val != 0x02)
                return 0;

            val = reader.ReadByte();

            int length = 0;
            if (val == 0x81) // byte
            {
                length = reader.ReadByte();
            }
            else if (val == 0x82) // short
            {
                byte hi = reader.ReadByte();
                byte lo = reader.ReadByte();
                byte[] lengthBytes = { lo, hi, 0x00, 0x00 };
                length = BitConverter.ToInt32(lengthBytes, 0);
            }
            else // long
            {
                length = val;
            }

            // remove high order zeros
            while (reader.ReadByte() == 0x00)
                length -= 1;

            reader.BaseStream.Position--;
            return length;
        }
    }
}
