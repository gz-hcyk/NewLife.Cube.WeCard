using System.Security.Cryptography;
using System.Text;
using NewLife;

namespace NewLife.Cube.WeCard;

/// <summary>
/// 自建应用事件加解密。AES-128-CBC、ZeroPadding，输出小写十六进制。
/// KEY = AppKey，IV = AppSecret 前 16 位。
/// 文档 https://wiki.weixiao.qq.com/api/selfbuilt/tools/crypt.html
/// 填充与文档中的 PHP 示例一致：长度已对齐时仍补一整块 0x00。
/// </summary>
public sealed class WeCardCrypt
{
    private readonly Byte[] _key;
    private readonly Byte[] _iv;

    /// <summary>实例化</summary>
    /// <param name="appKey">16 字节 AppKey</param>
    /// <param name="appSecret">至少 16 字节，取前 16 字节做 IV</param>
    public WeCardCrypt(String appKey, String appSecret)
    {
        if (appKey.IsNullOrEmpty()) throw new ArgumentNullException(nameof(appKey));
        if (appSecret.IsNullOrEmpty() || appSecret.Length < 16)
            throw new ArgumentException("AppSecret 至少 16 位，IV 取其前 16 位", nameof(appSecret));

        _key = Encoding.UTF8.GetBytes(appKey);
        if (_key.Length != 16)
            throw new ArgumentException($"AppKey 应为 16 字节，实际 {_key.Length}", nameof(appKey));
        _iv = Encoding.UTF8.GetBytes(appSecret.Substring(0, 16));
    }

    /// <summary>加密为小写十六进制</summary>
    /// <param name="plain">明文。空串按空明文加密</param>
    /// <returns>十六进制密文</returns>
    public String Encrypt(String plain) => Encrypt(plain ?? "", _key, _iv);

    /// <summary>解密十六进制密文，并去掉尾部 0x00</summary>
    /// <param name="hexCipher">十六进制密文</param>
    /// <returns>明文</returns>
    public String Decrypt(String hexCipher) => Decrypt(hexCipher, _key, _iv);

    /// <summary>加密。供单测直接传入文档示例密钥。</summary>
    /// <param name="plain">明文</param>
    /// <param name="key">16 字节密钥</param>
    /// <param name="iv">16 字节 IV</param>
    /// <returns>小写十六进制</returns>
    public static String Encrypt(String plain, Byte[] key, Byte[] iv)
    {
        var data = Encoding.UTF8.GetBytes(plain ?? "");
        var pad = 16 - (data.Length % 16);
        if (pad == 0) pad = 16;
        var buf = new Byte[data.Length + pad];
        Buffer.BlockCopy(data, 0, buf, 0, data.Length);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.None;
        using var encryptor = aes.CreateEncryptor();
        var cipher = encryptor.TransformFinalBlock(buf, 0, buf.Length);
        return Convert.ToHexString(cipher).ToLowerInvariant();
    }

    /// <summary>解密并去掉 ZeroPadding。</summary>
    /// <param name="hexCipher">十六进制密文</param>
    /// <param name="key">16 字节密钥</param>
    /// <param name="iv">16 字节 IV</param>
    /// <returns>明文</returns>
    public static String Decrypt(String hexCipher, Byte[] key, Byte[] iv)
    {
        if (hexCipher.IsNullOrEmpty()) throw new ArgumentNullException(nameof(hexCipher));
        var cipher = Convert.FromHexString(hexCipher.Trim());
        if (cipher.Length == 0 || cipher.Length % 16 != 0)
            throw new InvalidDataException("密文长度不是 16 的倍数");

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.None;
        using var decryptor = aes.CreateDecryptor();
        var plain = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        var end = plain.Length;
        while (end > 0 && plain[end - 1] == 0) end--;
        return Encoding.UTF8.GetString(plain, 0, end);
    }
}
