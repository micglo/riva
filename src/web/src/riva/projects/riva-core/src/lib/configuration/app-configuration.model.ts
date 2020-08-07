export interface AppConfiguration {
    api_url: string;
    auth: {
        issuer: string;
        client_id: string;
        redirect_uri: string;
        silent_refresh_redirect_uri: string;
        post_logout_redirect_uri: string;
        scope: string;
    };
}
