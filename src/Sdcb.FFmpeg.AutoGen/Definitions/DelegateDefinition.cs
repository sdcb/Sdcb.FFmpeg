using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Sdcb.FFmpeg.AutoGen.Definitions
{
    internal record DelegateDefinition : TypeDefinition, ICanGenerateXmlDoc
    {
        public string FunctionName { get; init; }
        public TypeDefinition ReturnType { get; init; }
        public FunctionParameter[] Parameters { get; init; }
        public string XmlDocument { get; set; }

        public virtual bool Equals(DelegateDefinition other) =>
            other is not null
            && base.Equals(other)
            && EqualityComparer<string>.Default.Equals(FunctionName, other.FunctionName)
            && EqualityComparer<TypeDefinition>.Default.Equals(ReturnType, other.ReturnType)
            && Parameters.SequenceEqual(other.Parameters)
            && EqualityComparer<string>.Default.Equals(XmlDocument, other.XmlDocument);

        public override int GetHashCode()
        {
            HashCode hashcode = new();
            hashcode.Add(FunctionName);
            hashcode.Add(ReturnType);
            foreach (var item in Parameters) hashcode.Add(item);
            hashcode.Add(XmlDocument);
            hashcode.Add(base.GetHashCode());

            return hashcode.ToHashCode();
        }
    }
}