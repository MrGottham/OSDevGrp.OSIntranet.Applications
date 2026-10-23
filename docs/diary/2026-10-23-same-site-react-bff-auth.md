# Diary: Same-site React and BFF authentication

Use the same React bundle on `next.osdevgrp.dk`, `mrgottham.dk`, and `formindskmadspild.dk` while routing browser API and OpenID Connect traffic through each frontend origin to the BFF.

## Step 1: Routing React requests through the same origin

**Author:** main

### Prompt Context

**Verbatim prompt:** Prøv at lave disse rettelser
**Interpretation:** Update the React application and repository reverse-proxy/build configuration to support cookie authentication when the same application runs on multiple domains.
**Inferred intent:** Avoid relying on third-party cookies by making API and OpenID Connect callback traffic same-origin with the React frontend.

### What I did

Changed `ServiceBase` to use relative API URLs, adjusted Vite so production builds no longer require a BFF URL while local development retains its configurable `/api` proxy, removed the now-unneeded BFF endpoint build/runtime setting from the React Docker configuration, and added Nginx forwarding for `/api/` and the OpenID Connect callback paths from the React listener to the BFF. The React Nginx routes forward the original host and scheme.

### Why

Relative API URLs let the same static React bundle work on all three frontend domains. A reverse proxy keeps authentication cookies first-party to the domain serving React and avoids browser restrictions on cross-site cookies.

### What worked

`npm run build` passed from `/home/os/osdevgrp/OSDevGrp.OSIntranet.Applications/osdevgrp.osintranet.react`. `npm run lint` passed with no errors. `docker compose config --quiet` passed.

### What didn't work

The first build and lint commands were run from the repository root and failed with `npm error code ENOENT` because there is no root `package.json`; rerunning them from the React project directory passed. Local Nginx syntax validation could not run because `nginx executable not installed`.

### What I learned

The React project already uses `/api` paths and includes credentials in its fetch requests. The BFF validates absolute login return URLs against `Security:TrustedDomainCollection`, so production configuration must contain the frontend hostnames. The OIDC provider must accept callbacks on each frontend hostname because the callback must pass through the same-origin proxy.

### What was tricky

The OIDC callback does not use the `/api/` prefix. The proxy must therefore forward the default `/signin-oidc` callback (and the sign-out callback) as well as the API route. The production hosting proxy must apply equivalent routing if it is separate from this repository's Nginx configuration.

### What warrants review

Confirm the production ingress forwards `/api/`, `/signin-oidc`, and `/signout-callback-oidc` to the BFF while serving React for other paths. Confirm it preserves the original host and HTTPS scheme. Add `mrgottham.dk` and `formindskmadspild.dk` (and any `www` hosts in use) to `securityTrustedDomainCollection`, and register the corresponding callback URLs with the OpenID Connect provider.

### Future work

Exercise login, logout, antiforgery validation, and cookie behavior through the deployed ingress on each hostname.

## Step 2: Reverting the repository Nginx changes

**Author:** main

### Prompt Context

**Verbatim prompt:** Lad os lave en undo i forhold til nginx.conf
**Interpretation:** Undo only the Nginx configuration edits made during the previous step.
**Inferred intent:** Keep the Azure/prod routing decision separate from this repository's Nginx configuration.

### What I did

Removed the `/api/`, `/signin-oidc`, and `/signout-callback-oidc` proxy locations added to the React listener, and restored its original `X-Forwarded-Host $server_name` header. The other React and Docker changes were left intact.

### Why

The user requested an undo specifically for `nginx.conf`, without reverting the rest of the same-origin React changes.

### What worked

`git diff --check` passed, and `nginx.conf` no longer appears as modified in `git status`.

### What didn't work

Nothing failed during the undo or its verification.

### What I learned

The Azure routing configuration is managed outside this repository's Nginx configuration.

### What was tricky

Only the React-listener additions were reverted; the other pre-existing Nginx proxy blocks were preserved.

### What warrants review

The production Azure ingress still needs the appropriate same-origin routing if the relative React API URLs are retained.

### Future work

Decide and configure the Azure ingress routing independently of this repository's Nginx setup.

## Step 3: Routing React API and OIDC requests to the BFF

**Author:** main

### Prompt Context

**Verbatim prompt:**

