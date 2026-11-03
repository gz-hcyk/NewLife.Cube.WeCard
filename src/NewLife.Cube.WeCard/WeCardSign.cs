using System.Security.Cryptography;
using System.Text;
using NewLife;

namespace NewLife.Cube.WeCard;

/// <summary>
/// 微卡签名。算法见 https://wiki.weixiao.qq.com/api/selfbuilt/tools/signAlgorithm.html
/// 非空参数按参数名 ASCII 排序，拼成 key=value&amp;...，末尾加 &amp;key=APP_SECRET，MD5 后转大写。
/// sign / signature 自身不参与签名。
/// </summary>
public static class WeCardSign
{
    /// <summary>计算签名</summary>
    /// <param name="parameters">参数集合。空值不参与</param>
    /// <param name="appSecret">AppSecret，拼在 &amp;key= 后面</param>
    /// <returns>大写 MD5</returns>
    public static String Sign(IEnumerable<KeyValuePair<String, String>> parameters, String appSecret)
    {
        if (appSecret.IsNullOrEmpty()) throw new ArgumentNullException(nameof(appSecret));
        var list = new List<KeyValuePair<String, String>>();
        if (parameters != null)
        {
            foreach (var item in parameters)
            {
                if (item.Key.IsNullOrEmpty()) continue;
                if (item.Key.EqualIgnoreCase("sign") || item.Key.EqualIgnoreCase("signature")) continue;
                if (item.Value.IsNullOrEmpty()) continue;
                list.Add(new KeyValuePair<String, String>(item.Key, item.Value));
            }
        }

        list.Sort((a, b) => String.CompareOrdinal(a.Key, b.Key));
        var sb = new StringBuilder();
        for (var i = 0; i < list.Count; i++)
        {
            if (i > 0) sb.Append('&');
            sb.Append(list[i].Key).Append('=').Append(list[i].Value);
        }

        if (sb.Length > 0) sb.Append('&');
        sb.Append("key=").Append(appSecret);
        return Md5Upper(sb.ToString());
    }

    /// <summary>校验签名。签名字段缺失时返回 false。</summary>
    /// <param name="parameters">含 signature 或 sign 的参数</param>
    /// <param name="appSecret">AppSecret</param>
    /// <returns>true 表示一致</returns>
    public static Boolean Verify(IEnumerable<KeyValuePair<String, String>> parameters, String appSecret)
    {
        if (parameters == null) return false;
        String expect = null;
        foreach (var item in parameters)
        {
            if (item.Key.EqualIgnoreCase("signature") || item.Key.EqualIgnoreCase("sign"))
            {
                expect = item.Value;
                break;
            }
        }

        if (expect.IsNullOrEmpty()) return false;
        var actual = Sign(parameters, appSecret);
        return actual.EqualIgnoreCase(expect);
    }

    /// <summary>MD5 大写十六进制</summary>
    /// <param name="text">待摘要文本</param>
    /// <returns>32 位大写十六进制</returns>
    public static String Md5Upper(String text)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(text ?? ""));
        return Convert.ToHexString(hash);
    }
}
