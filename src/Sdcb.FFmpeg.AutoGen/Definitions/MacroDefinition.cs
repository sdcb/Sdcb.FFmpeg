using System;

namespace Sdcb.FFmpeg.AutoGen.Definitions
{
    internal record MacroDefinitionRaw : IDefinition
    {
        public string Name { get; init; }

        public string ExpressionText { get; init; }
    }

    internal record MacroDefinition : MacroDefinitionRaw, ICanGenerateXmlDoc
    {
        public string TypeName { get; init; }
        public bool IsValid { get; init; }
        public bool IsConst { get; init; }

        public string RawExpressionText { get; init; }

        public virtual string XmlDocument => $"{Name} = {RawExpressionText}";

        internal static MacroDefinition FromFailed(string name, string expressionText)
        {
            return new MacroDefinition
            {
                Name = name, 
                ExpressionText = expressionText, 
                IsValid = false, 
            };
        }

        internal static MacroDefinition FromSuccess(string name, string rawExpressionText, bool isConst, string typeName, string expressionText)
        {
            return new MacroDefinition
            {
                Name = name,
                RawExpressionText = rawExpressionText,
                ExpressionText = expressionText,
                IsValid = true,
                IsConst = isConst, 
                TypeName = typeName, 
            };
        }
    }
}