> Der må være noget som er gået helt galt for os. Når jeg starter min react applikation o i Rancher Desktop får jeg fejl: (indeks):1 Unchecked runtime.lastError: The message port closed before a response was received.
> installHook.js:1 SyntaxError: Unexpected token '<', `"<!doctype "...` is not valid JSON (at VM37:1:1)
> installHook.js:1 Error while populating error content: SyntaxError: Unexpected token '<', `"<!doctype "...` is not valid JSON (at VM38:1:1)

**Interpretation:** Diagnose the React JSON parsing failure when running the app with Rancher Desktop.
**Inferred intent:** Ensure the local reverse proxy sends API and authentication callback traffic to the BFF instead of the static React server.

### What I did

Restored BFF proxy locations for `/api/`, `/signin-oidc`, and `/signout-callback-oidc` in the React listener in `/Nginx/nginx.conf`. The routes preserve the incoming host, including its port, and HTTPS scheme in forwarded headers. Rebuilt the `reverseproxy` image.

### Why

The React listener previously forwarded every path to the static React server. Requests to `/api/...` therefore received `index.html`, which caused `Unexpected token '<'` when the frontend parsed the response as JSON.

### What worked

`docker compose build reverseproxy` succeeded. `nginx -t` passed in the built image when the Compose upstream names were temporarily mapped for the syntax check. `docker compose config --quiet` and `git diff --check` passed.

### What didn't work

`docker compose run --rm --no-deps reverseproxy nginx -t` failed because the Compose services were not running and Nginx could not resolve `mvc:8080`. Re-running the syntax test with temporary upstream host mappings passed. `docker compose ps --all` showed no running services in this environment, so no live end-to-end request was tested.

### What I learned

The HTML response was produced by the React SPA fallback, not by a JSON API error. The browser-extension `runtime.lastError` is unrelated to that response parsing failure.

### What was tricky

OIDC login correlation depends on preserving the frontend host through the initial challenge and forwarding the callback through that same frontend host. The forwarded host therefore preserves the port as well.

### What warrants review

Recreate the `reverseproxy` service so the newly built image is used, then confirm `/api/security/...` returns JSON and test login and logout through the React hostname.

### Future work

Verify the routing and authentication end-to-end in the user's Rancher Desktop stack.

## Step 4: Using direct BFF access when Nginx is not in the Rancher path

**Author:** main

### Prompt Context

**Verbatim prompt:** Jeg, det vil være korrekt at ændre dette, hvis nginx også blev brugt i mit rancher desktop setup. Det gør den så ikke. Her kører vi applikationerne på et netværk i Rancher Desktop og bruger ikke nginx containern.
**Interpretation:** The Rancher Desktop deployment does not route browser requests through this repository's Nginx container.
**Inferred intent:** Configure the React container to call the BFF endpoint directly, rather than sending relative API paths to the React static server.

### What I did

Restored `VITE_BFF_ENDPOINT` handling in `ServiceBase`, the Vite development proxy, the React Docker image build argument, and the React Compose build/runtime settings. Removed the Nginx routes that I had just added for `/api/` and the OIDC callbacks. The React container's configured BFF endpoint is therefore baked into the bundle, and the browser calls that BFF origin directly.

### Why

Without the Nginx proxy, a relative `/api/...` request goes to the React static server, which responds with the SPA `index.html` rather than API JSON. Direct BFF URLs restore the deployment model the Rancher setup requires.

### What worked

The React production build passed with `VITE_BFF_ENDPOINT=https://bff.osdevgrp.localhost`. React lint passed with only the two previously existing Fast Refresh warnings. Compose configuration validation passed when `bffEndpointAddress` was explicitly set, and `nginx.conf` no longer contains the newly introduced routes.

### What didn't work

Initial npm validation was run from the repository root and failed with `ENOENT` because `package.json` is under the React project. A Compose validation without `bffEndpointAddress` emitted warnings that it was unset; validation with an explicit endpoint passed. The first whitespace check found trailing whitespace in a quoted transcript line, which was removed.

### What I learned

The BFF endpoint must be supplied when building the static React bundle: setting a container environment variable alone cannot change already-built JavaScript. The local Vite dev server continues to use that endpoint as its proxy target, while the React bundle itself uses it directly in non-development mode.

### What was tricky

