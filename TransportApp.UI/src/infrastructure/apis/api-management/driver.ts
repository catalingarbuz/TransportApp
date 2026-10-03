import { useAppSelector } from "@application/store";
import { DriverAddDTO, DriverApi } from "../client";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";

const getDriversQueryKey = "getDriversQuery";
const addDriverMutationKey = "addDriverMutation";

export const useDriverApi = () => {
    const { token } = useAppSelector(x => x.profileReducer);
    const config = getAuthenticationConfiguration(token);
    const getDrivers = (page: { page?: number; pageSize?: number; search?: string }) => new DriverApi(config).apiDriverGetPageGet(page);
    const addDriver = (driver: DriverAddDTO) => new DriverApi(config).apiDriverAddPost({ driverAddDTO: driver });

    return {
        getDrivers: { key: getDriversQueryKey, query: getDrivers },
        addDriver: { key: addDriverMutationKey, mutation: addDriver }
    };
};