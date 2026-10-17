import { Configuration } from "@infrastructure/apis/client";

/**
 * This function returns the configuration to the generated API client with the authentication header.
 */
export const getAuthenticationConfiguration = (token: string | null) => new Configuration(token !== null ? { accessToken: token } : {});