Direct browser-to-BFF requests are cross-origin and therefore depend on a valid BFF CORS policy. A single statically built endpoint also cannot vary automatically between different BFF hostnames on different domains.

### What warrants review

Set `bffEndpointAddress` to the address reachable by the browser for the corresponding deployment, rebuild the React image, and ensure the BFF CORS origins include the React origin. This Rancher correction does not by itself solve third-party-cookie restrictions when a frontend and BFF are on different sites in production.

### Future work

Determine how production traffic will provide same-site routing or BFF host aliases for each frontend domain.

## Step 5: Resolving the BFF host from the active frontend domain

**Author:** main

### Prompt Context

**Verbatim prompt:** Jeg kan sagten få samme bff til at køre på 3 forskellige domænenavne i Azure. Jeg ved bare ikke, hvordan vi bedst muligt får react applikationen til at køre med den rigtige bff baseret på domænenavnet
**Interpretation:** Use one React build on all frontend domains and select the corresponding BFF host at runtime.
**Inferred intent:** Avoid per-domain React builds while keeping browser-to-BFF requests same-site through a BFF hostname on each domain.

### What I did

Added an explicit frontend-host-to-BFF-host map in `ServiceBase`: the `next` domains map to `bff.osdevgrp.*`, while the two apex domains and their `www` variants map to their respective `bff` subdomains. The scheme is taken from the active page. Kept `VITE_BFF_ENDPOINT` for Vite development proxying only, and removed it from the static React image build/runtime settings so one built bundle selects its BFF at runtime.

### Why

A static `VITE_BFF_ENDPOINT` is fixed during build and cannot vary when the same asset bundle is served on different domains. The browser hostname is available at runtime, so an explicit allowlisted mapping selects the intended BFF without rebuilding React.

### What worked

`npm run build`, `npm run lint`, and `docker compose config --quiet` passed. Lint reports only the two pre-existing Fast Refresh warnings.

### What didn't work

Nothing failed during implementation or validation.

### What I learned

This solution depends on each mapped BFF hostname resolving to the same Azure BFF service, with a valid TLS certificate and the correct host forwarded to the application. The browser requests remain cross-origin, so CORS still needs the exact React origins.

### What was tricky

The BFF origin must be selected from a fixed host allowlist rather than synthesized for arbitrary frontend hosts. Each BFF hostname will issue its own host-scoped auth cookie; cookies cannot be shared across the separate registrable domains.

### What warrants review

Verify the exact production hostnames and any aliases. Add all React origins to the BFF CORS allowlist, add frontend hosts to its trusted return-domain list, and register all BFF callback URLs with the identity provider. Test each domain through its TLS-enabled BFF hostname.

### Future work

Validate the new runtime host mapping and complete sign-in flow from each deployed frontend hostname.

## Step 6: Moving BFF host mappings into runtime configuration

**Author:** main

### Prompt Context

**Verbatim prompt:** Det ser lovende ud og har nu det oppe at køre i min Rancher Desktop. Jeg er dog ikke vil med at hardcode domænenavne og bff endpoint i resolveBffHost. Vil vi kunne lave en form for konfiguration til dette
**Interpretation:** Make the frontend-host-to-BFF mapping configurable outside the service implementation.
**Inferred intent:** Allow BFF hostnames to be changed without editing application logic or rebuilding domain-specific bundles.

### What I did

Added `public/runtime-config.json` containing the frontend hostname to BFF-origin map and a runtime configuration loader that validates HTTPS origins. React now loads this file before rendering the application, and `ServiceBase` resolves the BFF from the loaded configuration instead of a hardcoded mapping. Vite development continues using its dev proxy without requiring the runtime config.

### Why

Keeping deployment hostnames in a runtime-served JSON file separates infrastructure configuration from service logic and allows one React build to select a BFF endpoint based on the current hostname.

### What worked

The React production build passed. The lint command passed with only the two existing Fast Refresh warnings. A Node smoke test validated two configured host lookups, case normalization, and rejection of an unknown host. Docker Compose configuration validation passed.

### What didn't work

Nothing failed during implementation or validation.

### What I learned

The static asset server needs to serve `/runtime-config.json` from the React root. The configuration is fetched with `cache: no-store`, so edits to the file are not hidden by browser HTTP caching.

### What was tricky

The application must load and validate runtime configuration before constructing services, otherwise initial API calls could be made before a BFF origin is known. Startup now displays an explicit error if configuration cannot be loaded.

