using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public static class CharacterHelper
    {
        private static Dictionary<char, char> _unicodeToAsciiCharMapping;

        public static Dictionary<char, char> UnicodeToAsciiCharMapping
        {
            get { return _unicodeToAsciiCharMapping ?? (_unicodeToAsciiCharMapping = CreateCharMapping()); }
        }

        private static Dictionary<char, char> CreateCharMapping()
        {
            var from = new char[]
            {
                'à', 'á', 'â', 'ã', 'è', 'é', 'ê', 'ì', 'í', 'ò', 'ó', 'ô', 'õ', 'ù', 'ú', 'ý', 'ă', 'đ', 'ĩ', 'ũ', 'ơ',
                'ư', 'ạ', 'ả', 'ấ', 'ầ', 'ẩ', 'ẫ', 'ậ', 'ắ', 'ằ', 'ẳ', 'ẵ', 'ặ', 'ẹ', 'ẻ', 'ẽ', 'ế', 'ề', 'ể', 'ễ', 'ệ',
                'ỉ', 'ị', 'ọ', 'ỏ', 'ố', 'ồ', 'ổ', 'ỗ', 'ộ', 'ớ', 'ờ', 'ở', 'ỡ', 'ợ', 'ụ', 'ủ', 'ứ', 'ừ', 'ử', 'ữ', 'ự',
                'ỳ', 'ỵ', 'ỷ', 'ỹ'
            };
            var to = new char[]
            {
                'a', 'a', 'a', 'a', 'e', 'e', 'e', 'i', 'i', 'o', 'o', 'o', 'o', 'u', 'u', 'y', 'a', 'd', 'i', 'u', 'o',
                'u', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e',
                'i', 'i', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'u', 'u', 'u', 'u', 'u', 'u', 'u',
                'y', 'y', 'y', 'y'
            };
            var result = new Dictionary<char, char>();
            for (var i = 0; i < from.Count(); ++i)
            {
                result.Add(from[i], to[i]);
            }
            return result;
        }

        public static string MapUnicodeToAscii(string source)
        {
            var mapping = UnicodeToAsciiCharMapping;
            var des = new StringBuilder();
            foreach (var t in source)
            {
                des.Append(mapping.ContainsKey(t) ? mapping[t] : t);
            }
            return des.ToString();
        }
    }
}
