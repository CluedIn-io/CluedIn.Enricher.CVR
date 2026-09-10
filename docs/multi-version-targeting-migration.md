# Migrating a Connector/Enricher to Multi-Version Targeting

This document tracks the migration of `CluedIn.Enricher.CVR` from a single-version build to the
multi-version targeting pattern. Modeled on the prior migrations of `CluedIn.Connector.Dataverse.V2`,
`CluedIn.Enricher.GoogleMaps`, `CluedIn.Enricher.Gleif`, `CluedIn.Enricher.OpenCorporates`, and
`CluedIn.Enricher.Brreg` (all done the same session, same pattern).

Branch: `feature/multi-version-targeting` (off `develop`).

---

## Overview

Target matrix: `4.7.0`, `4.8.0`, `5.0.0-beta.*` (net6.0/net6.0/net10.0, auto-detected by the
pipeline template). Verified independently for this repo's own feeds via a throwaway restore that
`5.0.0-*` currently resolves to `5.0.0-beta.576` — beta is the current prerelease channel.

4.6.0 excluded — no evidence this repo's small ExternalSearch provider surface needs it.

---

## Step 1 — Pipeline template (`azure-pipelines.yml`)

Status: **Done**

Switched from `crawler.build.yml` to `crawler.build.jobs.yml`. Flipped `runIntegrationTests`
default from `false` to `true` and dropped the `createIntegrationEnvironmentScriptFilePath`
references — `./build/integration-test.ps1` never existed in this repo (same finding as GoogleMaps'
migration), and the repo's one integration test project has no external environment dependency (its
two tests are both `[Fact(Skip = ...)]`, referencing a GitHub issue). Switched `windows-latest` →
`ubuntu-22.04` to match the shared template's expected environment.

---

## Step 2 — `Directory.Build.props`

Status: **Done**

Honours `CluedInMultiVersionTargetFramework` with `net10.0` local-dev fallback, derives
`CLUEDIN_V47`/`V48`/`V50` `DefineConstants`, pins `LangVersion` to 13.0 up front (several prior
repos hit `CS8936` on net6.0 without this).

---

## Step 3 — `Packages.props` and `NuGet.config`

Status: **Done**

- Renamed `Nuget.config` → `NuGet.config` (two-step `git mv`, matching prior repos' Linux
  case-sensitivity fix).
- `_CluedIn` guarded so the pipeline value wins.
- Test package versions split conditionally on `CLUEDIN_V50` (xunit v3/AutoFixture.Xunit3 vs
  xunit v2/AutoFixture.Xunit2/`Microsoft.NET.Test.Sdk` 17.12.0).
- `CluedIn.Testing.Base`/`CluedIn.CrawlerIntegrationTesting` switched to the version-suffixed
  package IDs (`.470`/`.480`/`.500`) per the GoogleMaps precedent — confirmed all three exist on the
  develop feed and restore cleanly before wiring up.
- No extra NuGet feed needed — `4.7.0`/`4.8.0` restore cleanly against this repo's existing feeds
  (`develop`/`release`/`AzurePipelines`/`nuget.org`).

---

## Step 4 — Test projects

Status: **Done**

- `test/Directory.Build.props` stripped to `IsTestProject` + `coverlet.msbuild`/
  `Microsoft.NET.Test.Sdk`/`Moq` only — removed the unconditional `xunit.v3`/`AutoFixture.Xunit3`
  references (would've clashed, CS0433, with a conditional xunit v2 `ItemGroup` added to the
  consuming csproj).
- Deleted `test/unit/Directory.Build.props` — dead scaffold, no csproj under `test/unit` consumes
  it (same finding as GoogleMaps' repo).
- Added conditional xunit v2/v3 + AutoFixture `ItemGroup`s directly to
  `ExternalSearch.CVR.Integration.Tests.csproj`, and switched its `CluedIn.Testing.Base`/
  `CluedIn.CrawlerIntegrationTesting` references to the suffixed package IDs.
- No `GlobalUsings.cs` needed — this test project doesn't use `AutoFixture` attributes or
  `ITestOutputHelper` directly, so there's no namespace divergence to bridge.
- Verified with real `dotnet test` runs (not just `dotnet build`) on both `5.0.0-*`/net10.0 and
  `4.7.0`/net6.0 — both non-skipped-by-default tests are actually `[Fact(Skip = ...)]`, so both legs
  report `Skipped: 2, Failed: 0`.

---

## Step 5 — API compatibility audit across 4.7.0 / 4.8.0 / 5.0.0-beta.*

Status: **Done**

Built every `src/` project for real against all three legs, 0 errors. Two real breaks found and
fixed:

### RestSharp 106-vs-114 (same family as GoogleMaps/OpenCorporates/Permid/Brreg)

Source was written against RestSharp 114+ (`Method.Post`/`.Get` PascalCase, `RestClientOptions`,
`RestResponse<T>`), which breaks on CluedIn 4.7/4.8's RestSharp 106.15.0 (`Method.POST`/`.GET`
uppercase, no `RestClientOptions`, `IRestResponse<T>`). Confirmed the exact 106.15.0 API surface via
PowerShell reflection against the restored DLL rather than guessing (`RestClient` has only
parameterless/`Uri`/`string` constructors; auth goes through `Authenticator =
new HttpBasicAuthenticator(user, pass)` instead of `RestClientOptions.Credentials`).