### What warrants review

Verify the exact frontend and BFF URLs in `public/runtime-config.json` for the target environment. If the same image is deployed with different host mappings, provide an environment-specific file at `/runtime-config.json` (for example, by mounting or replacing the file) so the mapping can change without rebuilding the React bundle.

### Future work

Configure the Azure deployment to serve the appropriate runtime configuration and verify the login flow on each hostname.

## Step 7: Supplying the runtime BFF map through container environment

**Author:** main

### Prompt Context

**Verbatim prompt:** Denne løsning er jeg ikke så glad for. Vi har brug for en god løsning, som passer ind i vores docker compose setup og gerne med brug af ENV som jeg let kan konfigurere i både Rancher desktop og på Auzre
**Interpretation:** Supply the runtime host-to-BFF mapping through the React container's environment.
**Inferred intent:** Keep one React image and configure endpoint routing consistently in Rancher Desktop and Azure without maintaining domain-specific frontend builds.

### What I did

Added the `BFF_ENDPOINTS` environment variable to the React Compose service. Added a Node startup configurator and shell entrypoint that validate the JSON mapping and write `/app/runtime-config.json` before Supervisor launches the static server. Removed the checked-in runtime mapping file, leaving the React build independent of deployment endpoint values.

### Why

The container environment is available in both Docker Compose and Azure container hosting. Generating the runtime JSON at container startup keeps the same built frontend image configurable per deployment without embedding domains in the application code or image build.

### What worked

The React build and lint passed. Compose interpolated the JSON environment value correctly. A full React executor image build succeeded, and a container startup test generated the expected `/app/runtime-config.json` from `BFF_ENDPOINTS`. The temporary validation image was removed afterward.

### What didn't work

Nothing failed during implementation or validation.

### What I learned

`BFF_ENDPOINTS` is a public routing configuration, not a secret. Each running React container needs mappings for every frontend hostname served by that container.

### What was tricky

The static file server runs as a non-root user, so `/app` must remain group-writable for the entrypoint to generate the configuration file before serving the application.

### What warrants review

Set `BFF_ENDPOINTS` in Rancher Desktop and Azure as a JSON object from browser-facing React hostnames to HTTPS BFF origins. Confirm the BFF CORS and trusted-return-domain settings and identity-provider callback URLs are configured for the same hosts.

### Future work

Redeploy the updated React image with the environment value set and exercise authentication through each frontend hostname.

## Step 8: Simplifying runtime configuration startup

**Author:** main

### Prompt Context

**Verbatim prompt:** Do we tent to overcomplicate the configruation with .sh file .mjs file etc?
**Interpretation:** Reduce unnecessary startup/configuration files while preserving environment-based BFF mappings.
**Inferred intent:** Keep the deployment mechanism straightforward for Compose and Azure.

### What I did

Removed the separate Node `.mjs` configuration generator. The existing shell entrypoint now checks that `BFF_ENDPOINT_MAPPINGS` is set, writes it into the runtime config JSON envelope, and launches Supervisor. The React runtime loader remains responsible for validating the mapping structure and HTTPS endpoints.

### Why

Only one small container startup adapter is needed to transfer environment configuration into the static files served to the browser. Keeping validation in the React runtime loader avoids duplicating it in a separate Node generator.

### What worked

The React build and lint passed. Compose interpolation passed. The React container built successfully and generated the expected runtime JSON from the renamed environment variable at startup. The temporary validation image was removed.

### What didn't work

Nothing failed during the simplification or validation.

### What I learned

The shell wrapper is the only server-side bridge needed because static JavaScript cannot read container environment variables directly.

### What was tricky

The environment value remains a JSON mapping, so quoting must be preserved when setting it in Compose or Azure. Invalid JSON will be reported by the React runtime loader at application startup.

### What warrants review

Verify that Rancher Desktop and Azure set `BFF_ENDPOINT_MAPPINGS` to valid JSON containing HTTPS origins for each frontend hostname served by the container.

### Future work

Redeploy and verify the runtime configuration in Rancher Desktop and Azure.

## Step 9: Removing the shell entrypoint

**Author:** main

### Prompt Context

**Verbatim prompt:** Tænker, at vi nok heller ikke behøver .sh filen
**Interpretation:** Remove the standalone shell startup script from the React container.
**Inferred intent:** Keep the environment-to-runtime-config startup path to a single file.

