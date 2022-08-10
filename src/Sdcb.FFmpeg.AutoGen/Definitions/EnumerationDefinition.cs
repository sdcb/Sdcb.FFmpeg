using System;
using System.Collections.Generic;

namespace Sdcb.FFmpeg.AutoGen.Definitions
{
    internal record EnumerationDefinition : NamedDefinition, IDefinition
    {
        public EnumerationItem[] Items { get; init; }

        public virtual bool Equals(EnumerationDefinition other) =>
            other is not null
            && Equals(Items, other.Items) && base.Equals(other);

        public override int GetHashCode()
        {
            HashCode hashcode = new();
            foreach (var item in Items) hashcode.Add(item);
            hashcode.Add(Content);
            hashcode.Add(base.GetHashCode());

            return hashcode.ToHashCode();
        }
    }
}