Centralized the fix in a new `Client/RestSharpCompat.cs` (4 call sites across
`CVRExternalSearchProvider.cs` and `Client/CvrClient.*.cs` all shared the same pattern, so one
helper beat repeating the `#if` four times):
- `RestSharpCompat.HttpGet`/`HttpPost` — `const Method` guarded by `CLUEDIN_V50`.
- `RestSharpCompat.CreateClient(Uri, NetworkCredential)` — builds via `RestClientOptions` (5.0+) or
  `RestClient` + `HttpBasicAuthenticator` (pre-5.0).
- A `using CvrCompanyResponse = ...` type alias in `CvrClient.Cvr.cs` for the
  `RestResponse<CompanyResult>`/`IRestResponse<CompanyResult>` split, and a `CvrRestResponseBase`
  alias in `CVRExternalSearchProvider.cs` for the non-generic `RestResponse`/`IRestResponse` split
  used by `ConstructVerifyConnectionResponse`.

### Nager.PublicSuffix 2.4.0-vs-3.8.0 (transitive dependency, same category Brreg's migration found)

`Nager.PublicSuffix` resolves to 2.4.0 pre-5.0 vs 3.8.0 at 5.0+ — confirmed via PowerShell
reflection against both cached DLLs, not assumed:
- Namespaces moved: `SimpleHttpRuleProvider` lives at root in 2.4.0 (named `WebTldRuleProvider`
  there) but under `Nager.PublicSuffix.RuleProviders` in 3.8.0; `ParseException` is at root in
  2.4.0 but under `Nager.PublicSuffix.Exceptions` in 3.8.0.
- `DomainInfo.TLD` (2.4.0) renamed `DomainInfo.TopLevelDomain` (3.8.0).

Fixed in `Net/DomainName.cs` with `#if CLUEDIN_V50` on the `using`s and the rule-provider
construction, plus a new `DomainName.GetTopLevelDomain(DomainInfo)` helper (used by
`CVRExternalSearchProvider.cs:125`) so the property-name split doesn't leak into a lambda expression.

---

## Step 6 — Reset the semantic version (`GitVersion.yml`)

Status: **Done**

```yaml
next-version: 1.0
...
ignore:
  sha: []
  commits-before: 2026-08-01T00:00:00
```

**New gotcha, not found by any prior repo in this batch:** this repo's `GitVersion.yml` already had
its own `ignore: sha: []` block near the bottom of the file. My first attempt added a *second*,
separate `ignore: commits-before: ...` block higher up — valid YAML syntax, no parse error, but
**YAML silently lets the later duplicate key win**, so the pre-existing `ignore: sha: []` at the
bottom completely clobbered my `commits-before` setting. Every value I tried (1 day, 2 days, even 6
weeks past the highest tag) had zero effect for this reason — not a timezone issue this time, a
duplicate-key issue. **Fixed by merging `commits-before` into the existing `ignore:` block** instead
of adding a new one. Lesson for the remaining repos: check for an existing `ignore:` key in the
target `GitVersion.yml` before adding one — grep the whole file for `^ignore:`, not just the top of
it, since some repos have it at the bottom.

Verified with the pipeline's actual pinned `GitVersion.Tool 5.9.0` (installed to a scratch
tool-path) — resolves to `MajorMinorPatch: "1.0.0"`, `SemVer: "1.0.0-multi-version-targeting.109"`.
Cleared `.git/gitversion_cache` between attempts to rule out stale caching as a red herring (it
wasn't the cause here, but worth ruling out explicitly rather than assuming).

---

## Step 7 — Push and confirm CI

Status: *(updated after pushing)*

---

## Checklist

- [x] `azure-pipelines.yml` — switched to `crawler.build.jobs.yml`; `runIntegrationTests` defaulted true; dead `integration-test.ps1` references removed; pool switched to `ubuntu-22.04`
- [x] `Directory.Build.props` — `CluedInMultiVersionTargetFramework` honoured; `DefineConstants` derived; `LangVersion` pinned to 13.0
- [x] `NuGet.config` — renamed from `Nuget.config`; feeds confirmed sufficient
- [x] `Packages.props` — `_CluedIn` guarded; test packages split by `CLUEDIN_V50`; testing-support packages switched to suffixed IDs
- [x] Test projects — `test/Directory.Build.props` stripped; dead `test/unit/Directory.Build.props` deleted; conditional xunit v2/v3 wired into the integration test csproj; real `dotnet test` passes (skipped, not failed) on both 4.7.0/net6.0 and 5.0.0-beta.*/net10.0
- [x] Source — RestSharp 106-vs-114 break fixed via new `RestSharpCompat.cs`; `Nager.PublicSuffix` 2.4.0-vs-3.8.0 break fixed in `DomainName.cs`; all `src/` projects build 0 errors on all three legs
- [x] `GitVersion.yml` — duplicate `ignore:` key bug found and fixed; resolves to real `1.0.0`, verified with pinned GitVersion.Tool 5.9.0