### What I did

Replaced the shell entrypoint with one Node ESM entrypoint that validates `BFF_ENDPOINT_MAPPINGS`, writes the runtime JSON, starts Supervisor, and forwards termination signals. Updated the React Dockerfile to use Node directly as the container entrypoint and removed the `.sh` file.

### Why

The Node runtime is already present in the React image. Using it as the single entrypoint avoids an additional shell wrapper while retaining startup validation and correct Supervisor lifecycle behavior.

### What worked

The React image built successfully and a container test confirmed that the Node entrypoint generated the expected runtime JSON. React build/lint, Node syntax validation, Compose configuration validation, and `git diff --check` passed.

### What didn't work

Nothing failed during implementation or validation.

### What I learned

A single Node entrypoint can both prepare configuration and launch the existing Supervisor process; it must forward termination signals so Docker shutdown reaches Supervisor.

### What was tricky

The container runs as a non-root user, so `/app` must remain writable by that user for configuration generation.

### What warrants review

Test normal shutdown and restart behavior in Rancher Desktop after deploying the updated React image.

### Future work

Rebuild and roll out the image in Rancher Desktop and Azure.

## Step 10: Reducing entrypoint validation duplication

**Author:** main

### Prompt Context

**Verbatim prompt:** Synes måske også at .mjs filen er at overkomplicere
**Interpretation:** Simplify the Node container entrypoint after deciding runtime ENV still needs to be supported.
**Inferred intent:** Keep only the minimal server-side logic required to expose container configuration to the static React bundle.

### What I did

Removed duplicate BFF mapping validation from the Node entrypoint. It now only checks that the environment variable exists, parses its JSON, writes the runtime config file, and runs Supervisor. The React runtime configuration loader remains responsible for validating host mappings and HTTPS endpoint URLs.

### Why

The frontend already validates the configuration before starting, so validating its content a second time in the entrypoint was unnecessary. Retaining one Node entrypoint is still required to bridge container ENV into files served by the static site.

### What worked

The React container built and generated the expected runtime JSON from the environment variable. Node syntax, Compose config, and `git diff --check` passed.

### What didn't work

Nothing failed during this simplification or validation.

### What I learned

The minimum runtime configuration bridge still requires code because browser JavaScript cannot access container environment variables directly.

### What was tricky

The container entrypoint also launches Supervisor, so it needs to forward signals and preserve the child process exit status while remaining a single Node file.

### What warrants review

Review the simplified startup behavior and ensure invalid JSON produces a clear container startup error, while structurally invalid mappings are surfaced by the React runtime loader.

### Future work

Rebuild the image to roll out this simplified entrypoint.

## Step 11: Loading React services after runtime configuration

**Author:** main

### Prompt Context

**Verbatim prompt:**

> Virker lokalt, men når jeg kører via rancher desktop med korrekt konfiguration får jeg: `Runtime configuration has not been loaded.`

**Interpretation:** The production React container constructs services before the runtime BFF mapping is available.
**Inferred intent:** Ensure the runtime configuration is loaded before any service instance resolves its BFF endpoint.

### What I did

Changed `main.jsx` to dynamically import `App` only after `loadRuntimeConfiguration()` resolves in production. Local development retains its current direct render path.

### Why

`App` had been statically imported at module initialization. That import evaluated `ServiceContext`, which constructs service instances immediately; those constructors called `getBffEndpoint` before asynchronous configuration loading had completed.

### What worked

The React production build passed and emitted a separate App chunk, confirming the import is deferred. Lint passed with the two existing Fast Refresh warnings, and `git diff --check` passed.

### What didn't work

Nothing failed during the correction or validation.

### What I learned

With ES modules, top-level static imports are evaluated before the importing module's body, regardless of the order of statements in that body. An awaited configuration step therefore requires a dynamic import for modules that instantiate services.

### What was tricky

The ordering requirement applies to module evaluation itself, not just to the React render call; moving render later without deferring the `App` import would not fix the constructor timing.

### What warrants review

Rebuild and redeploy the React image, then check that `/runtime-config.json` loads successfully and that service requests reach the BFF.

### Future work

Verify Rancher Desktop startup and authentication end-to-end with the configured BFF mapping.
