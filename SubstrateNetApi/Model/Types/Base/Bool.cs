﻿using System;

namespace SubstrateNetApi.Model.Types.Base
{
    public class Bool : BaseType<bool>
    {
        public override string Name() => "bool";

        public override int Size() => 1;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = byteArray[0] > 0;
        }

        public void Create(bool value)
        {
            Bytes = new byte[] { (byte)(value ? 0x01 : 0x00) };
            Value = value;
        }
    }
}