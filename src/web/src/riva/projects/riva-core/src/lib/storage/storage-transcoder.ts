export class JsonStorageTranscoder {
    public static encode<T>(value: T): string {
        return JSON.stringify(value);
    }

    public static decode<T>(value: string): T {
        try {
            return JSON.parse(value);
        } catch (error) {
            return undefined;
        }
    }
}
