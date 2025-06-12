# GHLearning-EasyApiKey
[![.NET](https://github.com/gordon-hung/GHLearning-EasyApiKey/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gordon-hung/GHLearning-EasyApiKey/actions/workflows/dotnet.yml) 
[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/gordon-hung/GHLearning-EasyApiKey)
[![codecov](https://codecov.io/gh/gordon-hung/GHLearning-EasyApiKey/graph/badge.svg?token=UCJmfOS6nI)](https://codecov.io/gh/gordon-hung/GHLearning-EasyApiKey)
## 專案架構概覽

### 1.	Core 專案
- 定義核心介面與實體，例如 IApiKeyRepository 介面與 ApiKeyEntity。
- ApiKeyEntity 包含 Code、Name、Description、Secret 等欄位，代表一組 API 金鑰資訊。

### 2.	Infrastructure 專案
- 提供 ApiKey 的實作與授權邏輯。
- ApiKeyRepository 依據設定的 ApiKeyOptions 進行金鑰驗證。
- ApiKeyAuthorizationHandler 實作授權邏輯，從 HTTP Header 取得 ApiKey 並驗證。
- ApiKeyRequirement 作為授權需求標記。

### 3.	SharedKernel 專案
- 定義共用常數，例如 HttpHeaderConsts.ApiKeyHeaderName（"X-API-KEY"），統一 API 金鑰的 HTTP Header 名稱。

### 4.	WebApi 專案
- 實際 Web API 入口，會註冊授權服務與中介軟體，並於 Controller 層驗證 ApiKey。

### 5.	測試專案
- 針對授權流程與金鑰驗證進行單元測試與整合測試。

## ApiKey 運用流程
### 1.	設定 ApiKey
- 於設定檔或程式碼中，將多組 ApiKeyEntity 設定到 ApiKeyOptions.ApiKeys。

### 2.	請求驗證
- Client 於 HTTP Header 加入 X-API-KEY（或 X-Api-Key，不分大小寫）欄位，填入金鑰。

### 3.	授權流程
- ApiKeyAuthorizationHandler 會攔截請求，讀取 Header 的 ApiKey。
- 呼叫 IApiKeyRepository.ValidationAsync 驗證金鑰是否存在於設定清單。
- 驗證通過則授權成功，否則回傳 401 Unauthorized。

### 4.	測試驗證
- 測試專案會模擬不同情境（正確金鑰、缺少金鑰、錯誤金鑰）來驗證授權行為。

## 主要程式片段
```
// 取得 Header
if (httpContext.Request.Headers.TryGetValue(HttpHeaderConsts.ApiKeyHeaderName, out var extractedApiKey))
{
    if (await apiKeyRepository.ValidationAsync(extractedApiKey!))
    {
        context.Succeed(requirement);
    }
}
```

## 小結
此專案以分層架構實作 ApiKey 授權，將金鑰驗證、授權邏輯與設定分離，易於擴充與測試。ApiKey 主要透過 HTTP Header 傳遞，並於授權處理器中驗證。
如需更詳細的註冊與設定方式，可進一步查閱 WebApi 專案的啟動程式碼（如 Program.cs 或 Startup.cs）。