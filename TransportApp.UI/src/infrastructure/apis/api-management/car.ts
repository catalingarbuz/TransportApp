import { useAppSelector } from "@application/store";
import { CarAddDTO, CarApi } from "../client";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";

const getCarsQueryKey = "getCarsQuery";
const addCarMutationKey = "addCarMutation";

export const useCarApi = () => {
    const { token } = useAppSelector(x => x.profileReducer);
    const config = getAuthenticationConfiguration(token);
    const getCars = (page: { page?: number; pageSize?: number; search?: string }) => new CarApi(config).apiCarGetPageGet(page);
    const addCar = (car: CarAddDTO) => new CarApi(config).apiCarAddPost({ carAddDTO: car });

    return {
        getCars: { key: getCarsQueryKey, query: getCars },
        addCar: { key: addCarMutationKey, mutation: addCar }
    };
};