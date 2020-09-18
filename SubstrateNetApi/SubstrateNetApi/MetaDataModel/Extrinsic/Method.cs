﻿using System;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel
{
    public class Method
    {
        private byte _moduleIndex;

        private byte _callIndex;

        private byte[] _parameters;

        public Method(byte moduleIndex, byte callIndex, byte[] parameters)
        {
            _moduleIndex = moduleIndex;
            _callIndex = callIndex;
            _parameters = parameters;
        }
        
        public Method(byte moduleIndex, byte callIndex)
        {
            _moduleIndex = moduleIndex;
            _callIndex = callIndex;
            _parameters = new byte[0];
        }

        public byte[] Encode()
        {
            var result = new List<byte>();
            result.Add(_moduleIndex);
            result.Add(_callIndex);
            result.AddRange(_parameters);
            return result.ToArray();
        }

        // TODO this needs to be changed ... 
        public byte[] EncodeAsCall()
        {
            var bytes = new List<byte>();
            bytes.Add(0x04);
            bytes.Add(_moduleIndex);
            bytes.Add(_callIndex);
            bytes.AddRange(_parameters);
            return Utils.SizePrefixedByteArray(bytes);
        }
    }
}
