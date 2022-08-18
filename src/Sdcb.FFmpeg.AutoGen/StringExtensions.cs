using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.AutoGen
{
    internal class StringExtensions
    {
        public static string EnumNameTransform(string name) => CSharpKeywordTransform(string.Concat(name
            .Split('_')
            .Select(x => x switch
            {
                var _ when char.IsDigit(x[0]) => $"_{x}",
                _ => char.ToUpper(x[0]) + x[1..].ToLower(),
            })));

        public static string CSharpKeywordTransform(string syntax) => syntax switch
        {
            _ when IsCSharpKeyword(syntax) => "@" + syntax,
            _ => syntax
        };

        public static bool IsCSharpKeyword(string key) => _csharpKeywords.Contains(key);

        public static readonly HashSet<string> _csharpKeywords = ("abstract,as,base,bool,break,byte,case," +
                    "catch,char,checked,class,const,continue,decimal,default,delegate,do," +
                    "double,else,enum,event,explicit,extern,false,finally,fixed,float,for," +
                    "foreach,goto,if,implicit,in,int,interface,internal,is,lock,long,namespace," +
                    "new,null,object,operator,out,override,params,private,protected,public," +
                    "readonly,ref,return,sbyte,sealed,short,sizeof,stackalloc,static,string," +
                    "struct,switch,this,throw,true,try,typeof,uint,ulong,unchecked,unsafe," +
                    "ushort,using,virtual,void,volatile,while").Split(',').ToHashSet();

        public static string CommonPrefixOf(IEnumerable<string> strings)
        {
            string commonPrefix = strings.FirstOrDefault() ?? "";

            foreach (var s in strings)
            {
                var potentialMatchLength = Math.Min(s.Length, commonPrefix.Length);

                if (potentialMatchLength < commonPrefix.Length)
                    commonPrefix = commonPrefix.Substring(0, potentialMatchLength);

                for (var i = 0; i < potentialMatchLength; i++)
                {
                    if (s[i] != commonPrefix[i])
                    {
                        commonPrefix = commonPrefix.Substring(0, i);
                        break;
                    }
                }
            }

            return commonPrefix;
        }
    }
}
