namespace NewLife.Cube.WeCard;

/// <summary>标记一条自建应用接口。路径与文档均来自 selfbuilt 文档树，测试据此核对 URL。</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class WeCardApiAttribute : Attribute
{
    /// <summary>HTTP 方法</summary>
    public String Method { get; }

    /// <summary>文档中的路径，不含域名</summary>
    public String Path { get; }

    /// <summary>网关</summary>
    public WeCardHost Host { get; }

    /// <summary>凭证方式</summary>
    public WeCardAuth Auth { get; }

    /// <summary>官方文档页</summary>
    public String Doc { get; }

    /// <summary>实例化</summary>
    /// <param name="method">GET 或 POST</param>
    /// <param name="path">路径</param>
    /// <param name="host">网关</param>
    /// <param name="auth">凭证方式</param>
    /// <param name="doc">文档 URL</param>
    public WeCardApiAttribute(String method, String path, WeCardHost host, WeCardAuth auth, String doc)
    {
        Method = method;
        Path = path;
        Host = host;
        Auth = auth;
        Doc = doc;
    }
}
