# NewLife.Cube.WeCard

腾讯微卡**自建应用**扩展 for [NewLife.Cube 魔方](https://github.com/NewLifeX/NewLife.Cube)。任意 Cube 宿主引用对应包并调用 `AddWeCard` / `UseWeCard` 后，即可调用开放接口、接收事件回调。不改魔方框架源码。用户、角色、权限和审计仍用魔方。

**本库只覆盖自建应用** [selfbuilt 文档树](https://wiki.weixiao.qq.com/api/selfbuilt/guides/epitome.html)。第三方应用（`/api/third*`）与服务商代开发不在范围内。

支付、门禁、来访、缴费是薄封装：只有客户端方法和回调挂钩，不建业务表，不默认开通支付。

## 安装

同一份源码打两个包，程序集名都是 `NewLife.Cube.WeCard`，公开 API 相同。`NewLife.Cube` 与 `NewLife.Cube.Core` 的程序集名都是 `NewLife.Cube`，一个宿主只能装其中一组。

| 宿主 | 魔方包 | 本库 |
|---|---|---|
| MVC（Razor，工程 `NewLife.CubeNC`） | `NewLife.Cube.Core` | `NewLife.Cube.WeCard` |
| WebApi（工程 `NewLife.Cube`） | `NewLife.Cube` | `NewLife.Cube.WeCard.WebApi` |

```bash
# MVC 宿主
dotnet add package NewLife.Cube.Core
dotnet add package NewLife.Cube.WeCard

# WebApi 宿主
dotnet add package NewLife.Cube
dotnet add package NewLife.Cube.WeCard.WebApi
```

本仓库尚未发布到 nuget.org。需要本地包时：

```bash
dotnet pack src/NewLife.Cube.WeCard/NewLife.Cube.WeCard.csproj -c Release -p:CubeHost=Mvc
dotnet pack src/NewLife.Cube.WeCard/NewLife.Cube.WeCard.csproj -c Release -p:CubeHost=WebApi
```

回调控制器继承 `ControllerBase`。Web SDK 宿主直接引用本包并调用 `AddControllers()` 时，会写入 `ApplicationPartAttribute`，端点随常规启动进入路由。若只有类库引用、Web 工程的引用闭包里看不到本程序集，再补：

```csharp
builder.Services.AddControllers()
    .AddApplicationPart(typeof(NewLife.Cube.WeCard.Controllers.WeCardCallbackController).Assembly);
```

## 文档

- [需求文档](Doc/需求文档.md)
- [功能清单](Doc/功能清单.md)
- [架构设计](Doc/架构设计.md)
- 官方入口：[开放平台概述（自建）](https://wiki.weixiao.qq.com/api/selfbuilt/guides/epitome.html)

## 配置

公有云开放平台是 `https://open.wecard.qq.com`。私有化把 `BaseUrl` 设成「私有化域名 + /kong」，路径与公有云相同。机具支付、电子码、缴费大厅的域名不同，私有化时分别改 `PayApiBaseUrl`、`SchoolApiBaseUrl`、`ArrearBaseUrl`。

```csharp
builder.Services.AddControllers();
builder.Services.Configure<WeCardOptions>(builder.Configuration.GetSection("WeCard"));
builder.Services.AddWeCard();

var app = builder.Build();
app.UseWeCard();
app.MapControllers();
```

```json
{
  "WeCard": {
    "AppKey": "",
    "AppSecret": "",
    "Ocode": "",
    "BaseUrl": "https://open.wecard.qq.com",
    "NotifyKey": "",
    "EnableCallback": true
  }
}
```

`AppSecret`、`NotifyKey` 不要提交到仓库。也可以不走配置节，在 `AddWeCard(o => { ... })` 里赋值。显式参数优先于魔方 OAuth 配置（`Provider=WeCard`，AppId 填 AppKey，Secret 填 AppSecret）。

```csharp
var (client, source) = WeCardCredential.Resolve(new WeCardOptions
{
    AppKey = "应用AppKey",
    AppSecret = "应用AppSecret",
    Ocode = "主体代码",
});
var token = await client.GetAccessTokenAsync();
var users = await client.GetOrgUsersAsync(new Dictionary<string, object>
{
    ["org_id"] = 0,
    ["fetch_child"] = 1,
    ["page"] = 1,
    ["page_size"] = 100,
});
```

`NotifyKey` 只在接入「实时数据推送」时填写。留空则 `/WeCard/PayNotify` 返回 FAIL，不会解密支付通知。

## 回调 URL

在微卡后台把公网地址配成下面的路径。未填写 AppKey/AppSecret，或 `EnableCallback=false` 时，这些端点返回 404。

| 路径 | 用途 | 成功应答 |
|---|---|---|
| `https://域名/WeCard/Callback` | 成员、组织、应用可用范围事件。`raw_data` 为 AES-128-CBC ZeroPadding 十六进制，KEY=AppKey，IV=AppSecret 前 16 位 | `{"code":0,"message":"ok"}` |
| `https://域名/WeCard/PayNotify` | 交易流水 / 设备心跳。AEAD_AES_256_GCM，密钥为 NotifyKey | `{"code":"SUCCESS","message":"ok"}` |
| `https://域名/WeCard/PayCallback` | 应用支付回调，表单或 JSON，必须带 signature | `{"code":0,"message":"ok"}` |
| `https://域名/WeCard/GuardNotify` | 门禁通行通知 | `{"result":0,"msg":"ok"}` |
| `https://域名/WeCard/Arrear` | 缴费大厅向学校查询待缴（入站） | `{"code":0,"message":"ok","data":...}` |
| `https://域名/WeCard/ArrearNotify` | 缴费结果通知（入站） | `{"code":0,"message":"ok"}` |
| `https://域名/WeCard/UserSync` | 每月全量用户同步（学校提供的入站地址） | `app_key` + 加密 `raw_data` |

```csharp
WeCardEvents.Handlers.Add(new MyHandler());
```

也可以把 `IWeCardEventHandler` 注册进依赖注入。需要回写待缴或全量用户时实现 `IWeCardInboundHandler`。

OAuth 不替换魔方登录。H5 / PC 授权 URL、管理员 `manager-connect`、wxcode 换票和取用户信息在 `WeCardOAuth`。小程序插件（provider `wx1549b963bbae9931`，文档版本 4.0.1）跑在小程序里；服务端换票时 `redirect_uri` 用 `mnp://` + 小程序 AppId。

## 接口对照

官方算法：[签名](https://wiki.weixiao.qq.com/api/selfbuilt/tools/signAlgorithm.html)、[加解密](https://wiki.weixiao.qq.com/api/selfbuilt/tools/crypt.html)、[应用凭证](https://wiki.weixiao.qq.com/api/selfbuilt/guides/accessToken.html)、[支付对称加密](https://wiki.weixiao.qq.com/api/selfbuilt/pay/crypt/symmetricEncryption.html)、[支付非对称加密](https://wiki.weixiao.qq.com/api/selfbuilt/pay/crypt/asymmetricEncryption.html)。

应用 access_token：`GetAccessTokenAsync`，`POST /cgi-bin/oauth2/token`。wxcode：`WeCardOAuth.ExchangeCodeAsync`，`POST /connect/oauth2/token`。用户信息：`GetUserInfoAsync`，`POST /connect/oauth/get-user-info`。

| 能力 | 方法 | 文档 |
|---|---|---|
| 获取主体基本信息 | `GetSchoolInfoAsync` `POST /cgi-bin/campus/school-info` | [mainpartInfo.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/mainpartInfo.html) |
| 获取组织架构类型 | `GetOrgTypesAsync` `POST /cgi-bin/org/types` | [orgTypes.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/orgTypes.html) |
| 获取组织架构 | `GetOrgsAsync` `POST /cgi-bin/user/org-edu-list` | [orgs.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/orgs.html) |
| 按 id 获取组织架构 | `GetOrgByIdsAsync` `POST /cgi-bin/org/get-org-by-ids` | [getOrgByIds.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/getOrgByIds.html) |
| 获取组织架构成员 | `GetOrgUsersAsync` `POST /cgi-bin/user/get-org-users` | [orgUsers.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/orgUsers.html) |
| 按电子卡号获取成员 | `GetUsersByCardNumbersAsync` `POST /cgi-bin/user/get-user-by-card-numbers` | [getUserByCardNumber.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUserByCardNumber.html) |
| 搜索成员 | `SearchUsersAsync` `POST /cgi-bin/user/search` | [searchUser.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/searchUser.html) |
| 拉取成员变更记录 | `GetUpdatedUsersAsync` `POST /cgi-bin/user/get-update-records` | [getUpdateUser.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUpdateUser.html) |
| 获取成员身份类型 | `GetUserIdentityTypeAsync` `POST /cgi-bin/user/get-identity-type` | [userIdentityType.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/userIdentityType.html) |
| 获取成员绑卡状态 | `GetUserBindStatusAsync` `POST /cgi-bin/user/get-user-bind-card-status` | [getUserBindStatus.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUserBindStatus.html) |
| 按卡号获取企业微信 userid | `GetWxWorkUserIdAsync` `POST /cgi-bin/user/wxwork/get-user-id` | [getUidByCardNumber.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/getUidByCardNumber.html) |
| 即时创建或更新用户 | `CreateUserAsync` `POST /cgi-bin/user/manage/create` | [userCreateNow.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/userCreateNow.html) |
| 批量删除用户 | `BatchDeleteUsersAsync` `POST /cgi-bin/user/manage/batch-delete` | [batchDeleteUser.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/batchDeleteUser.html) |
| 变更 NFC | `ChangeNfcAsync` `POST /cgi-bin/user/change-nfc` | [changeNfc.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/changeNfc.html) |
| 移除 NFC | `RemoveNfcAsync` `POST /cgi-bin/user/remove-nfc` | [removeNfc.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/removeNfc.html) |
| 重置 NFC 卡面 | `ResetNfcFaceAsync` `POST /cgi-bin/user/reset-nfc-face` | [resetNfcCardFace.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/resetNfcCardFace.html) |
| 主动推送学生信息 | `PushStudentInfoAsync` `POST /cgi-bin/user/manage/sync-user-info` | [pushStuInfo.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/pushStuInfo.html) |
| 按 id 获取标签 | `GetTagByIdAsync` `POST /cgi-bin/user/get-tag-by-id` | [getTagById.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/tag/getTagById.html) |
| 新增角色组 | `AddRoleGroupAsync` `POST /cgi-bin/user/role/add-custom-role-group` | [addRoleGroups.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/addRoleGroups.html) |
| 新增角色 | `AddRoleAsync` `POST /cgi-bin/user/role/add-custom-role` | [addRoles.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/addRoles.html) |
| 角色批量添加成员 | `BatchAddRoleMembersAsync` `POST /cgi-bin/user/role/add-members` | [batchAddRoles.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/batchAddRoles.html) |
| 角色批量移除成员 | `BatchDeleteRoleMembersAsync` `POST /cgi-bin/user/role/del-members` | [batchDelRoles.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/batchDelRoles.html) |
| 删除角色组 | `DeleteRoleGroupAsync` `POST /cgi-bin/user/role/del-custom-role-group` | [delRoleGroups.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/delRoleGroups.html) |
| 删除角色 | `DeleteRoleAsync` `POST /cgi-bin/user/role/del-custom-role` | [delRoles.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/delRoles.html) |
| 获取角色组 | `GetRoleGroupsAsync` `GET /cgi-bin/role/groups` | [roleGroups.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/roleGroups.html) |
| 获取角色 | `GetRolesAsync` `GET /cgi-bin/role/roles` | [roles.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/role/roles.html) |
| 获取权限组列表 | `GetPermissionGroupsAsync` `POST /cgi-bin/permission/group-list` | [permissionGroups.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/permissionGroups.html) |
| 获取权限组详情 | `GetPermissionGroupAsync` `POST /cgi-bin/permission/group` | [permissionGroupDetail.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/permissionGroupDetail.html) |
| 获取用户权限 | `GetUserPermissionAsync` `POST /cgi-bin/permission/user-permission` | [userPermission.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/permission/userPermission.html) |
| 同步物理卡 | `SyncPhysicalCardAsync` `POST /cgi-bin/physical-card/sync` | [physical-card-sync.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/physical-card-sync.html) |
| 更新物理卡 | `UpdatePhysicalCardAsync` `POST /cgi-bin/physical-card/update` | [physical-card-update.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/physical-card-update.html) |
| 解绑物理卡 | `RelievePhysicalCardAsync` `POST /cgi-bin/user/manage/relieve-card` | [relieve-card.html](https://wiki.weixiao.qq.com/api/selfbuilt/contact/physical_card/relieve-card.html) |
| 电子卡跳转小程序获取成员信息 | `GetMiniProgramStudentAsync` `POST /apps/school-api/mnp-student` | [cardMnpStu.html](https://wiki.weixiao.qq.com/api/selfbuilt/e_card/cardMnpStu.html) |
| 小程序添加电子卡 | `AddStudentCardAsync` `GET /apps/school-api/add-student-card` | [stuMnpAddCard.html](https://wiki.weixiao.qq.com/api/selfbuilt/e_card/stuMnpAddCard.html) |
| 小程序打开用户电子卡 | `GetStudentCardsAsync` `GET /apps/school-api/student-cards` | [stuMnpCardInfo.html](https://wiki.weixiao.qq.com/api/selfbuilt/e_card/stuMnpCardInfo.html) |
| 电子码在线解码 | `DecodeCampusCodeAsync` `POST /apps/school-api/campus-code` | [campuscode.html](https://wiki.weixiao.qq.com/api/selfbuilt/e_code/campuscode.html) |
| 电子码扫码事件通知 | `NotifyCampusScanAsync` `POST /apps/school-api/scan-notify` | [scanNotify.html](https://wiki.weixiao.qq.com/api/selfbuilt/e_code/scanNotify.html) |
| 用小程序 openid 换微卡身份 | `GetMiniProgramIdentityAsync` `POST /apps/wxmnp/plugin/student-before-plugin` | [mnpPlugin.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/mnpPlugin.html) |
| 查询通知能力 | `GetNoticeAbilityAsync` `POST /cgi-bin/notice/ability` | [noticeAble.html](https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeAble.html) |
| 通知数据明细 | `GetNoticeDetailAsync` `POST /cgi-bin/notice/details` | [noticeDetail.html](https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeDetail.html) |
| 用户阅读通知情况 | `GetNoticeReadAsync` `POST /cgi-bin/notice/read` | [noticeRead.html](https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeRead.html) |
| 发送通知 | `SendNoticeAsync` `POST /cgi-bin/notice/send` | [noticeSend.html](https://wiki.weixiao.qq.com/api/selfbuilt/announcement/noticeSend.html) |
| 查询充值记录 | `GetRechargeRecordsAsync` `GET /cgi-bin/pay/app/recharge-records` | [rechargeRecord.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/rechargeRecord.html) |
| 查询用户支付状态 | `GetUserPayStateAsync` `GET /cgi-bin/pay/app/user-state` | [userState.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/account/userState.html) |
| 查询用户钱包账户 | `GetUserAccountsAsync` `GET /cgi-bin/pay/app/user-accounts` | [userWallet.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/userWallet.html) |
| 应用代扣下单 | `AppPapPayAsync` `POST /connect/oauth/apppay/pappay` | [apppappay.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/apppappay.html) |
| 非实名支付下单 | `CreateAnonymousWxPayAsync` `POST /cgi-bin/pay/app/wxpay` | [noRealNamePay.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/noRealNamePay.html) |
| 查询应用支付订单 | `GetAppOrderAsync` `GET /cgi-bin/pay/app/order-info` | [orderInfo.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/orderInfo.html) |
| 服务商代扣 | `ProviderPapPayAsync` `POST /cgi-bin/provider-pappay/pappay` | [providerPappay.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/providerPappay.html) |
| 实名支付下单 | `CreateRealNamePayAsync` `POST /cgi-bin/pay/app/mppay` | [realNamePay.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/realNamePay.html) |
| 应用支付退款 | `RefundAppPayAsync` `POST /cgi-bin/pay/app/refund` | [refund.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/refund.html) |
| 查询商户列表 | `GetMerchantListAsync` `GET /cgi-bin/pay/provider/merchant-list` | [merchantList.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/merchant/merchantList.html) |
| 付款码换用户信息 | `MixPayAuthCodeToUserAsync` `POST /authcode_to_userinfo` | [authCodeToUserinfo.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/authCodeToUserinfo.html) |
| 人脸授权信息 | `MixPayFaceAuthInfoAsync` `POST /faceauthinfo` | [faceAuthInfo.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/faceAuthInfo.html) |
| 混合支付下单 | `MixPayAsync` `POST /mixpay/pay` | [mixPay.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/mixPay.html) |
| 混合支付查单 | `MixPayQueryOrderAsync` `POST /mixpay/order_query` | [orderQuery.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/orderQuery.html) |
| 混合支付退款 | `MixPayRefundAsync` `POST /mixpay/refund` | [orderRefund.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/orderRefund.html) |
| 混合支付退款查询 | `MixPayQueryRefundAsync` `POST /mixpay/refund_query` | [refundQuery.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/mixpay/refundQuery.html) |
| 按应用查充值余额 | `GetRechargeBalanceByAppAsync` `GET /cgi-bin/pay/provider/get-recharge-balance-by-appid` | [appRecharge.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/appRecharge.html) |
| 按应用查补助余额 | `GetSubsidyBalanceByAppAsync` `GET /cgi-bin/pay/provider/get-sub-balance-by-appid` | [appSub.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/appSub.html) |
| 充值流水 | `GetProviderRechargeRecordsAsync` `GET /cgi-bin/pay/provider/recharge-records` | [balanceFlow.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceFlow.html) |
| 余额账户余额 | `GetRechargeBalanceAsync` `GET /cgi-bin/pay/provider/recharge/balance` | [balanceLeft.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceLeft.html) |
| 余额充值 | `IncreaseRechargeBalanceAsync` `POST /cgi-bin/pay/provider/recharge/increase` | [balanceRecharge.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceRecharge.html) |
| 余额扣减 | `ReduceRechargeBalanceAsync` `POST /cgi-bin/pay/provider/recharge/reduce` | [balanceReduce.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/balanceReduce.html) |
| 新充值流水 | `GetRechargeFlowAsync` `GET /cgi-bin/pay/provider/recharge-flow` | [newBalanceFlow.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/newBalanceFlow.html) |
| 新补助流水 | `GetWalletFlowAsync` `GET /cgi-bin/pay/provider/wallet-flow` | [newSubsidyFlow.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/newSubsidyFlow.html) |
| 按组织查充值余额 | `GetRechargeBalanceByOrgAsync` `GET /cgi-bin/pay/provider/get-recharge-balance-by-org` | [orgRecharge.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/orgRecharge.html) |
| 按组织查补助余额 | `GetSubsidyBalanceByOrgAsync` `GET /cgi-bin/pay/provider/get-sub-balance-by-org` | [orgSub.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/orgSub.html) |
| 接收余额流水推送 | `PushRechargeFlowsAsync` `POST /cgi-bin/pay/provider/recharge/push-flows` | [receiveFlowPush.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/receiveFlowPush.html) |
| 补助流水 | `GetWalletRecordsAsync` `GET /cgi-bin/pay/provider/wallet-records` | [subsidyFlow.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyFlow.html) |
| 发放补助 | `IncreaseWalletAsync` `POST /cgi-bin/pay/provider/recharge/wallet/increase` | [subsidyGrant.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyGrant.html) |
| 查询补助发放结果 | `QueryWalletGrantAsync` `GET /cgi-bin/pay/provider/recharge/wallet/send/result` | [subsidyGrantQuery.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyGrantQuery.html) |
| 补助余额 | `GetWalletBalanceAsync` `GET /cgi-bin/pay/provider/recharge/wallet/balance` | [subsidyLeft.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyLeft.html) |
| 扣减补助 | `ReduceWalletAsync` `POST /cgi-bin/pay/provider/recharge/wallet/reduce` | [subsidyReduce.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openrecharge/subsidyReduce.html) |
| 添加优惠券接收人 | `AddVoucherReceiverAsync` `POST /cgi-bin/pay/provider/voucher/add-receiver` | [addReceiver.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/addReceiver.html) |
| 向用户发券 | `PresentVoucherAsync` `POST /cgi-bin/pay/provider/voucher/present-coupon-for-uid` | [addReceiverRepeat.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/addReceiverRepeat.html) |
| 创建优惠券 | `CreateVoucherAsync` `POST /cgi-bin/pay/provider/voucher/create` | [createVoucher.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/createVoucher.html) |
| 优惠券领取记录 | `GetVoucherReceiverRecordsAsync` `GET /cgi-bin/pay/provider/voucher/receiver-records` | [receiverRecord.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/receiverRecord.html) |
| 移除优惠券接收人 | `RemoveVoucherReceiverAsync` `POST /cgi-bin/pay/provider/voucher/remove-receiver` | [removeReceiver.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/removeReceiver.html) |
| 查询优惠券发放 | `QueryVoucherGrantAsync` `GET /cgi-bin/pay/provider/voucher/query` | [voucherGrantQuery.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/openvoucher/voucherGrantQuery.html) |
| 支付订单详情 | `GetPayOrderDetailAsync` `POST /cgi-bin/pay/provider/v4/order-detail` | [payDetails.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/payDetails.html) |
| 计划扣费添加成员 | `AddPlanMemberAsync` `POST /cgi-bin/pay/app/app-add-plan-member` | [addPlanUser.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/addPlanUser.html) |
| 计划扣费批量解约 | `BatchUnsubscribePlanAsync` `POST /cgi-bin/pay/app/app-batch-unsubscribe-plan` | [batchUnsubscribe.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/batchUnsubscribe.html) |
| 创建扣费计划 | `CreatePayPlanAsync` `POST /cgi-bin/pay/app/app-create-plan` | [createPlan.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/createPlan.html) |
| 计划立即扣费 | `DebitPlanImmediatelyAsync` `POST /cgi-bin/pay/app/app-immediately-debit` | [immediatelyDebit.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/immediatelyDebit.html) |
| 开通计划扣费能力 | `OpenPlanPayAsync` `POST /cgi-bin/pay/app/app-pay-open` | [openAbility.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/openAbility.html) |
| 计划扣费退款 | `RefundPlanPayAsync` `POST /cgi-bin/pay/app/app-pay-refund` | [orderRefund.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/orderRefund.html) |
| 计划扣费单笔解约 | `UnsubscribePlanAsync` `POST /cgi-bin/pay/app/app-unsubscribe-plan` | [singleUnsubscribe.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/planpay/singleUnsubscribe.html) |
| 教育续费通用户状态 | `GetEduUserPayStatusAsync` `GET /cgi-bin/user-pay/app-edu-user-status` | [app-edu-user-status.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/app-edu-user-status.html) |
| 教育续费通预签约 | `EduPresignAsync` `POST /cgi-bin/user-pay/edu-presign` | [edu-presign.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/edu-presign.html) |
| 代扣查单 | `WithholdQueryOrderAsync` `POST /v2/orderquery` | [orderQuery.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/orderQuery.html) |
| 代扣退款 | `WithholdRefundAsync` `POST /v2/refund` | [orderRefund.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/orderRefund.html) |
| 代扣下单 | `WithholdPapPayAsync` `POST /v2/pappay` | [papPay.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/papPay.html) |
| 代扣退款查询 | `WithholdQueryRefundAsync` `POST /v2/refundquery` | [refundQuery.html](https://wiki.weixiao.qq.com/api/selfbuilt/pay/withhold/refundQuery.html) |
| 新建门禁场景组 | `AddGuardAreaAsync` `POST /cgi-bin/device/guard/add-area` | [add_area.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/add_area.html) |
| 新建闸机 | `AddGuardGateAsync` `POST /cgi-bin/device/guard/add-gate` | [add_gate.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/add_gate.html) |
| 删除门禁场景组 | `DeleteGuardAreaAsync` `POST /cgi-bin/device/guard/delete-area` | [delete_area.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/delete_area.html) |
| 删除闸机 | `DeleteGuardGateAsync` `POST /cgi-bin/device/guard/delete-gate` | [delete_gate.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/delete_gate.html) |
| 闸机上报 | `ReportGuardGateAsync` `POST /cgi-bin/device/guard/gate-report` | [gate_report.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/gate_report.html) |
| 查询门禁场景组 | `GetGuardAreaAsync` `POST /cgi-bin/device/guard/get-area` | [get_area.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/get_area.html) |
| 更新门禁场景组 | `UpdateGuardAreaAsync` `POST /cgi-bin/device/guard/update-area` | [update_area.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/update_area.html) |
| 更新闸机 | `UpdateGuardGateAsync` `POST /cgi-bin/device/guard/update-gate` | [update_gate.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/update_gate.html) |
| 上报门禁点位 | `ReportGuardPositionAsync` `POST /cgi-bin/guard/position` | [position.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/position.html) |
| 查询通行记录 | `GetGuardRecordsAsync` `POST /cgi-bin/guard/record-list` | [record_list.html](https://wiki.weixiao.qq.com/api/selfbuilt/guard/record_list.html) |
| 拉取访客预约记录 | `GetVisitAppointmentsAsync` `POST /app-visit/open/get-appt-list` | [getList.html](https://wiki.weixiao.qq.com/api/selfbuilt/visitor/getList.html) |
| 按车牌查询来访 | `QueryVisitByPlateAsync` `POST /app-visit/open/check-exists-by-plate-number` | [queryByPlateNumber.html](https://wiki.weixiao.qq.com/api/selfbuilt/visitor/queryByPlateNumber.html) |
| 获取用户待缴信息 | `GetUserArrearAsync` `POST /api/open/project/getUserArrear` | [getUserArrear.html](https://wiki.weixiao.qq.com/api/selfbuilt/payment_system/getUserArrear.html) |
| 同步应缴项目 | `SyncUserArrearAsync` `POST /api/open/project/syncUserArrear` | [syncArrears.html](https://wiki.weixiao.qq.com/api/selfbuilt/payment_system/syncArrears.html) |


没有单独 HTTP 路径、由本库用回调或工具覆盖的页面：

| 页面 | 落点 |
|---|---|
| [事件总述](https://wiki.weixiao.qq.com/api/selfbuilt/guides/event.html)、[成员事件](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/userEvent.html)、[组织事件](https://wiki.weixiao.qq.com/api/selfbuilt/contact/org/orgEvent.html)、[可用范围](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/appUsableEvent.html) | `/WeCard/Callback` |
| [全量同步](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/syncStuInfo.html)、[同步概述](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/sycInformationSummary.html) | 主动推送 `PushStudentInfoAsync`；定时同步 `/WeCard/UserSync` |
| [身份字段矩阵](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/identityType.html)、[证件与国籍](https://wiki.weixiao.qq.com/api/selfbuilt/contact/member/country.html) | 说明页，无请求 URL |
| [H5](https://wiki.weixiao.qq.com/api/selfbuilt/authentication/h5_oauth.html) / [PC](https://wiki.weixiao.qq.com/api/selfbuilt/authentication/pc_oauth.html) / [小程序](https://wiki.weixiao.qq.com/api/selfbuilt/authentication/mnp_oauth.html) | `WeCardOAuth` |
| [管理员登录态](https://wiki.weixiao.qq.com/api/selfbuilt/guides/standard.html) | `BuildManagerConnectUrl` |
| [支付通知](https://wiki.weixiao.qq.com/api/selfbuilt/pay/notify.html)、[应用支付回调](https://wiki.weixiao.qq.com/api/selfbuilt/pay/apppay/payCallback.html) | `/WeCard/PayNotify`、`/WeCard/PayCallback` |
| [门禁通知](https://wiki.weixiao.qq.com/api/selfbuilt/guard/notify.html) | `/WeCard/GuardNotify` |
| [缴费大厅入站](https://wiki.weixiao.qq.com/api/selfbuilt/payment_system/payment_arrear.html) | `/WeCard/Arrear`、`/WeCard/ArrearNotify` |
| 代扣旧路径 `/pappay`、`/orderquery`、`/refund`、`/refundquery` | 文档写明停止维护，只封装 `/v2/*` |

## 明确不做

- 第三方应用与 `/api/third*`
- 运营后台皮肤、小程序前端工程、硬件驱动
- 支付清算对账产品和默认开通支付
- 修改 NewLife.Cube 源码

## 打包与测试

魔方依赖 `6.15.2026.901`（nuget.org 上该版本之后均为 beta），目标框架 `net8.0`。默认 `CubeHost=Mvc`。单测不访问 `open.wecard.qq.com`。

```bash
dotnet test tests/WeCard.Tests/WeCard.Tests.csproj -c Release -p:CubeHost=Mvc
dotnet test tests/WeCard.Tests/WeCard.Tests.csproj -c Release -p:CubeHost=WebApi
```

示例宿主 `samples/WeCard.Host` 演示注册。两种 CubeHost 都会还原对应魔方包，并确认回调控制器进入路由。
