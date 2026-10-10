using System.Security.Cryptography;
using System.Text;
using NewLife;

namespace NewLife.Cube.WeCard;

/// <summary>
/// 支付通知加解密。
/// 对称：AEAD_AES_256_GCM，文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/crypt/symmetricEncryption.html
/// 非对称：RSA-OAEP（SHA-512），文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/crypt/asymmetricEncryption.html
/// GCM 的 nonce 按 .NET 与 RFC 5116 推荐使用 12 字节。密文为 base64(原文密文 || 16 字节 tag)。
/// </summary>
public static class WeCardPayCrypt
{
    /// <summary>GCM 认证标签长度。</summary>
    public const Int32 TagSize = 16;

    /// <summary>GCM nonce 长度。</summary>
    public const Int32 NonceSize = 12;

    /// <summary>AES-256-GCM 加密</summary>
    /// <param name="plain">明文</param>
    /// <param name="key">32 字节密钥文本（UTF-8）</param>
    /// <param name="associatedData">附加数据，可空，文档要求短于 16 字节</param>
    /// <param name="nonce">12 字节随机串。空则自动生成</param>
    /// <returns>nonce 与 base64 密文</returns>
    public static (String Nonce, String CipherText) EncryptAesGcm(String plain, String key, String associatedData = null, String nonce = null)
    {
        var keyBytes = RequireKey(key);
        if (nonce.IsNullOrEmpty()) nonce = NewNonce();
        var nonceBytes = Encoding.UTF8.GetBytes(nonce);
        if (nonceBytes.Length != NonceSize)
            throw new ArgumentException($"nonce 需要 {NonceSize} 字节，实际 {nonceBytes.Length}", nameof(nonce));

        var aad = Encoding.UTF8.GetBytes(associatedData ?? "");
        var data = Encoding.UTF8.GetBytes(plain ?? "");
        var cipher = new Byte[data.Length];
        var tag = new Byte[TagSize];
        using var aes = new AesGcm(keyBytes, TagSize);
        aes.Encrypt(nonceBytes, data, cipher, tag, aad);

        var packed = new Byte[cipher.Length + tag.Length];
        Buffer.BlockCopy(cipher, 0, packed, 0, cipher.Length);
        Buffer.BlockCopy(tag, 0, packed, cipher.Length, tag.Length);
        return (nonce, Convert.ToBase64String(packed));
    }

    /// <summary>生成 12 位可见字符 nonce，UTF-8 长度正好 12 字节。</summary>
    /// <returns>nonce</returns>
    public static String NewNonce()
    {
        const String alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        var raw = RandomNumberGenerator.GetBytes(NonceSize);
        var chars = new Char[NonceSize];
        for (var i = 0; i < NonceSize; i++) chars[i] = alphabet[raw[i] % alphabet.Length];
        return new String(chars);
    }

    /// <summary>AES-256-GCM 解密</summary>
    /// <param name="cipherTextBase64">base64(密文 || tag)</param>
    /// <param name="key">32 字节密钥文本</param>
    /// <param name="nonce">12 字节 nonce 文本</param>
    /// <param name="associatedData">附加数据，可空</param>
    /// <returns>明文</returns>
    public static String DecryptAesGcm(String cipherTextBase64, String key, String nonce, String associatedData = null)
    {
        if (cipherTextBase64.IsNullOrEmpty()) throw new ArgumentNullException(nameof(cipherTextBase64));
        var keyBytes = RequireKey(key);
        var nonceBytes = Encoding.UTF8.GetBytes(nonce ?? "");
        if (nonceBytes.Length != NonceSize)
            throw new ArgumentException($"nonce 需要 {NonceSize} 字节，实际 {nonceBytes.Length}", nameof(nonce));

        var packed = Convert.FromBase64String(cipherTextBase64);
        if (packed.Length < TagSize) throw new InvalidDataException("GCM 密文短于 tag");
        var cipherLen = packed.Length - TagSize;
        var cipher = new Byte[cipherLen];
        var tag = new Byte[TagSize];
        Buffer.BlockCopy(packed, 0, cipher, 0, cipherLen);
        Buffer.BlockCopy(packed, cipherLen, tag, 0, TagSize);

        var plain = new Byte[cipherLen];
        var aad = Encoding.UTF8.GetBytes(associatedData ?? "");
        using var aes = new AesGcm(keyBytes, TagSize);
        aes.Decrypt(nonceBytes, cipher, tag, plain, aad);
        return Encoding.UTF8.GetString(plain);
    }

    /// <summary>RSA-OAEP-SHA512 公钥加密</summary>
    /// <param name="plain">明文</param>
    /// <param name="publicPem">SubjectPublicKeyInfo PEM</param>
    /// <returns>密文字节</returns>
    public static Byte[] EncryptRsaOaepSha512(Byte[] plain, String publicPem)
    {
        if (plain == null) throw new ArgumentNullException(nameof(plain));
        using var rsa = RSA.Create();
        rsa.ImportFromPem(publicPem);
        return rsa.Encrypt(plain, RSAEncryptionPadding.OaepSHA512);
    }

    /// <summary>RSA-OAEP-SHA512 私钥解密</summary>
    /// <param name="cipher">密文</param>
    /// <param name="privatePem">PKCS#1 或 PKCS#8 PEM</param>
    /// <returns>明文</returns>
    public static Byte[] DecryptRsaOaepSha512(Byte[] cipher, String privatePem)
    {
        if (cipher == null) throw new ArgumentNullException(nameof(cipher));
        using var rsa = RSA.Create();
        rsa.ImportFromPem(privatePem);
        return rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA512);
    }

    private static Byte[] RequireKey(String key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));
        var bytes = Encoding.UTF8.GetBytes(key);
        if (bytes.Length != 32)
            throw new ArgumentException($"通知密钥需 32 字节，实际 {bytes.Length}", nameof(key));
        return bytes;
    }
}
