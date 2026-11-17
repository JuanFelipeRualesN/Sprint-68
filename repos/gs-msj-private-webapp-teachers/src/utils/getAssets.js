import Settings from "../custom-classes/settings";

const assetModules = require.context("../assets/images", true, /\.(png|jpe?g|svg|mp4)$/);

// Get an asset by version and base name
export const getAsset = (baseName) => {
  const extensions = [".svg", ".png", ".jpg", ".jpeg", ".mp4"];
  const pathsToTry = (ext) => [
    `./${Settings.PROJECT_VERSION}/${baseName}${ext}`,
    `./common-icons/${baseName}${ext}`,
  ];

  for (const ext of extensions) {
    for (const path of pathsToTry(ext)) {
      try {
        return assetModules(path);
      } catch {
        // Ignore
      }
    }
  }

  console.warn(`Asset not found: ${baseName} (version: ${Settings.PROJECT_VERSION})`);
  return null;
};
