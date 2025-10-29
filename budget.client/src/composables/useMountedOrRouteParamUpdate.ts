import { onMounted, watch } from 'vue'
import { type RouteParamsGeneric, useRoute } from 'vue-router'

const useMountedOrRouteParamUpdate = (callback: (params: RouteParamsGeneric) => void): void => {
    const route = useRoute();

    onMounted(() => callback(route.params));
    watch(() => route.params, (params) => callback(params));
}

export default useMountedOrRouteParamUpdate;
