﻿using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Types
{
    public abstract class StructType : IType
    {
        public abstract string Name();

        [JsonIgnore]
        public byte[] Bytes { get; internal set; }

        public abstract int Size();

        public abstract byte[] Encode();

        public abstract void Decode(byte[] byteArray, ref int p);


        public virtual void Create(string str) => Create(Utils.HexToByteArray(str));

        public virtual void CreateFromJson(string str) => Create(Utils.HexToByteArray(str));

        public void Create(byte[] byteArray)
        {
            var p = 0;
            Decode(byteArray, ref p);
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}