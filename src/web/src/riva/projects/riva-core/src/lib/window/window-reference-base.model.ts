export abstract class WindowReferenceBase {
    get nativeWindow(): Window {
        throw new Error('Not implemented.');
    }
}
