using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

/// <summary>
/// <see cref="AVDictionary"/>
/// </summary>
public unsafe class MediaDictionary : SafeHandle, IDictionary<string, string>
{
    private static IntPtr Invalid = IntPtr.Zero;

    public override bool IsInvalid => handle == Invalid;

    public MediaDictionary() : base(Invalid, true)
    {
    }

    private MediaDictionary(AVDictionary* dict, bool isOwner) : base((IntPtr)dict, isOwner) { }

    public static MediaDictionary FromDictionary(IDictionary<string, string> dict)
    {
        var md = new MediaDictionary();
        foreach (var entry in dict)
        {
            md[entry.Key] = entry.Value;
        }
        return md;
    }

    public static MediaDictionary FromNative(AVDictionary* dict, bool isOwner) => new MediaDictionary(dict, isOwner);

    public static implicit operator AVDictionary*(MediaDictionary? dict) => dict != null ? (AVDictionary*)dict?.handle : null;

    #region IDictionary<string, string> entries
    public ICollection<string> Keys => this.Select(x => x.Key).ToArray();

    public ICollection<string> Values => this.Select(x => x.Value).ToArray();

    public int Count => av_dict_count(this);

    public bool IsReadOnly => false;

    public string this[string key]
    {
        get
        {
            AVDictionaryEntry* entry = av_dict_get(this, key, null, (int)AV_DICT_READ.MatchCase);
            if (entry == null)
            {
                throw new KeyNotFoundException(key);
            }

            return PtrExtensions.PtrToStringUTF8((IntPtr)entry->value)!;
        }
        set
        {
            AVDictionary* ptr = this;
            av_dict_set(&ptr, key, value, default).ThrowIfError();
            handle = (IntPtr)ptr;
        }
    }

    public void Add(string key, string value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value)); // in AVDictionary, value is also not-null.

        if (ContainsKey(key))
        {
            throw new ArgumentException($"An item with the same key has already been added. Key: {key}");
        }

        AVDictionary* ptr = this;
        av_dict_set(&ptr, key, value, default).ThrowIfError();
        handle = (IntPtr)ptr;
    }

    public bool ContainsKey(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        AVDictionaryEntry* entry = av_dict_get(this, key, null, (int)AV_DICT_READ.MatchCase);
        return entry != null;
    }

    public bool Remove(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        AVDictionary* ptr = this;
        bool containsKey = ContainsKey(key);

        av_dict_set(&ptr, key, null, 0).ThrowIfError();
        handle = (IntPtr)ptr;
        return containsKey;
    }

    public bool TryGetValue(string key, [NotNullWhen(returnValue: true)] out string value)
    {
        AVDictionaryEntry* entry = av_dict_get(this, key, null, (int)AV_DICT_READ.MatchCase);
        if (entry == null)
        {
            value = null!;
            return false;
        }
        else
        {
            value = PtrExtensions.PtrToStringUTF8((IntPtr)entry->value)!;
            return true;
        }
    }

    void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
    {
        Add(item.Key, item.Value);
    }

    bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
    {
        return TryGetValue(item.Key, out string? value) && value == item.Value;
    }

    bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
    {
        if (TryGetValue(item.Key, out string? value) && value == item.Value)
        {
            Remove(item.Key);
            return true;
        }
        return false;
    }

    public void Clear() => Free();

    void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if (arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < Count) throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        foreach (KeyValuePair<string, string> entry in this)
        {
            array[arrayIndex++] = entry;
        }
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        IntPtr opaque = IntPtr.Zero;
        while (true)
        {
            opaque = av_dict_get_safe(this, opaque);
            if (opaque == IntPtr.Zero) yield break;

            yield return GenerateResult(opaque);
        }

        static KeyValuePair<string, string> GenerateResult(IntPtr ptr)
        {
            var entry = (AVDictionaryEntry*)ptr;
            return new KeyValuePair<string, string>(
                PtrExtensions.PtrToStringUTF8((IntPtr)entry->key)!,
                PtrExtensions.PtrToStringUTF8((IntPtr)entry->value)!);
        }

        static IntPtr av_dict_get_safe(MediaDictionary dict, IntPtr prev)
        {
            return (IntPtr)av_dict_get(dict, "", (AVDictionaryEntry*)prev, (int)AV_DICT_READ.IgnoreSuffix);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    public IEnumerable<KeyValuePair<string, string>> QueryPrefix(string prefix, bool caseSensitive = true)
    {
        IntPtr opaque = IntPtr.Zero;
        while (true)
        {
            opaque = av_dict_get_safe(this, opaque);
            if (opaque == IntPtr.Zero) yield break;

            yield return GenerateResult(opaque);
        }

        static KeyValuePair<string, string> GenerateResult(IntPtr ptr)
        {
            var entry = (AVDictionaryEntry*)ptr;
            return new KeyValuePair<string, string>(
                PtrExtensions.PtrToStringUTF8((IntPtr)entry->key)!,
                PtrExtensions.PtrToStringUTF8((IntPtr)entry->value)!);
        }

        IntPtr av_dict_get_safe(MediaDictionary dict, IntPtr prev)
        {
            return (IntPtr)av_dict_get(dict, prefix, (AVDictionaryEntry*)prev, (int)((caseSensitive ? AV_DICT_READ.MatchCase : default) | AV_DICT_READ.IgnoreSuffix));
        }
    }

    public void Set(string key, string value, AV_DICT_WRITE flags = default)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value)); // in AVDictionary, value is also not-null.

        AVDictionary* ptr = this;
        av_dict_set(&ptr, key, value, (int)flags).ThrowIfError();
        handle = (IntPtr)ptr;
    }

    /// <summary>
    /// <see cref="av_dict_copy(AVDictionary**, AVDictionary*, int)"/>
    /// </summary>
    public MediaDictionary Clone()
    {
        AVDictionary* destination = null;
        av_dict_copy(&destination, this, 0).ThrowIfError();
        return new MediaDictionary(destination, isOwner: true);
    }

    /// <summary>
    /// <see cref="av_dict_free(AVDictionary**)"/>
    /// </summary>
    public void Free()
    {
        AVDictionary* p = this;
        av_dict_free(&p);
        handle = Invalid;
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }

    public void Reset(AVDictionary* dict)
    {
        handle = (IntPtr)dict;
    }
}
