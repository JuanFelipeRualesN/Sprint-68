import Settings from "../../custom-classes/settings";


/// Mirrors what useVersionedTranslations does for react-localize-redux
/// shared keys stay in the base en/ and es/ folders, 
// and versions/<version>/ merges its overrides on top.
const loadVersionedTranslations = (i18n) => {
  const version = Settings.PROJECT_VERSION;
  if (!version) return;

  let context;

  try {
    context = require.context("./versions", true, /\.json$/);
  } catch (e) {
    // Use the base translations if the version folder doesn't exist
    return;
  }

  let applied = 0;

  context.keys().forEach((key) => {
    const match = key.match(/^\.\/([^/]+)\/([^/]+)\/(.+)\.json$/);
    if (!match) return;

    const [, fileVersion, language, namespace] = match;
    if (fileVersion !== version) return;

    const resources = context(key);
    i18n.addResourceBundle(
      language,
      namespace,
      resources.default || resources,
      true, // deep: merge into the existing bundle
      true // overwrite: version keys win over the base ones
    );
    applied += 1;
  });

  if (applied === 0 && process.env.NODE_ENV === "development") {
    console.warn(`No i18next overrides found for version "${version}".`);
  }
};

export default loadVersionedTranslations;
