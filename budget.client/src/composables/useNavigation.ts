import { useRoute, useRouter } from 'vue-router';
import { routes } from '@/router.ts';

// INFO: createWebHistory stores a monotonic position in history.state, but it's seeded from
// window.history.length on load, which also counts entries from before the app was opened.
// We record that as a baseline so depth reflects only navigation done within the app.
let baselinePosition: number | null = null;

const getNavigationDepth = (): number => {
    const currentPosition: number = window.history.state?.position ?? 0;
    baselinePosition ??= currentPosition;
    return currentPosition - baselinePosition;
};

const useNavigation = () => {
    const route = useRoute();
    const router = useRouter();

    const canGoBack = (): boolean => {
        return getNavigationDepth() > 0 || route.meta.back !== undefined;
    };

    const goBack = (): void => {
        if (getNavigationDepth() > 0) {
            router.back();
        }
        else {
            const target = route.meta.back ?? routes.home;
            router.replace(target);
        }
    };

    return { canGoBack, goBack };
};

export default useNavigation;
