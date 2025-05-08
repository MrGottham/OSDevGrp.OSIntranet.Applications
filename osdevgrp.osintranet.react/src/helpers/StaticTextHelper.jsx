export function getStaticTextByKey(staticTexts, key) {
    const found = staticTexts.find(item => item.key === key);
    if (found) {
        return found.text;
    }

    throw Error("No static text was found for the key named: " + key);
}