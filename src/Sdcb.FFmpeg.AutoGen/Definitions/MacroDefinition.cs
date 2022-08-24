#nullable enable
using Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units;
using System;

namespace Sdcb.FFmpeg.AutoGen.Definitions
{
    internal record MacroDefinitionRaw(string Name, string RawExpressionText) : IDefinition;

    internal record MacroDefinitionBase(string Name, string RawExpressionText) : IDefinition, ICanGenerateXmlDoc
    {
        public virtual bool IsValid => false;
        
        public virtual string XmlDocument => $"{Name} = {RawExpressionText}";

        internal static MacroDefinitionBase FromFailed(string name, string rawExpressionText)
        {
            return new MacroDefinitionBase(name, rawExpressionText);
        }

        internal static MacroDefinitionGood FromSuccess(string name, string rawExpressionText, string typeName, bool isConst, string expressionText)
        {
            return new MacroDefinitionGood(name, rawExpressionText, isConst, typeName, expressionText);
        }
    }

    internal record MacroDefinitionGood(string Name, string RawExpressionText, bool IsConst, string TypeName, string ExpressionText) : MacroDefinitionBase(Name, RawExpressionText)
    {
        public override bool IsValid => true;
        public override string XmlDocument => RawExpressionText == ExpressionText ? "" : $"{Name} = {RawExpressionText}";
    }
}