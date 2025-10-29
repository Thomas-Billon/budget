import type { RouteParamsGeneric } from "vue-router";

const getIdFromRoute = (param?: RouteParamsGeneric[keyof RouteParamsGeneric]): number | undefined => {
    if (param === undefined || typeof param !== 'string') {
        console.error('Error: param needs to be a valid id.');
        return;
    }

    const id = parseInt(param);

    if (isNaN(id) || id <= 0) {
        console.error('Error: param needs to be a valid id.');
        return;
    }

    return id;
};

export { getIdFromRoute };
