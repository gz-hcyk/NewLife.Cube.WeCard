namespace NewLife.Cube.WeCard;

/// <summary>自建应用接口方法。路径来自 wiki.weixiao.qq.com/api/selfbuilt ，不含 /api/third*。</summary>
public partial class WeCardClient
{
    /// <summary>获取主体基本信息。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/mainpartInfo.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/campus/school-info", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/mainpartInfo.html")]
    public Task<IDictionary<String, Object>> GetSchoolInfoAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/campus/school-info", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>获取组织架构类型。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/orgTypes.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/org/types", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/orgTypes.html")]
    public Task<IDictionary<String, Object>> GetOrgTypesAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/org/types", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>获取组织架构。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/orgs.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/org-edu-list", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/orgs.html")]
    public Task<IDictionary<String, Object>> GetOrgsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/org-edu-list", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>按 id 获取组织架构。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/getOrgByIds.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/org/get-org-by-ids", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/getOrgByIds.html")]
    public Task<IDictionary<String, Object>> GetOrgByIdsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/org/get-org-by-ids", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>获取组织架构成员。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/orgUsers.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/get-org-users", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/orgUsers.html")]
    public Task<IDictionary<String, Object>> GetOrgUsersAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/get-org-users", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>按电子卡号获取成员。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUserByCardNumber.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/get-user-by-card-numbers", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUserByCardNumber.html")]
    public Task<IDictionary<String, Object>> GetUsersByCardNumbersAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/get-user-by-card-numbers", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>搜索成员。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/searchUser.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/search", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/searchUser.html")]
    public Task<IDictionary<String, Object>> SearchUsersAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/search", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>拉取成员变更记录。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUpdateUser.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/get-update-records", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUpdateUser.html")]
    public Task<IDictionary<String, Object>> GetUpdatedUsersAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/get-update-records", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>获取成员身份类型。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/userIdentityType.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/get-identity-type", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/userIdentityType.html")]
    public Task<IDictionary<String, Object>> GetUserIdentityTypeAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/get-identity-type", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>获取成员绑卡状态。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUserBindStatus.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/get-user-bind-card-status", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUserBindStatus.html")]
    public Task<IDictionary<String, Object>> GetUserBindStatusAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/get-user-bind-card-status", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>按卡号获取企业微信 userid。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUidByCardNumber.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/wxwork/get-user-id", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUidByCardNumber.html")]
    public Task<IDictionary<String, Object>> GetWxWorkUserIdAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/wxwork/get-user-id", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>即时创建或更新用户。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/userCreateNow.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/manage/create", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/userCreateNow.html")]
    public Task<IDictionary<String, Object>> CreateUserAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/manage/create", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>批量删除用户。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/batchDeleteUser.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/manage/batch-delete", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/batchDeleteUser.html")]
    public Task<IDictionary<String, Object>> BatchDeleteUsersAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/manage/batch-delete", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>变更 NFC。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/changeNfc.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/change-nfc", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/changeNfc.html")]
    public Task<IDictionary<String, Object>> ChangeNfcAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/change-nfc", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>移除 NFC。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/removeNfc.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/remove-nfc", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/removeNfc.html")]
    public Task<IDictionary<String, Object>> RemoveNfcAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/remove-nfc", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>重置 NFC 卡面。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/resetNfcCardFace.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/reset-nfc-face", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/resetNfcCardFace.html")]
    public Task<IDictionary<String, Object>> ResetNfcFaceAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/reset-nfc-face", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>主动推送学生信息。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/pushStuInfo.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/manage/sync-user-info", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/pushStuInfo.html")]
    public Task<IDictionary<String, Object>> PushStudentInfoAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/manage/sync-user-info", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>按 id 获取标签。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/tag/getTagById.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/get-tag-by-id", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/tag/getTagById.html")]
    public Task<IDictionary<String, Object>> GetTagByIdAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/get-tag-by-id", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>新增角色组。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/addRoleGroups.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/role/add-custom-role-group", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/addRoleGroups.html")]
    public Task<IDictionary<String, Object>> AddRoleGroupAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/role/add-custom-role-group", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>新增角色。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/addRoles.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/role/add-custom-role", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/addRoles.html")]
    public Task<IDictionary<String, Object>> AddRoleAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/role/add-custom-role", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>角色批量添加成员。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/batchAddRoles.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/role/add-members", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/batchAddRoles.html")]
    public Task<IDictionary<String, Object>> BatchAddRoleMembersAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/role/add-members", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>角色批量移除成员。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/batchDelRoles.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/role/del-members", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/batchDelRoles.html")]
    public Task<IDictionary<String, Object>> BatchDeleteRoleMembersAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/role/del-members", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>删除角色组。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/delRoleGroups.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/role/del-custom-role-group", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/delRoleGroups.html")]
    public Task<IDictionary<String, Object>> DeleteRoleGroupAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/role/del-custom-role-group", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>删除角色。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/delRoles.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/role/del-custom-role", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/delRoles.html")]
    public Task<IDictionary<String, Object>> DeleteRoleAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/role/del-custom-role", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>获取角色组。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/roleGroups.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/role/groups", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/roleGroups.html")]
    public Task<IDictionary<String, Object>> GetRoleGroupsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/role/groups", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>获取角色。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/roles.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/role/roles", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/roles.html")]
    public Task<IDictionary<String, Object>> GetRolesAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/role/roles", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>获取权限组列表。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/permissionGroups.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/permission/group-list", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/permissionGroups.html")]
    public Task<IDictionary<String, Object>> GetPermissionGroupsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/permission/group-list", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>获取权限组详情。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/permissionGroupDetail.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/permission/group", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/permissionGroupDetail.html")]
    public Task<IDictionary<String, Object>> GetPermissionGroupAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/permission/group", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>获取用户权限。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/userPermission.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/permission/user-permission", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/userPermission.html")]
    public Task<IDictionary<String, Object>> GetUserPermissionAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/permission/user-permission", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>同步物理卡。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/physical-card-sync.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/physical-card/sync", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/physical-card-sync.html")]
    public Task<IDictionary<String, Object>> SyncPhysicalCardAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/physical-card/sync", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>更新物理卡。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/physical-card-update.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/physical-card/update", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/physical-card-update.html")]
    public Task<IDictionary<String, Object>> UpdatePhysicalCardAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/physical-card/update", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>解绑物理卡。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/relieve-card.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user/manage/relieve-card", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/relieve-card.html")]
    public Task<IDictionary<String, Object>> RelievePhysicalCardAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user/manage/relieve-card", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>电子卡跳转小程序获取成员信息。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/e_card/cardMnpStu.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/apps/school-api/mnp-student", WeCardHost.School, WeCardAuth.SignedBody, "https://wiki.weixiao.qq.com/api/selfbuilt/e_card/cardMnpStu.html")]
    public Task<IDictionary<String, Object>> GetMiniProgramStudentAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.School, "/apps/school-api/mnp-student", body, ocode, WeCardAuth.SignedBody, cancellationToken);

    /// <summary>小程序添加电子卡。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/e_card/stuMnpAddCard.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/apps/school-api/add-student-card", WeCardHost.School, WeCardAuth.SignedQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/e_card/stuMnpAddCard.html")]
    public Task<IDictionary<String, Object>> AddStudentCardAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.School, "/apps/school-api/add-student-card", body, ocode, WeCardAuth.SignedQuery, cancellationToken);

    /// <summary>小程序打开用户电子卡。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/e_card/stuMnpCardInfo.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/apps/school-api/student-cards", WeCardHost.School, WeCardAuth.SignedQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/e_card/stuMnpCardInfo.html")]
    public Task<IDictionary<String, Object>> GetStudentCardsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.School, "/apps/school-api/student-cards", body, ocode, WeCardAuth.SignedQuery, cancellationToken);

    /// <summary>电子码在线解码。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/e_code/campuscode.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/apps/school-api/campus-code", WeCardHost.School, WeCardAuth.SignedBody, "https://wiki.weixiao.qq.com/api/selfbuilt/e_code/campuscode.html")]
    public Task<IDictionary<String, Object>> DecodeCampusCodeAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.School, "/apps/school-api/campus-code", body, ocode, WeCardAuth.SignedBody, cancellationToken);

    /// <summary>电子码扫码事件通知。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/e_code/scanNotify.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/apps/school-api/scan-notify", WeCardHost.School, WeCardAuth.SignedBody, "https://wiki.weixiao.qq.com/api/selfbuilt/e_code/scanNotify.html")]
    public Task<IDictionary<String, Object>> NotifyCampusScanAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.School, "/apps/school-api/scan-notify", body, ocode, WeCardAuth.SignedBody, cancellationToken);

    /// <summary>用小程序 openid 换微卡身份。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/mnpPlugin.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/apps/wxmnp/plugin/student-before-plugin", WeCardHost.School, WeCardAuth.SignedBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/mnpPlugin.html")]
    public Task<IDictionary<String, Object>> GetMiniProgramIdentityAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.School, "/apps/wxmnp/plugin/student-before-plugin", body, ocode, WeCardAuth.SignedBody, cancellationToken);

    /// <summary>查询通知能力。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeAble.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/notice/ability", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeAble.html")]
    public Task<IDictionary<String, Object>> GetNoticeAbilityAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/notice/ability", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>通知数据明细。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeDetail.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/notice/details", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeDetail.html")]
    public Task<IDictionary<String, Object>> GetNoticeDetailAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/notice/details", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>用户阅读通知情况。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeRead.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/notice/read", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeRead.html")]
    public Task<IDictionary<String, Object>> GetNoticeReadAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/notice/read", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>发送通知。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeSend.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/notice/send", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeSend.html")]
    public Task<IDictionary<String, Object>> SendNoticeAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/notice/send", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>查询充值记录。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/rechargeRecord.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/app/recharge-records", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/rechargeRecord.html")]
    public Task<IDictionary<String, Object>> GetRechargeRecordsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/app/recharge-records", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>查询用户支付状态。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/userState.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/app/user-state", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/userState.html")]
    public Task<IDictionary<String, Object>> GetUserPayStateAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/app/user-state", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>查询用户钱包账户。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/userWallet.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/app/user-accounts", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/userWallet.html")]
    public Task<IDictionary<String, Object>> GetUserAccountsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/app/user-accounts", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>应用代扣下单。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/apppappay.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/connect/oauth/apppay/pappay", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/apppappay.html")]
    public Task<IDictionary<String, Object>> AppPapPayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/connect/oauth/apppay/pappay", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>非实名支付下单。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/noRealNamePay.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/wxpay", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/noRealNamePay.html")]
    public Task<IDictionary<String, Object>> CreateAnonymousWxPayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/wxpay", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>查询应用支付订单。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/orderInfo.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/app/order-info", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/orderInfo.html")]
    public Task<IDictionary<String, Object>> GetAppOrderAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/app/order-info", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>服务商代扣。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/providerPappay.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/provider-pappay/pappay", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/providerPappay.html")]
    public Task<IDictionary<String, Object>> ProviderPapPayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/provider-pappay/pappay", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>实名支付下单。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/realNamePay.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/mppay", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/realNamePay.html")]
    public Task<IDictionary<String, Object>> CreateRealNamePayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/mppay", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>应用支付退款。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/refund.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/refund", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/refund.html")]
    public Task<IDictionary<String, Object>> RefundAppPayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/refund", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>查询商户列表。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/merchant/merchantList.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/merchant-list", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/merchant/merchantList.html")]
    public Task<IDictionary<String, Object>> GetMerchantListAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/merchant-list", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>付款码换用户信息。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/authCodeToUserinfo.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/authcode_to_userinfo", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/authCodeToUserinfo.html")]
    public Task<IDictionary<String, Object>> MixPayAuthCodeToUserAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/authcode_to_userinfo", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>人脸授权信息。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/faceAuthInfo.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/faceauthinfo", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/faceAuthInfo.html")]
    public Task<IDictionary<String, Object>> MixPayFaceAuthInfoAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/faceauthinfo", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>混合支付下单。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/mixPay.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/mixpay/pay", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/mixPay.html")]
    public Task<IDictionary<String, Object>> MixPayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/mixpay/pay", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>混合支付查单。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/orderQuery.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/mixpay/order_query", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/orderQuery.html")]
    public Task<IDictionary<String, Object>> MixPayQueryOrderAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/mixpay/order_query", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>混合支付退款。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/orderRefund.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/mixpay/refund", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/orderRefund.html")]
    public Task<IDictionary<String, Object>> MixPayRefundAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/mixpay/refund", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>混合支付退款查询。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/refundQuery.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/mixpay/refund_query", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/refundQuery.html")]
    public Task<IDictionary<String, Object>> MixPayQueryRefundAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/mixpay/refund_query", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>按应用查充值余额。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/appRecharge.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/get-recharge-balance-by-appid", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/appRecharge.html")]
    public Task<IDictionary<String, Object>> GetRechargeBalanceByAppAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/get-recharge-balance-by-appid", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>按应用查补助余额。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/appSub.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/get-sub-balance-by-appid", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/appSub.html")]
    public Task<IDictionary<String, Object>> GetSubsidyBalanceByAppAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/get-sub-balance-by-appid", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>充值流水。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceFlow.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/recharge-records", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceFlow.html")]
    public Task<IDictionary<String, Object>> GetProviderRechargeRecordsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/recharge-records", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>余额账户余额。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceLeft.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/recharge/balance", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceLeft.html")]
    public Task<IDictionary<String, Object>> GetRechargeBalanceAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/recharge/balance", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>余额充值。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceRecharge.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/recharge/increase", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceRecharge.html")]
    public Task<IDictionary<String, Object>> IncreaseRechargeBalanceAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/recharge/increase", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>余额扣减。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceReduce.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/recharge/reduce", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceReduce.html")]
    public Task<IDictionary<String, Object>> ReduceRechargeBalanceAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/recharge/reduce", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>新充值流水。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/newBalanceFlow.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/recharge-flow", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/newBalanceFlow.html")]
    public Task<IDictionary<String, Object>> GetRechargeFlowAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/recharge-flow", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>新补助流水。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/newSubsidyFlow.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/wallet-flow", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/newSubsidyFlow.html")]
    public Task<IDictionary<String, Object>> GetWalletFlowAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/wallet-flow", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>按组织查充值余额。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/orgRecharge.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/get-recharge-balance-by-org", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/orgRecharge.html")]
    public Task<IDictionary<String, Object>> GetRechargeBalanceByOrgAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/get-recharge-balance-by-org", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>按组织查补助余额。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/orgSub.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/get-sub-balance-by-org", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/orgSub.html")]
    public Task<IDictionary<String, Object>> GetSubsidyBalanceByOrgAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/get-sub-balance-by-org", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>接收余额流水推送。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/receiveFlowPush.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/recharge/push-flows", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/receiveFlowPush.html")]
    public Task<IDictionary<String, Object>> PushRechargeFlowsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/recharge/push-flows", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>补助流水。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyFlow.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/wallet-records", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyFlow.html")]
    public Task<IDictionary<String, Object>> GetWalletRecordsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/wallet-records", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>发放补助。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyGrant.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/recharge/wallet/increase", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyGrant.html")]
    public Task<IDictionary<String, Object>> IncreaseWalletAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/recharge/wallet/increase", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>查询补助发放结果。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyGrantQuery.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/recharge/wallet/send/result", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyGrantQuery.html")]
    public Task<IDictionary<String, Object>> QueryWalletGrantAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/recharge/wallet/send/result", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>补助余额。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyLeft.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/recharge/wallet/balance", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyLeft.html")]
    public Task<IDictionary<String, Object>> GetWalletBalanceAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/recharge/wallet/balance", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>扣减补助。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyReduce.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/recharge/wallet/reduce", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyReduce.html")]
    public Task<IDictionary<String, Object>> ReduceWalletAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/recharge/wallet/reduce", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>添加优惠券接收人。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/addReceiver.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/voucher/add-receiver", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/addReceiver.html")]
    public Task<IDictionary<String, Object>> AddVoucherReceiverAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/voucher/add-receiver", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>向用户发券。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/addReceiverRepeat.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/voucher/present-coupon-for-uid", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/addReceiverRepeat.html")]
    public Task<IDictionary<String, Object>> PresentVoucherAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/voucher/present-coupon-for-uid", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>创建优惠券。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/createVoucher.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/voucher/create", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/createVoucher.html")]
    public Task<IDictionary<String, Object>> CreateVoucherAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/voucher/create", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>优惠券领取记录。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/receiverRecord.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/voucher/receiver-records", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/receiverRecord.html")]
    public Task<IDictionary<String, Object>> GetVoucherReceiverRecordsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/voucher/receiver-records", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>移除优惠券接收人。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/removeReceiver.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/voucher/remove-receiver", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/removeReceiver.html")]
    public Task<IDictionary<String, Object>> RemoveVoucherReceiverAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/voucher/remove-receiver", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>查询优惠券发放。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/voucherGrantQuery.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/pay/provider/voucher/query", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/voucherGrantQuery.html")]
    public Task<IDictionary<String, Object>> QueryVoucherGrantAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/pay/provider/voucher/query", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>支付订单详情。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/payDetails.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/provider/v4/order-detail", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/payDetails.html")]
    public Task<IDictionary<String, Object>> GetPayOrderDetailAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/provider/v4/order-detail", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>计划扣费添加成员。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/addPlanUser.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/app-add-plan-member", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/addPlanUser.html")]
    public Task<IDictionary<String, Object>> AddPlanMemberAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/app-add-plan-member", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>计划扣费批量解约。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/batchUnsubscribe.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/app-batch-unsubscribe-plan", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/batchUnsubscribe.html")]
    public Task<IDictionary<String, Object>> BatchUnsubscribePlanAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/app-batch-unsubscribe-plan", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>创建扣费计划。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/createPlan.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/app-create-plan", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/createPlan.html")]
    public Task<IDictionary<String, Object>> CreatePayPlanAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/app-create-plan", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>计划立即扣费。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/immediatelyDebit.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/app-immediately-debit", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/immediatelyDebit.html")]
    public Task<IDictionary<String, Object>> DebitPlanImmediatelyAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/app-immediately-debit", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>开通计划扣费能力。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/openAbility.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/app-pay-open", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/openAbility.html")]
    public Task<IDictionary<String, Object>> OpenPlanPayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/app-pay-open", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>计划扣费退款。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/orderRefund.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/app-pay-refund", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/orderRefund.html")]
    public Task<IDictionary<String, Object>> RefundPlanPayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/app-pay-refund", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>计划扣费单笔解约。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/singleUnsubscribe.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/pay/app/app-unsubscribe-plan", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/singleUnsubscribe.html")]
    public Task<IDictionary<String, Object>> UnsubscribePlanAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/pay/app/app-unsubscribe-plan", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>教育续费通用户状态。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/app-edu-user-status.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("GET", "/cgi-bin/user-pay/app-edu-user-status", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/app-edu-user-status.html")]
    public Task<IDictionary<String, Object>> GetEduUserPayStatusAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("GET", WeCardHost.Open, "/cgi-bin/user-pay/app-edu-user-status", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>教育续费通预签约。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/edu-presign.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/user-pay/edu-presign", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/edu-presign.html")]
    public Task<IDictionary<String, Object>> EduPresignAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/user-pay/edu-presign", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>代扣查单。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/orderQuery.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/v2/orderquery", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/orderQuery.html")]
    public Task<IDictionary<String, Object>> WithholdQueryOrderAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/v2/orderquery", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>代扣退款。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/orderRefund.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/v2/refund", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/orderRefund.html")]
    public Task<IDictionary<String, Object>> WithholdRefundAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/v2/refund", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>代扣下单。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/papPay.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/v2/pappay", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/papPay.html")]
    public Task<IDictionary<String, Object>> WithholdPapPayAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/v2/pappay", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>代扣退款查询。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/refundQuery.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/v2/refundquery", WeCardHost.PayApi, WeCardAuth.RawBody, "https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/refundQuery.html")]
    public Task<IDictionary<String, Object>> WithholdQueryRefundAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.PayApi, "/v2/refundquery", body, ocode, WeCardAuth.RawBody, cancellationToken);

    /// <summary>新建门禁场景组。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/add_area.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/device/guard/add-area", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/add_area.html")]
    public Task<IDictionary<String, Object>> AddGuardAreaAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/device/guard/add-area", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>新建闸机。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/add_gate.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/device/guard/add-gate", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/add_gate.html")]
    public Task<IDictionary<String, Object>> AddGuardGateAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/device/guard/add-gate", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>删除门禁场景组。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/delete_area.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/device/guard/delete-area", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/delete_area.html")]
    public Task<IDictionary<String, Object>> DeleteGuardAreaAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/device/guard/delete-area", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>删除闸机。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/delete_gate.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/device/guard/delete-gate", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/delete_gate.html")]
    public Task<IDictionary<String, Object>> DeleteGuardGateAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/device/guard/delete-gate", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>闸机上报。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/gate_report.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/device/guard/gate-report", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/gate_report.html")]
    public Task<IDictionary<String, Object>> ReportGuardGateAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/device/guard/gate-report", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>查询门禁场景组。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/get_area.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/device/guard/get-area", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/get_area.html")]
    public Task<IDictionary<String, Object>> GetGuardAreaAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/device/guard/get-area", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>更新门禁场景组。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/update_area.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/device/guard/update-area", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/update_area.html")]
    public Task<IDictionary<String, Object>> UpdateGuardAreaAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/device/guard/update-area", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>更新闸机。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/update_gate.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/device/guard/update-gate", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/update_gate.html")]
    public Task<IDictionary<String, Object>> UpdateGuardGateAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/device/guard/update-gate", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>上报门禁点位。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/position.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/guard/position", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/position.html")]
    public Task<IDictionary<String, Object>> ReportGuardPositionAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/guard/position", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>查询通行记录。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/guard/record_list.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/cgi-bin/guard/record-list", WeCardHost.Open, WeCardAuth.AccessTokenBody, "https://wiki.weixiao.qq.com/api/selfbuilt/guard/record_list.html")]
    public Task<IDictionary<String, Object>> GetGuardRecordsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/cgi-bin/guard/record-list", body, ocode, WeCardAuth.AccessTokenBody, cancellationToken);

    /// <summary>拉取访客预约记录。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/visitor/getList.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/app-visit/open/get-appt-list", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/visitor/getList.html")]
    public Task<IDictionary<String, Object>> GetVisitAppointmentsAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/app-visit/open/get-appt-list", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>按车牌查询来访。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/visitor/queryByPlateNumber.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/app-visit/open/check-exists-by-plate-number", WeCardHost.Open, WeCardAuth.AccessTokenQuery, "https://wiki.weixiao.qq.com/api/selfbuilt/visitor/queryByPlateNumber.html")]
    public Task<IDictionary<String, Object>> QueryVisitByPlateAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Open, "/app-visit/open/check-exists-by-plate-number", body, ocode, WeCardAuth.AccessTokenQuery, cancellationToken);

    /// <summary>获取用户待缴信息。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/payment_system/getUserArrear.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/api/open/project/getUserArrear", WeCardHost.Arrear, WeCardAuth.SignedBody, "https://wiki.weixiao.qq.com/api/selfbuilt/payment_system/getUserArrear.html")]
    public Task<IDictionary<String, Object>> GetUserArrearAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Arrear, "/api/open/project/getUserArrear", body, ocode, WeCardAuth.SignedBody, cancellationToken);

    /// <summary>同步应缴项目。支付与门禁仅为薄封装，不写业务表、不默认开通。文档 https://wiki.weixiao.qq.com/api/selfbuilt/payment_system/syncArrears.html</summary>
    /// <param name="body">请求字段，名称与官方文档一致。空则只携带凭证</param>
    /// <param name="ocode">主体代码。空则使用 Options.Ocode。签名类与机具类接口忽略该参数</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>官方响应字典。code 或 errcode 非成功时抛出 WeCardException</returns>
    [WeCardApi("POST", "/api/open/project/syncUserArrear", WeCardHost.Arrear, WeCardAuth.SignedBody, "https://wiki.weixiao.qq.com/api/selfbuilt/payment_system/syncArrears.html")]
    public Task<IDictionary<String, Object>> SyncUserArrearAsync(Object body = null, String ocode = null, CancellationToken cancellationToken = default)
        => SendAsync("POST", WeCardHost.Arrear, "/api/open/project/syncUserArrear", body, ocode, WeCardAuth.SignedBody, cancellationToken);

}
