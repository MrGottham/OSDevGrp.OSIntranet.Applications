# Diary: Accounting Posting Journal Updates — ImageSharp Build

Implement the ImageSharp 4 upgrade and secure license handling slice of the accounting-posting-journal-updates PRD.

## Step 1: Upgrade ImageSharp and Secure License Handling

**Author:** main

### Prompt Context

**Verbatim prompt:** `imagesharp`
**Interpretation:** Implement the selected ImageSharp/license-handling iteration from the accounting posting journal updates task.
**Inferred intent:** Upgrade BFF DomainServices to ImageSharp 4 while keeping the acquired license available to local and Docker builds without including it in source control, Docker contexts, published output, or images.

### What I did

Upgraded `SixLabors.ImageSharp` to 4.1.2 and `SixLabors.ImageSharp.Drawing` to 3.1.2. Updated `CaptchaGenerator` to use the Drawing 3 canvas-paint API and the current font measurement APIs while retaining its image dimensions, colors, random noise, and character placement behavior. Configured the project-relative `SixLaborsLicenseFile` property for local builds.

Excluded `/licenses/` from the Docker build context and declared the Compose file secret sourced from `./licenses/sixlabors.lic`. Enabled the Dockerfile frontend with BuildKit syntax and mounted the secret only in the BFF WebApi publish command, overriding the property with `/run/secrets/sixlabors.lic`. Added the `docker compose build builder` instruction beside that publish step.

### Why

ImageSharp 4 and Drawing 3 are the required compatible package versions. The BuildKit mount limits license availability to the BFF publish process and prevents the ignored license from entering the normal build context, publish output, or runtime image.

### What worked

The BFF DomainServices unit-test project passed all 2,222 unit tests. `docker compose config --quiet` passed. `docker compose build builder` and `docker compose build bff` both completed successfully. Container checks confirmed that the secret path and license files were absent from the builder image, BFF publish output, and BFF runtime `/app` directory.

### What didn't work

The first test build failed because Drawing 3 and Fonts 3 removed the old `IImageProcessingContext.Fill`, `DrawLine`, and `DrawText` extensions and the previous `GlyphBounds`/`TryMeasureCharacterSizes` APIs. Migrating `CaptchaGenerator` to `Paint`, `DrawingCanvas`, `RichTextOptions`, and `TextMeasurer.MeasureBounds` resolved those compile errors.

The first Compose configuration check failed with `secrets.mvc.environment must be a string` because the new top-level `secrets` block had been placed inside `services`. Moving it after the service declarations resolved the YAML structure.

### What I learned

ImageSharp.Drawing 3 uses a canvas supplied through `IImageProcessingContext.Paint`; font measurements now return `FontRectangle` values through `TextMeasurer.MeasureBounds`. Compose build secrets must be declared at the top level and separately attached to the builder's `build.secrets`.

### What was tricky

The package upgrade changed both drawing and font APIs, so a simple version bump could not preserve compilation. The secret also had to be scoped to just the BFF publish operation because the builder publishes several applications in separate Dockerfile steps.

### What warrants review

Confirm the selected package versions remain the intended supported versions, and review the updated CAPTCHA render path for visual equivalence. The container checks validated the license's absence from generated output and runtime paths without reading or exposing its contents.

### Future work

The remaining independent PRD slice is end-to-end posting-line deletion through the DomainServices command, authorized BFF WebApi endpoint, and React application.
