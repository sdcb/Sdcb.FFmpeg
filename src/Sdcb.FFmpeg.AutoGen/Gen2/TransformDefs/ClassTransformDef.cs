using System.Collections.Generic;
using System.Linq;

namespace Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs
{
    internal record ClassTransformDef : G2TransformDef
    {
        public ClassTransformDef(ClassCategories ClassCategory, string OldName, string NewName, FieldDef[] FieldDefs, bool AllReadOnly)
            : base(ClassCategory, OldName, NewName, FieldDefs.ToDictionary(k => k.Name, v => v), AllReadOnly)
        {
        }

        protected override string DefinitionLineCode => $"public unsafe partial class {NewName} : SafeHandle";

        protected override IEnumerable<string> GetCommonHeaderCode()
        {
            yield return $"protected {OldName}* _ptr => ({OldName}*)handle;";
            yield return "";
            yield return $"public static implicit operator {OldName}*({NewName} data) => data != null ? ({OldName}*)data.handle : null;";
            yield return "";
            yield return $"protected {NewName}({OldName}* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)";
            yield return "{";
            yield return "}";
            yield return "";
            yield return $"public static {NewName} FromNative({OldName}* ptr, bool isOwner) => new {NewName}(ptr, isOwner);";
            yield return "";
            yield return $"internal static {NewName} FromNative(IntPtr ptr, bool isOwner) => new {NewName}(({OldName}*)ptr, isOwner);";
            yield return "";
            yield return $"public static {NewName}? FromNativeOrNull({OldName}* ptr, bool isOwner) => ptr == null ? null : new {NewName}(ptr, isOwner);";
            yield return "";
            yield return $"public override bool IsInvalid => handle == IntPtr.Zero;";
        }
    }
}
