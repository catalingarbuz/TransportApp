import { LoginDTO, UserAddDTO } from "../client/models";
import { AuthorizationApi } from "../client/apis";
import { UserApi } from "../client/apis";
/**
 * Use constants to identify mutations and queries.
 */
const loginMutationKey = "loginMutation";
const registerMutationKey = "registerMutation";

/**
 * Returns the an object with the callbacks that can be used for the React Query API, in this case just to login the user.
 */
export const useLoginApi = () => {
    const loginMutation = (loginDTO: LoginDTO) => new AuthorizationApi().apiAuthorizationLoginPost({ loginDTO }); // Use the generated client code and adapt it.

    return {
        loginMutation: { // Return the mutation object.
            key: loginMutationKey, // Add the key to identify the mutation.
            mutation: loginMutation // Add the mutation callback.
        }
    }
}

export const useRegistrationApi = () => {
    const registerMutation = (userAddDTO: UserAddDTO) => new UserApi().apiUserAddPost({ userAddDTO });

    return {
        registerMutation: {
            key: registerMutationKey,
            mutation: registerMutation
        }
    }
}