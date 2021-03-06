﻿using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class RustTuple<T1, T2> : StructType where T1 : IType, new()
                                                where T2 : IType, new()
    {
        public override string Name() => $"({new T1().Name()},{new T2().Name()})";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Value = new IType[2];

            var t1 = new T1();
            t1.Decode(byteArray, ref p);
            Value[0] = t1;

            var t2 = new T2();
            t2.Decode(byteArray, ref p);
            Value[1] = t2;

            _size = p - start;

            Bytes = new byte[_size];
            Array.Copy(byteArray, start, Bytes, 0, _size);
        }

        public IType[] Value { get; internal set; }
    }
}