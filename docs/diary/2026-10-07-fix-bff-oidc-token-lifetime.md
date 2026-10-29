# Diary: Fix BFF OIDC Token Lifetime

Correct the BFF's cached access-token expiry and stop access-token or ID-token expiry from shortening the configured login session.

## Step 1: Separate Token and Session Lifetimes

**Author:** main

### Prompt Context

**Verbatim prompt:** Start implementation
**Interpretation:** Implement the agreed plan: calculate cached access-token expiry from OIDC `expires_in`, and keep the BFF login session at its configured 60 minutes.
**Inferred intent:** Prevent the BFF from treating its access token as expired at the ID-token expiry and avoid logging users out earlier than the configured session duration.

### What I did

Updated `Program.cs` to resolve the stored access-token expiry from the OIDC token response's `ExpiresIn` value using `TimeProvider`. Added `AccessTokenExpirationResolver`, which accepts only positive invariant integer seconds and fails clearly for a missing or invalid value. Disabled `UseTokenLifetime` so the cookie scheme controls the authentication ticket lifetime. Updated `CookieSigningInContextExtensions` to use the ticket's `ExpiresUtc` rather than `.Token.expires_at` when setting the browser cookie expiry, while continuing to clear token items from authentication properties. Added resolver and cookie-expiry unit tests.

### Why

The validated OIDC security token is the ID token, whose `ValidTo` is not authoritative for the access token. Likewise, access-token expiry must not implicitly determine the user's login-session duration.

### What worked

The resolver correctly calculates UTC expiry from `expires_in`; the cookie regression test confirms a shorter `.Token.expires_at` no longer overrides the authentication ticket expiry. All 694 BFF WebApi unit tests passed, and the full solution build succeeded.

### What didn't work

The first resolver test compile failed because its namespace shadowed the resolver type; renaming the namespace fixed it. The initial cookie test used the wrong `CookieSigningInContext` constructor signature; the compiler showed the ASP.NET Core 10 signature, which the test now uses. Its first assertion exposed whole-second cookie expiry precision; the test now compares the time delta directly. The VS Code test tool did not discover the new test file, so validation used `dotnet test` with a fully qualified name filter. An initial filter used the old namespace and selected zero cases; the corrected filter selected and passed all six resolver cases.

### What I learned

The BFF cookie sign-in extension independently overrides browser-cookie expiry from `.Token.expires_at`; disabling `UseTokenLifetime` alone would not have decoupled the browser cookie from the access token.

### What was tricky

The OIDC token response's `expires_in` is a string, and may be absent or malformed. The implementation intentionally fails clearly rather than silently falling back to the validated ID token's expiry.

### What warrants review

Confirm the configured OIDC authority always supplies a positive `expires_in`; otherwise sign-in now fails rather than storing an access token with a misleading expiry. Confirm the 60-minute cookie policy is the intended session policy.

### Future work

No follow-up is required for this change. Refresh-token or delegated-token renewal behavior is unchanged.