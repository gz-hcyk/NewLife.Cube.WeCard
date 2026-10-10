using System.Security.Cryptography;
using System.Text;
using NewLife.Cube.WeCard;
using Xunit;

namespace WeCard.Tests;

public class WeCardCryptTests
{
    [Fact]
    public void Encrypt_MatchesPhpSampleVector()
    {
        var key = Encoding.UTF8.GetBytes("1234567890123456");
        var iv = Encoding.UTF8.GetBytes("0123456789123456");
        var hex = WeCardCrypt.Encrypt("12345", key, iv);
        Assert.Equal("90b2c15e84cb78e5161f42867807c4bc", hex);
        Assert.Equal("12345", WeCardCrypt.Decrypt(hex, key, iv));
    }

    [Fact]
    public void Roundtrip_ChineseAndAlignedBlock()
    {
        var crypt = new WeCardCrypt("BE30B2BA30ABFADB", "CF8CDD8D0B7B1ACDA76201F406BED81E");
        var plain = "{\"card_number\":\"07302590\",\"name\":\"陈小明\"}";
        var hex = crypt.Encrypt(plain);
        Assert.Equal(plain, crypt.Decrypt(hex.ToUpperInvariant()));
        Assert.Equal(new String('测', 8), crypt.Decrypt(crypt.Encrypt(new String('测', 8))));
    }

    [Fact]
    public void Rejects_ShortKey()
    {
        Assert.Throws<ArgumentException>(() => new WeCardCrypt("short", "0123456789abcdef0123456789abcdef"));
        Assert.Throws<ArgumentException>(() => new WeCardCrypt("1234567890ABCDEF", "short-secret"));
    }

    [Fact]
    public void AesGcm_Roundtrip()
    {
        var key = "0123456789abcdef0123456789abcdef";
        var (nonce, cipher) = WeCardPayCrypt.EncryptAesGcm("{\"amount\":1}", key, "pay");
        Assert.Equal(12, Encoding.UTF8.GetByteCount(nonce));
        Assert.Equal("{\"amount\":1}", WeCardPayCrypt.DecryptAesGcm(cipher, key, nonce, "pay"));
    }

    [Fact]
    public void AesGcm_RejectsBadTag()
    {
        var key = "0123456789abcdef0123456789abcdef";
        var (nonce, cipher) = WeCardPayCrypt.EncryptAesGcm("x", key, "pay");
        var broken = Convert.ToBase64String(Convert.FromBase64String(cipher).Select((b, i) => i == 0 ? (Byte)(b ^ 1) : b).ToArray());
        Assert.ThrowsAny<CryptographicException>(() => WeCardPayCrypt.DecryptAesGcm(broken, key, nonce, "pay"));
    }

    [Fact]
    public void RsaOaepSha512_Roundtrip()
    {
        using var rsa = RSA.Create(2048);
        var pub = rsa.ExportSubjectPublicKeyInfoPem();
        var pri = rsa.ExportPkcs8PrivateKeyPem();
        var plain = Encoding.UTF8.GetBytes("wecard");
        var cipher = WeCardPayCrypt.EncryptRsaOaepSha512(plain, pub);
        Assert.Equal(plain, WeCardPayCrypt.DecryptRsaOaepSha512(cipher, pri));
    }
}
