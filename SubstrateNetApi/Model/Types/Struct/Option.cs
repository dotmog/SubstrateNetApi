﻿using SubstrateNetApi.Model.Types.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class Option<T> : StructType where T : IType, new()
    {
        public override string Name() => $"Option<{new T().Name()}>";

        private int _size;
        public override int Size() => _size;

        public bool OptionFlag { get; set; }

        public override byte[] Encode()
        {
           var bytes = new List<byte>();
           if (OptionFlag)
            {
                bytes.Add(1);
                bytes.AddRange(Value.Encode());

            } else
            {
                bytes.Add(0);
            }

            return bytes.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            var optionByte = new U8();
            optionByte.Decode(byteArray, ref p);

            OptionFlag = optionByte.Value > 0;

            T t = default;
            if (optionByte.Value > 0)
            {
                t = new T();
                t.Decode(byteArray, ref p);
            }

            _size = p - start;

            var bytes = new byte[_size];
            Array.Copy(byteArray, start, bytes, 0, _size);

            Bytes = bytes;
            Value = t != null ? t : default;
        }

        public override void CreateFromJson(string str)
        {
            Create(Utils.HexToByteArray(str));
        }

        public T Value { get; internal set; }

        public void Create(T value)
        {
            OptionFlag = value != null;
            Value = value;
            Bytes = Encode();
        }

    }
}